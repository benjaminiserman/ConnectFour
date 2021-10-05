using System;

public class WinstonAI : AI
{
	Random random = new Random(); // only needed for random moves

	public override string Name => "WinstonAI"; // make sure you change this

	public WinstonAI() // This code runs when RandomAI is selected.
	{
		Say("I look forward to a good game"); // use Say(string text) to say dialogue!
	}

	public override int Prompt(Board board, int round) // Here is where he thinks. Return an int corresponding to the column you want to drop your next token in. For columns A-F, return numbers 10-15. You can call ConnectLibrary.Dec(string s) to convert a number/letter from hex into decimal 0-15.
	{
		//Team == my team
		//requirements to run AI
		if((board.Rows != 6) || (board.Columns != 7) || (board.TeamCount != 2) || (board.Connect != 4)) throw new Exception ("This is not a standard game. WinstonAI can only be used with a standard board.");
		/*(int x, int y) intTuple;
		intTuple.x = 5;
		intType.y = 3;*/
		for(int i = 0; i < 7; i++)
		{
			Console.WriteLine($"{TopCheck(board)[i]}");
		}
		Console.ReadLine();
		//board[0, 0] == top left
		/*
		CheckDouble(board, Team, new FakeBoard(board));
		CheckDouble(board, Team, null);
		CheckDouble(board, Team);
		All options above will create fakeBoard in CheckDouble.
		To use a previously established array put the name in place of null.
		*/
		return random.Next(board.Columns); // Aaaand he just puts in a random move. Make sure your AI knows how many columns there are, it's not always 7!
	}

	#region methods
	
	private Vector2?[] TopCheck(Board board, FakeBoard fakeBoard = null)
	{
		if(fakeBoard != null)
		{
			Vector2?[] FakeTop = new Vector2?[7];
			for(int x = 0; x < 7; x++)
			{
				int y = 0;
				if(fakeBoard[x, 0] != State.Empty) FakeTop[x] = null;
				else
				{
					while(y < 6 && fakeBoard[x, y] == State.Empty)
					{
						y++;
					}
					y--;
					FakeTop[x] = (x, y);
				}
			}
			return FakeTop;
		}
		//
		Vector2?[] Top = new Vector2?[7];
		for(int x = 0; x < 7; x++)
		{
			int y = 0;
			if(board[x, 0] != State.Empty) Top[x] = null;
			else
			{
				while(y < 6 && board[x, y] == State.Empty)
				{
					y++;
				}
				y--;
				Top[x] = (x, y);
			}
		}
		return Top;
	}

	private Vector2? CheckDouble(Board board, State team)
	{
		//getnextY in library
		//ConnectLibrary.GetNextY(int x, Board board);
		//CheckVictor(int x, int y, State state)
		
		//establish fakeBoard
		//fakeBoard is built starting at the top left to right then going down one and going left to right again
		FakeBoard fakeBoard = new FakeBoard(board);
		//CheckVictor(int x, int y, State state)
		//place a ghost piece in the empty spots one at a time
		//int CheckDirection(int x, int y, int a, int b, State state)
		/*
			int (a, b)
			(1,0) -> check horizontal
			(0,1) -> check vertical
			(1, 1) -> check down the stairs
			(-1, -1) -> check up the stairs
		*/
		//GetNextY(int x) the empty coordinate
		for(int x = 0; x < 7; x++)
		{
			if(TopCheck(board)[x] != null)
			{
				int winCount = 0;
				int y = fakeBoard.GetNextY(x);
				fakeBoard[x, y] = team;
				for(int p = 0; p < 7; p++)
				{
					int o = fakeBoard.GetNextY(p);
					if(o >= 0)
					{
						if(fakeBoard.CheckVictor(p, o, team)) winCount++;
					}
				}
				if(winCount >= 2) return (x, y);
				fakeBoard[x, y] = State.Empty;
			}
		}
		return null;
	}

	private bool[] DoNotMove(Board board, State myTeam, State theirTeam)
	{
		//don't move their == true
		bool[] columns = new bool[7];
		FakeBoard fakeBoard = new FakeBoard(board);
		int x = 0;
		int y;
		for(Vector2 coor = (x, y); coor.x < 7; x++)
		{
			if(TopCheck(board)[coor.x] != null)
			{
				coor.y = fakeBoard.GetNextY(coor.x);
				int winCount = 0;
				fakeBoard[coor] = myTeam;
				if(TopCheck(board, fakeBoard)[coor.x] != null)
				{
					coor.y = fakeBoard.GetNextY(coor.x);
					if(fakeBoard.CheckVictor(coor, theirTeam)) columns[coor.x] = true;
					else
					{
						fakeBoard[coor] = theirTeam;
						for(int p = 0; p < 7; p++)
						{
							int o = fakeBoard.GetNextY(p);
							if(o >= 0)
							{
								if(fakeBoard.CheckVictor(p, o, theirTeam)) winCount++;
							}
						}
						if(winCount > 1) columns[coor.x] = true;
						else columns[coor.x] = false;
						fakeBoard[coor] = State.Empty;
						fakeBoard[coor.x, coor.y + 1] = State.Empty;
					}
				}
				else columns[coor.x] = false;
			}
			else columns[coor.x] = false;
		}

		return columns;
	}

	#endregion
	public override void MatchEnd(State victor, int round) // This is called every time a round ends.
	{
		if (victor == State.Empty) Say("A rare occurence"); // draw dialogue
		else if (victor == Team) Say("Huzzah! I win!"); // win dialogue
		else {
			Say("Unfortunate, I will do better next time"); // loss dialogue
		}
	}

	public override void GameEnd() // This is called at the end of a series of games.
	{
		Say("It was a pleasure playing with you");
	}
}

struct Vector2
{
	public int x, y;

	public Vector2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString()
	{
		return $"({x}, {y})";
	}

	public static Vector2 operator +(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x + b.x, a.y + b.y);
	}
	
	public static explicit operator int(Vector2 p)
	{
		return Int16.Parse($"{p.x}" + $"{p.y}");
	}
	public static implicit operator Vector2((int a, int b) t)
	{
		return new Vector2(t.a, t.b);
	}
	public static implicit operator (int a, int b)(Vector2 p)
	{
		return (p.x, p.y);
	}
}
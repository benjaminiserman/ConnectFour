using System;

public class Board
{
	public int Columns { get; private set; }
	public int Rows { get; private set; }
	public int Connect { get; private set; }
	public int TeamCount { get; private set; }

	private State[,] _gameState;

	public State this[int x, int y]
	{
		get => _gameState[x, y];
		private set => _gameState[x, y] = value;
	}

	public State this[(int x, int y) v] // tuple version
	{
		get => _gameState[v.x, v.y];
		private set => _gameState[v.x, v.y] = value;
	}

	public State CurrentTeam { get; private set; }
	public State StartingTeam { get; private set; }
	public State Victor { get; private set; } = State.Empty;

	public static bool GameInProgress { get; private set; }
	public bool NoMiddleStart { get; private set; }
	public bool EnableCreationAllowed { get; private set; } = false;
	public bool DisableCreationAllowed { get; private set; } = false;

	public bool IsASCII { get; private set; }
	public bool ColorblindEnabled { get; private set; }

	public string MoveHistory { get; private set; } = "";

	private int _round, _auth;
	private bool authSent = false;
	public int Auth
	{
		get
		{
			if (!authSent)
			{
				authSent = true;
				return _auth;
			}
			else return -1;
		}
	}

	public Board(int columns = 7, int rows = 6, int connect = 4, int teams = 2, bool noMiddleStart = false, State currentTeam = State.Red, bool ascii = false, bool colorblind = false)
	{
		if (GameInProgress) throw new Exception("A board is already in use.");

		if (connect > columns && connect > rows) throw new Exception("Connect cannot be larger than columns and rows.");	

		if (teams >= Enum.GetValues(typeof(State)).Length) throw new Exception ("There can be no more than four teams.");

		GameInProgress = true;		

		Columns = columns;
		Rows = rows;
		Connect = connect;
		TeamCount = teams;
		CurrentTeam = currentTeam;
		StartingTeam = currentTeam;
		NoMiddleStart = noMiddleStart;
		IsASCII = ascii;
		ColorblindEnabled = colorblind;

		_gameState = new State[columns, rows];	
		_auth = new Random().Next(int.MaxValue);	
	}

	public void EnableCreation(int auth) 
	{
		if (auth == _auth) 
		{
			EnableCreationAllowed = false;
			GameInProgress = false;
		}
	}

	public void DisableCreation(int auth) 
	{
		if (auth == _auth) 
		{
			DisableCreationAllowed = false;
			GameInProgress = true;
		}
	}

#region GameLogic

	private bool TryDrop(int x, State state)
	{
		if (state == State.Empty) return false;
		if (_gameState[x, 0] != State.Empty) return false;

		int y = Rows - 1;

		while (_gameState[x, y] != State.Empty)
		{
			y--;
		}

		_gameState[x, y] = state;

		MoveHistory += $"{x:X}";

		if (CheckFull()) EndGame();
		
		if (CheckVictor(x, y, _gameState[x, y]))
		{
			Victor = _gameState[x, y];
			EndGame();
		}

		return true;
	}

	public bool CheckFull()
	{
		for (int x = 0; x < Columns; x++)
		for (int y = 0; y < Rows; y++)
		if (_gameState[x, y] == State.Empty) return false;

		return true;
	}

	public bool CheckVictor(int x, int y, State state)
	{
		if (state == State.Empty) return false;

		if (CheckDirection(x, y, 1, 0, state) >= Connect) return true;
		if (CheckDirection(x, y, 0, 1, state) >= Connect) return true;
		if (CheckDirection(x, y, 1, 1, state) >= Connect) return true;
		if (CheckDirection(x, y, 1, -1, state) >= Connect) return true;

		return false;
	}

	public bool CheckVictor((int x, int y) v, State state) => CheckVictor(v.x, v.y, state); // tuple version

	public int CheckDirection(int x, int y, int a, int b, State state)
	{
		if (state == State.Empty) return 0;

		int count = 1;

		int v, w;

		for (int i = 1; i < Connect; i++)
		{
			v = x + (a * i);
			w = y + (b * i);

			if (v >= 0 && v < Columns && w >= 0 && w < Rows && _gameState[v, w] == state) count++;
			else break;
		}

		for (int i = 1; i < Connect; i++)
		{
			v = x + (-a * i);
			w = y + (-b * i);

			if (v >= 0 && v < Columns && w >= 0 && w < Rows && _gameState[v, w] == state) count++;
			else break;
		}

		return count;
	}

	public int CheckDirection((int x, int y) v, (int x, int y) d, State state) => CheckDirection(v.x, v.y, d.x, d.y, state);

	public bool InputMove(int x, int auth, int round)
	{
		if (auth != _auth)
		{
			Console.WriteLine("Wrong auth token.");
			return false;
		}
		if (x < 0 || x >= Columns)
		{
			Console.WriteLine($"Column {x} does not exist.");
			return false;
		}

		if (NoMiddleStart && round == 1 && CurrentTeam == StartingTeam && x == Columns / 2)
		{
			Console.WriteLine("First player cannot start in center!");
			return false;
		}

		_round = round;

		if (TryDrop(x, CurrentTeam))
		{
			CurrentTeam = MainClass.Next(CurrentTeam, TeamCount);
			return true;
		}
		else 
		{
			Console.WriteLine($"Column {x} is full.");
			return false;
		}
	}

	private void EndGame()
	{
		Draw(_round);

		Console.WriteLine("Game Over.");
		if (Victor == State.Empty) Console.WriteLine("Draw.");
		else Console.WriteLine($"{Victor} Team wins!");

		EnableCreationAllowed = true; // gives Game control over this.GameInProgress
		DisableCreationAllowed = true; // same
		GameInProgress = false;
	}

	public void Forfeit()
	{
		if (TeamCount > 2) Victor = State.Empty;
		else Victor = MainClass.Next(CurrentTeam, TeamCount);

		EndGame();
	}

#endregion

#region Graphics

	public void Draw(int round)
	{
		Console.Clear();
		Console.ForegroundColor = ConsoleColor.White;

		Console.WriteLine($"Connect {GetWord(Connect)}");
		Console.WriteLine($"Round: {round}");
		
		WriteHistory();

		DrawTop();

		for(int y = 0; y < Rows; y++)
		{
			Console.Write("|");

			for (int x = 0; x < Columns; x++)
			{
				DrawSlot(_gameState[x, y]);
			}

			Console.WriteLine();
		}

		DrawBottom();
	}

	public static string GetWord(int x)
	{
		switch (x)
		{
			case 0:
				return "Zero";
			case 1:
				return "One";
			case 2:
				return "Two";
			case 3:
				return "Three";
			case 4:
				return "Four";
			case 5:
				return "Five";
			case 6:
				return "Six";
			case 7:
				return "Seven";
			case 8:
				return "Eight";
			case 9:
				return "Nine";
			case 10:
				return "Ten";
			case 11:
				return "Eleven";
			case 12:
				return "Twelve";
			case 13:
				return "Thirteen";
			case 14:
				return "Fourteen";
			case 15:
				return "Fifteen";
			case 16:
				return "Sixteen";
			default:
				return x.ToString();
		}
	}

	private void WriteHistory()
	{
		Console.Write("History: ");

		State team = StartingTeam;

		for (int i = 0; i < MoveHistory.Length; i++)
		{
			Console.ForegroundColor = GetColor(team);
			Console.Write(MoveHistory[i]);
			team = MainClass.Next(team, TeamCount);
		}

		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine();
	}

	private void DrawSlot(State state)
	{
		if (state == State.Empty)
		{
			Console.Write("   ");
		}
		else 
		{
			Console.ForegroundColor = GetColor(state);

			Console.Write($" {GetGlyph(state, ColorblindEnabled, IsASCII)} ");
		}

		Console.ForegroundColor = ConsoleColor.White;

		Console.Write("|");
	}

	public static ConsoleColor GetColor(State state)
	{
		switch (state)
		{
			case State.Yellow:
				return ConsoleColor.Yellow;
			case State.Red:
				return ConsoleColor.Red;
			case State.Green:
				return ConsoleColor.Green;
			case State.Blue:
				return ConsoleColor.Blue;
			case State.Purple:
				return ConsoleColor.Magenta;
			case State.Orange:
				return ConsoleColor.DarkYellow;
			case State.White:
				return ConsoleColor.White;
			case State.Black:
				return ConsoleColor.DarkGray;
			default:
				return ConsoleColor.White;
		}
	}

	public static char GetGlyph(State state, bool colorBlindMode, bool ascii)
	{
		if (!colorBlindMode)
		{
			if (ascii) return 'O';
			else return '⬤';
		}
		else 
		{
			switch (state)
			{
				case State.Red:
					return ascii ? 'O' : '⓵';
				case State.Yellow:
					return ascii ? 'X' : '⓶';
				case State.Green:
					return ascii ? '&' : '⓷';
				case State.Blue:
					return ascii ? '@' : '⓸';
				case State.Purple:
					return ascii ? '#' : '⓹';
				case State.Orange:
					return ascii ? '+' : '⓺';
				case State.White:
					return ascii ? '$' : '⓻';
				case State.Black:
					return ascii ? '?' : '⓼';
				default:
					return ascii ? '!' : '⬤';
			}
		}
	}

	private void DrawTop()
	{
		int width = Columns * 4 + 1;

		for (int i = 0; i < width; i++)
		{
			Console.Write("=");
		}
		Console.WriteLine();
	}

	private void DrawBottom()
	{
		Console.Write("=");

		for (int i = 0; i < Columns; i++)
		{
			Console.Write($"[{i:X}]=");
		}
		Console.WriteLine();
	}

	#endregion
}
using System;

public class DAIgonal : AI
{
	Random random = new Random(); // only needed for random moves
	
	public override string Name => "DAIgonal"; // make sure you change this

	public DAIgonal() // This code runs when RandomAI is selected.
	{
		Say("\nG\n L\n  H\n   F"); // use Say(string text) to say dialogue!
	}

	public override int Prompt(Board board, int round) // Here is where he thinks. Return an int corresponding to the column you want to drop your next token in. For columns A-F, return numbers 10-15. You can call ConnectLibrary.Dec(string s) to convert a number/letter from hex into decimal 0-15.
	{
		/* // Integer Math
		int x = 6;

		x = x + 3;
		x = x * 2;
		x = x - 2;
		x = x / 2; // returns a whole number!
		x = x % 3; // gives remainder of x / 3

		x += 3;
		x *= 2;
		x -= 2;
		x /= 2;
		x %= 2;

		x++;
		x--;
		*/

		/* // string stuff
		string s = "gihljaobinaobnvaiowugnIWAUJn";
		
		s = s + "hello";
		s += "hello";

		s = $"ionfoiwnafiwanA {x} asfawfa"; // x is added to the string in the middle
		*/

		/* // boolean stuff
		bool b = true;
		b = false;

		b = !b; // true -> false, false -> true
		b = true || false; // || returns true if EITHER value is true.
		b = true && false; // && returns true if BOTH values are true.
		b = true ^ false; // returns true if EXACTLY ONE value is true.
		*/

		/*
		x == y
		x > y
		x < y
		x >= y
		x <= y
		*/

		//bool CheckVictor(int x, int y, State state)

		/*
			State.Empty
			State.Red
			State.Yellow
			State.Green
			State.Blue
		*/

		// board[x, y]
		// Team = the team you're on

		// board.Rows // x
		// board.Columns // y

		for (int x = 0; x < board.Columns; x++)
		{
		
		}

		//

		//bool CheckVictor(int x, int y, State state) // checks whether or not the specified team would win if they were to place a token in the specified space.

		return random.Next(board.Columns); // Aaaand he just puts in a random move. Make sure your AI knows how many columns there are, it's not always 7!
	}

	public override void MatchEnd(State victor, int round) // This is called every time a round ends.
	{
		Say("\nG\n G\n  W\n   P");
	}

	public override void GameEnd() // This is called at the end of a series of games.
	{
		Say("\nC\n '\n  Y\n   A");
	}
}
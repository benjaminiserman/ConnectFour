using System;

public class SilviaAI : AI
{
	Random random = new Random(); // only needed for random moves

	public override string Name => "SilviaAI"; // make sure you change this

	public SilviaAI() // This code runs when RandomAI is selected.
	{
		Say("I'm going to kill someone alive"); // use Say(string text) to say dialogue!
	}

	public override int Prompt(Board board, int round) // Here is where he thinks. Return an int corresponding to the column you want to drop your next token in. For columns A-F, return numbers 10-15. You can call ConnectLibrary.Dec(string s) to convert a number/letter from hex into decimal 0-15.
	{
		//when {going to lose the game}, don't;

		/* // Integer Math
		int x = 6;

		x = x + 3;
		x = x * 2;
		x = x - 2;
		x = x / 2; // returns a whole number!
		x = x % 3; // gives remainder of x / 3

		x += 3; // equivalent to x = x + 3;
		x *= 2;
		x -= 2;
		x /= 2;
		x %= 2;

		x++; // equivalent to x += 1;
		x--; // equvalt to x -= 1;
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

		if (board[board.Columns / 2, board.Rows - 1] == Team)
		{
			//Say($"Hey, I'm in column {x} at the bottom!");
			Console.ReadLine();
		}

		// random.Next(board.Columns)

		return board.Columns / 2; // Aaaand he just puts in a random move. Make sure your AI knows how many columns there are, it's not always 7!
	}

	public override void MatchEnd(State victor, int round) // This is called every time a round ends.
	{
		if (victor == State.Empty) Say("It looks like this is a draw but i'm cooler so we'll call it my win"); // draw dialogue
		else if (victor == Team) Say("Looks like I won, suck it! >:)"); // win dialogue
		else {
			Say("I lost... But I'll outside your house in 3 minutes, so get ready"); // loss dialogue
		}
	}

	public override void GameEnd() // This is called at the end of a series of games.
	{
		Say("It was fun beating you, dumbass");
	}
}
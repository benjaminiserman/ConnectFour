using System;

public class PsychAI : ProAI
{
	public override string Name => "PsychAI";

	public PsychAI()
	{
		Say("There are fourteen million, six hundred and five possible futures starting here, and you win in none of them.");
	}

	public override int Prompt(Board board, int round)
	{
		double[] values = new double[board.Columns];

		int[] iValues = GetValues(new FakeBoard(board));
		bool importantOnly = false;

		for (int i = 0; i < board.Columns; i++) 
		{
			values[i] = (double)iValues[i];

			if (values[i] >= 1000) importantOnly = true;
		}

		for (int x = 0; x < board.Columns; x++)
		{
			if (importantOnly && values[x] < 1000) continue;

			if (board.CheckVictor(x, ConnectLibrary.GetNextY(x, board), Team)) return x;

			FakeBoard fake = new FakeBoard(board);

			fake.InputMove(x, round);

			//ValueFuture(fake);
		}

		// log thinking
		/*for (int x = 0; x < board.Columns; x++) Console.WriteLine($"{x}: {values[x]}");
		Console.WriteLine($"Next move: {column}");
		Console.ReadLine();*/

		int column = ConnectLibrary.Max(values);

		if (column < 0 || column >= board.Columns) column = new Random().Next(board.Columns);

		return column;
	}

	protected bool IsCheckmate(FakeBoard board, State team)
	{
		int wins = 0;

		for (int x = 0; x < board.Columns; x++)
		{
			int y = board.GetNextY(x);

			if (board.CheckVictor(x, y, team)) wins++;
		}

		return false;
	}

	public override void MatchEnd(State victor, int round)
	{
		if (victor == State.Empty) Say("Threat thwarted.");
		else if (victor == Team) Say("Just as I predicted.");
		else {
			Say("Psh... You only won because Mercury is in retrograde...");
		}
	}

	public override void GameEnd() // This is called at the end of a series of games.
	{
		Say("It was fun playing with you, teehee!");
	}
}
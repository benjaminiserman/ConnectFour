using System;

public class AssistAI : ProAI
{
	public override string Name => "AssistAI";

	public AssistAI()
	{
		Say("I'll help you out here.");
	}

	public override int Prompt(Board board, int round)
	{
		Console.WriteLine($"Input move, {board.CurrentTeam} team.");

		int numberInput = -1;
		int lastInput = -2;

		bool confirmed = false;
		while (!confirmed)
		{		
			string input = "";
			numberInput = -1;

			while (!ConnectLibrary.TryDec(input, out numberInput) && input != "end") input = Console.ReadLine();

			if (input == "end") board.Forfeit();

			int[] values = GetValues(new FakeBoard(board));

			int max = ConnectLibrary.Max(values);

			if ((values[max] > 500 && max != numberInput) ||
				(values[numberInput] < 0 && values[max] > 0))
			{
				if (lastInput == numberInput) confirmed = true;
				else
				{
					Console.WriteLine($"Are you sure you want that? {max} may be a better option. Think carefully.");

					lastInput = numberInput;
				}
			}
			else confirmed = true;
		}

		return numberInput;
	}

	public override void MatchEnd(State victor, int round) // This is called every time a round ends.
	{
		if (victor == State.Empty) Say("Good stuff, human.");
		else if (victor == Team) Say("Good stuff, human.");
		else Say("We'll get them next time.");
		
	}

	public override void GameEnd() // This is called at the end of a series of games.
	{
		Say("I hope I was helpful.");
	}
}
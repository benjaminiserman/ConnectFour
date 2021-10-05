using System;

public class Human : AI
{
	public override string Name => "Human";

	public override int Prompt(Board board, int round)
	{
		string input = "";
		int numberInput = -1;

		Console.WriteLine($"Input move, {board.CurrentTeam} team.");

		for (int i = 0; i < 20; i++) Console.Write("\b"); // possible repl multiplayer input bug fix

		while (!ConnectLibrary.TryDec(input, out numberInput) && input != "end") input = Console.ReadLine();

		if (input == "end") board.Forfeit();

		return numberInput;
	}
}
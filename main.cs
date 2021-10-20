using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

public class MainClass
{
	private static bool running;
	private static bool auto = false;

	public static void Main(string[] args)
	{
		if (running) throw new Exception("Main() is already running.");
		running = true;

		while (true)
		{
			int columns, rows, connect, teamCount;
			bool noMiddleStart, load, ascii, colorBlind;
			string loadString = null;
		
			if (BoolQuery("Quickplay? (y/n)"))
			{
				columns = 7;
				rows = 6;
				connect = 4;
				teamCount = 2;
				noMiddleStart = false;
				load = false;
				ascii = false;
				colorBlind = false;
			}
			else
			{			
				columns = IntQuery("How many columns should the board have?");
				while (columns < 3 || columns > 16) columns = IntQuery("Columns must be between 3 and 16.");

				rows = IntQuery("How many rows should the board have?");
				while (rows < 3 || rows > 16) rows = IntQuery("Rows must be between 3 and 16.");	

				connect = IntQuery("How many dots should the player have to connect?");
				while (connect < 3 || (connect > columns && connect > rows)) connect = IntQuery("Connect must be greater than 2 and less than or equal to either columns or rows.");	

				teamCount = IntQuery("How many teams are playing?");
				while (teamCount < 2 || teamCount > 8) teamCount = IntQuery("Teams must be between 2 and 8.");

				if (columns % 2 != 0)
				{
					noMiddleStart = BoolQuery("Should the first player be forbidden from starting in the center? (y/n)");			
				}
				else noMiddleStart = false;

				colorBlind = BoolQuery("Enable colorblind mode? (y/n)");
				ascii = BoolQuery("Enable ASCII mode? (y/n)");

				load = BoolQuery("Load Game? (y/n)");
				if (load)
				{
					Console.WriteLine("Enter Load String. (Round History of desired game)");
					loadString = Console.ReadLine();
				}
			}

			var @assembly = typeof(AI).Assembly;
			var AIs = @assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AI)));

			Dictionary<State, AI> teams = new Dictionary<State, AI>();
			Dictionary<State, int> wins = new Dictionary<State, int>();
			int draws = 0;
			int totalGames = 0;

			Console.WriteLine("Available AIs:");

			foreach (Type ai in AIs)
			{
				Console.WriteLine(ai.Name);
			}

			string input = "";

			for (int i = 0; i < teamCount; i++)
			{
				Console.WriteLine($"Which AI should be Player {i + 1}?");
				Type foundAI = null;
				while (foundAI == null) 
				{
					input = Console.ReadLine().ToLower();
				
					foreach (Type AI in AIs)
					{
						if (AI.Name.ToLower() == input)
						{
							foundAI = AI;
							break;
						}
					}
				}

				AI createAI;
			
				createAI = Activator.CreateInstance(foundAI) as AI;		

				createAI.Team = (State)(i + 1);

				teams.Add((State)(i + 1), createAI);
				wins.Add((State)(i + 1), 0);
			}

			bool humanPresent = false;

			foreach(AI ai in teams.Values)
			{
				if (ai is Human)
				{
					humanPresent = true;
					break;
				}
			}

			if (!humanPresent)
			{
				if (BoolQuery("Should the game repeat without player input? (y/n)")) auto = true;
				else auto = false;
			}

			Console.WriteLine("Press enter to begin.");
			Console.ReadLine();

			State currentTeam = State.Red;

			while (true) 
			{				
				Board board = new Board(columns, rows, connect, teamCount, noMiddleStart, currentTeam, ascii, colorBlind);
				Game game = new Game(board, teams, board.Auth, load, loadString);

				if (load) load = false;
				
				game.RunGame();

				totalGames++;
				if (board.Victor != State.Empty)
				{
					wins[board.Victor]++;
				}
				else draws++;

				if (!auto)
				{
					input = null;
					while (input != "next" && input != "end" && input != string.Empty)
					{
						Console.WriteLine("Type \"next\" to continue or \"end\" to stop playing.");
						input = Console.ReadLine();
					} 
				
					if (input == "end") break;
				}
				else if (Console.KeyAvailable) break;

				currentTeam = Next(currentTeam, teamCount);
			}

			Console.WriteLine();

			foreach (AI ai in teams.Values)
			{
				Console.WriteLine($"Team {ai.Team}, under {ai.Name}, had {wins[ai.Team]} wins, for a win rate of {(wins[ai.Team] / (double)totalGames).ToString("P2")}");

				try
				{
					ai.GameEnd();
				}
				catch {}
			}

			Console.WriteLine($"{draws} games ended in a draw.");
		}

		running = false;
	}

	private static int IntQuery(string text)
	{
		string input = "";
		int intInput;
		Console.WriteLine(text);
		while (!Int32.TryParse(input, out intInput)) input = Console.ReadLine();
		return intInput;
	}

	private static bool BoolQuery(string text)
	{
		Console.WriteLine(text);
		string input = "";
		while (input != "y" && input != "n") input = Console.ReadLine().Trim().ToLower();
		if (input == "y") return true;
		else return false;
	}

	public static State Next(State state, int teamCount)
	{
		if (state == State.Empty) throw new Exception("Invalid Argument: state cannot be State.Empty!");

		int id = (int)state;
		id++;
		if (id > teamCount) id = 1;

		return (State)id;
	}
}

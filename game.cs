using System;
using System.Collections.Generic;

public class Game
{
	public int Round { get; private set; }
	public static bool GameInProgress { get; private set; }
	public State PromptingTeam { get; private set; }

	public Board Board { get; private set; }
	private Dictionary<State, AI> teams = new Dictionary<State, AI>();
	private int _auth;
	private bool _load;
	private string _loadString;
	private int _loadCounter;

	public Game(Board board, Dictionary<State, AI> teams, int auth, bool load = false, string loadString = null)
	{
		Board = board;
		this.teams = teams;
		_auth = auth;
		_load = load;
		_loadString = loadString;
	}

	public void RunGame()
	{
		if (GameInProgress) throw new Exception("A game is already in progress.");

		GameInProgress = true;

		Round = 0;

		State startingTeam = Board.CurrentTeam;

		while (Board.GameInProgress)
		{
			Board.Draw(Round);

			PromptingTeam = Board.CurrentTeam;
			if (PromptingTeam == startingTeam) Round++;

			int input = -1;
			if (_load && _loadCounter < _loadString.Length)
			{
				Console.WriteLine("Replaying... (press enter)");
				try
				{
					if (ConnectLibrary.TryDec(_loadString[_loadCounter].ToString(), out input)) Board.InputMove(input, _auth, Round);
					else throw new Exception();
				}
				catch
				{
					Console.WriteLine("Invalid Load String!");
					_loadCounter = _loadString.Length;
				}
				Console.ReadLine();
				_loadCounter++;
			}
			else do
			{
				try 
				{
					input = teams[Board.CurrentTeam].Prompt(Board, Round);
				}
				catch (Exception e)
				{ 
					Console.WriteLine($"{PromptingTeam} errored and has ended the game.");
					Console.WriteLine(e);
					Console.ReadLine();
					Board.Forfeit();
				}
			}
			while (!Board.InputMove(Convert.ToInt32(input), _auth, Round));	
		}

		Board.DisableCreation(_auth);

		foreach (AI ai in teams.Values)
		{
			try 
			{
				ai.MatchEnd(Board.Victor, Round);
			}
			catch 
			{ 
				Console.WriteLine($"{ai.Team} team, with AI {ai.Name} errored in it's MatchEnd() method."); 
			}
		}

		Board.EnableCreation(_auth);

		GameInProgress = false;
	}
}
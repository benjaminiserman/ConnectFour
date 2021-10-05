# ConnectFour
an ASCII ConnectFour game that runs in the console

Features:
 - Play with up to 8 players!
 - Board sizes range from standard (8x6) all the way up to 16x16
 - ASCII-friendly and colorblind modes
 - Optional rules are available to break the solved ConnectFour meta
 - AIs you can face off against (play against mine, ProAI for a challenge)
 - Add your own AIs and compete with others!
 
Want to make your own AI?
  Finally a challenger!
  To make an AI, I'd recommend copying and modifying the file randomAI.cs
  RandomAI is an example AI that just plays random moves every round
  its purpose is to show off the basics of implementing a ConnectFour AI. 
  
  for more tips and helper methods, check out library.cs
  
  the code uses reflection to create the AI pool.
  Any class deriving from the abstract class AI will be automatically added for your convenience.

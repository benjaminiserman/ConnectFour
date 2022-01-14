# ConnectFour
![Downloads](https://img.shields.io/github/downloads/winggar/ConnectFour/total?style=for-the-badge)

![Imgur](https://i.imgur.com/LsT3LXs.png)

ConnectFour is a console app that allows users to play custom games of ASCII ConnectFour against other players (locally) or AIs.

## Features
 - Play with up to 8 players!
 - Board sizes range from standard (8x6) all the way up to 16x16
 - ASCII-friendly and colorblind modes
 - Optional rules are available to break the solved ConnectFour meta
 - AIs you can face off against (play against mine, ProAI for a challenge)
 - Add your own AIs and compete with others!

## Prerequisites

Before you begin, ensure you have met the following requirements:
- [You use a machine supported by .NET 6](https://github.com/dotnet/core/blob/main/release-notes/6.0/supported-os.md)
- You have .NET 6 installed
- You have downloaded the file "build.zip" from the latest release

OR

- [You use a **Windows** machine supported by .NET 6](https://github.com/dotnet/core/blob/main/release-notes/6.0/supported-os.md)
- You **do not** need to have .NET 6 installed
- You have downloaded the file "standalone.zip" from the latest release

OR

- You have a modern, up-to-date web browser

Due to low demand, standalone builds for Mac OSX and Linux are not provided. If you'd like a standalone build for Mac OSX or Linux, [contact me](mailto:winggar1228@gmail.com).

## Usage

1. Download either "build.zip" or "standalone.zip" from the latest release, depending on your prerequisites.
2. Unzip the file.
3. Find the file "Template.exe" within the unzipped folder and run it.
4. Follow the on-screen instructions and enjoy!

OR

Don't want to download anything?
Go to [my replit](https://replit.com/@winggar/ConnectFour) and press Start at the top.

Note: The version hosted on Replit is not guaranteed to be up-to-date.

## Contribution
 
Want to make your own AI?

Finally a challenger!

### Forking

To contribute to ConnectFour, follow these steps:

1. Fork this repository.
2. Create a branch: `git checkout -b <branch_name>`.
3. Make your changes and commit them: `git commit -m '<commit_message>'`
4. Push to the original branch: `git push origin <project_name>/<location>`
5. Create the pull request.

Alternatively see the GitHub documentation on [creating a pull request](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

### Adding a new AI

To make an AI, I'd recommend copying and modifying the file "randomAI.cs". RandomAI is an example AI that just plays random moves every round. Its purpose is to show off the basics of implementing a ConnectFour AI. 
  
For more tips and helper methods, check out the file "library.cs".
  
My code uses reflection to create the AI pool. Any class deriving from the abstract class **AI** will be automatically added to the available AIs. However, to add AIs to the base game, you'll have to submit a pull request as explained above.

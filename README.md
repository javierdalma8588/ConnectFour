# Connect Four
This repository is about a project that replicates the classic game Connect 4. The game offers 2 modes Player VS Player and Player VS AI.

The project was built using Unity 2020.3.11f1 using C#.

You can test the game live at the following link:
https://play.unity.com/mg/other/connect-4-mini-game 

# Protect Architecture
This project is constituted by 3 main scripts that I will explain.

# GameManager.cs

The first Script is the GameManager.cs where all the variables regarding the game are handled. Some of this variable are:
![image](https://user-images.githubusercontent.com/59519933/123492190-735f1800-d5de-11eb-9d9c-3cfcb26b353f.png)

 As some other scripts need some information that this script has I made the GameManager.cs a Singleton.
 
 To begin this script sets the size of the board on a two dimensional array using the standard boar measures that are 7X6. Then we have a function regarding a small visual aid to the user that will draw the player piece on top of the column the player is currently selecting this is done by the OnMouseOver function. 
 
 Then we have the TakeTurn coroutine that takes as a parameter the column number. This coroutine handles all the events of a player or an AI drops a piece in the case of the player vs player we have that that after the player, click on a spawn point using the OnMouseDown function we will instantiate a piece on the column he select. The pieces go down using the rigid body component and are stopped by a collider or by another piece after that it is the next persons turn. The game keeps like that until the win conditions or draw conditions are met. In the case of the player vs AI after player one chooses a row to spawn the AI selects a random column to put his piece after that it is the users turn again.

This code includes a bool function that Updates the board every time the users or AI place a piece. When the game starts the board is filled with zeros.
 
![image](https://user-images.githubusercontent.com/59519933/123491533-85d85200-d5dc-11eb-9633-954984676c58.png)

When the player one chooses a row and spawns a piece the zero will turn to 1.

![image](https://user-images.githubusercontent.com/59519933/123491562-91c41400-d5dc-11eb-95c7-66afd5fc247c.png)

And for the player to the zero will change for a number 2.

![image](https://user-images.githubusercontent.com/59519933/123491572-98eb2200-d5dc-11eb-95d4-e0d23c23da9a.png)

Now for the winning conditions I checked all the possible ways that a user could connect 4 pieces and check all of the in this case these are horizontal, vertical and diagonals.And for the draw function I just checked if on the top row we still have places on the board where the number is not zero if that is the case we can still play otherwise the game ends on a tie. However, before checking the tie I have to make sure that the last piece that a user put didnt trigger a win condition,

And finally, just for debugging I created a DebugBoard function that will print the board in a 2D matrix in the console in that way it was easier for me to fix any issue I had while I was working on the project.

# UIManager.cs

This script is the one in charge of changing the scenes on the project and enabling UI objects like the wining screen and changing the text of the winner. As the game manager needed to use the information on the UIManager.cs this is also a Singleton.

This script handles the variables:
-WinningScreen
-WinningText

The winscreen is enabled by a function on this script but is called inside the GameManager.cs when a winning condition is met. Also the winning text is called and change dinamicaly by the GameManager.cs to change between player1 and player2 win..

And finally this script handles the scene changing as this game has 3 scenes:
-Main Menu
-Player VS Player game
-Player VS AI game

# InputFields.cs

This script is just in charge of handling on what column the user currently has his mouse (this is used to draw the UI feedback that I comment it on the GameManager.cs scrip) and to handle when the user clicks on top of a column we know it is the number of the column and spawns a piece. 

# What Could be improved Next:

-As we have a variable for the board size height and lenght we could build a modular system to create bigger or smaller boards
-Right now the code instantiates a new piece every time an improvement could be to use a pool of objects to re use some of them pieces of previous games if we do that we could save a lot of memory
-For the winning conditions I think the function could be written in a more elegant solution
-Right now we have 3 scenes that could be reduce to just one by handling the objects from each scene and putting them in 1 GameObject for each game mode

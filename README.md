# GamesCrane

## Introduction
GamesCrane is a C# based WinUI3 application that allows you to collect and store all your games in one place! Modeled after a crane vending machine, you're able to add your game shortcuts to the machine; even allowing for custom images. Once you add a game successfully to the machine, you're able to launch it--along with remove or switch its spot with another game. You no longer have to comb through a cluttered desktop screen to see what game you're interested in, as GamesCrane cleanly displays it for you (and you can organize how you like it)! With this convenient app to hold all your game launching in one place, your desktop will be cleaner than ever before.  
*Note: does not currently work with steam/origin/other games that have a separate launcher; future updates will address this issue.*  


## How to Use
To add, remove, or switch games, press the **Edit Games** button.  
To add a game, find the executable path. If your game is on desktop, right click it -> select properties -> copy the text under the title *Target* -> paste it into the path box. If your game needs admin, or you notice that the path has flags at the end (indicated by a -sometexthere or a --someothertexthere after the .exe), check the associated boxes.  
To remove a game, double click the desired game you want to remove. To switch games, click the first game, then double click the second game. Press **ESCAPE** to exit these special remove/switch states.  
To play a game, in a non remove/switch state, either double click on the desired game or click the game and press the play button. Note: the application does not automatically close on game launch, please make sure to close the app if you don't want it running!  

## Directory Layout
| Project Directories     | Brief Description          |
|-------------------------|----------------------------|
| [`/GamesCrane/GamesCrane/Assets`](./GamesCrane/GamesCrane/Assets) | Backgrounds and other images. |
| [`/GamesCrane/GamesCrane/Model`](./GamesCrane/GamesCrane/Model) | Files related to custom classes. |
| [`/GamesCrane/GamesCrane/Services`](./GamesCrane/GamesCrane/Services) | Non main-app related services; services that fall outside of UI modifications. |
| [`/GamesCrane/GamesCrane/View`](./GamesCrane/GamesCrane/View) | Files that contain UI elements. |
| [`/GamesCrane/GamesCrane/ViewModel`](./GamesCrane/GamesCrane/ViewModel) | Files to store information needed by various app pages. |

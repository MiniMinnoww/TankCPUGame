# Tank CPU Game
## Overview: An educational game

This game is designed mainly for "Week 9 - Enemy/AI" session with The University of Sheffield GameDev Society.

The general idea is that each group/person can write their own class that inherits from "CPUBrain".
They will then be able to implement methods and use provided helper methods (from CPUBrain).

In game, players can select which CPUs to spawn into the match, and can optionally add a "Player" controlled player for testing too.

Game Design Doc: [Google Doc Link](https://docs.google.com/document/d/1xcoRbonklk_kXdfvQmfVb2SIITTVZ0eMcYWmJpRE3aI/edit?usp=sharing)

## How to make a CPU
The game comes with a class called "CustomCPUBrain" which has some minor base code inside of it. To make a CPU, just edit this file.
Functionality for your CPU is stored in the base class "CPUBrain" that this inherits from. Here is a comprehensive list of methods you can call from your CPU
### Methods:


## Current Roadmap
- Finish the base game implementation
- Create the CPUBrain and it's helper methods, preventing 'cheating' by only exposing certain methods & properties
- Look into automatically generating the "Players" dropdown using an editor script?
- Polish the game
- Add a editor "Create > Scripting > Custom CPU" which auto adds the new CPU script to the SO list
- Potentially add an in-game debug mode

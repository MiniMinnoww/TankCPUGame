# Tank CPU Game
## Overview: An educational game

This game is designed mainly for "Week 9 - Enemy/AI" session with The University of Sheffield GameDev Society.

The general idea is that each group/person can write their own class that inherits from "CPUBrain".
They will then be able to implement methods and use provided helper methods (from CPUBrain).

In game, players can select which CPUs to spawn into the match, and can optionally add a "Player" controlled player for testing too.

Game Design Doc: [Google Doc Link](https://docs.google.com/document/d/1xcoRbonklk_kXdfvQmfVb2SIITTVZ0eMcYWmJpRE3aI/edit?usp=sharing)

## How to make a CPU
The game comes with a class called `CustomiseCPUBrain` which has some minor base code inside of it. To make a CPU, just edit this file.
If you need help / get stuck then there is an `ExampleCPUBrain` class that I built which uses these methods to run.

Functionality for your CPU is stored in the base class `CPUBrain` that this inherits from. Here is a comprehensive list of methods you can call from your CPU

### Methods

#### Movement
`bool IsObstacleAhead(float range = 10)`<br>
`IDetectableObstacle GetObstacleAhead(float range = 10)`<br>
`float GetRotationToLookAt(IDetectableObject obj)`<br>

`bool IsMoving()`<br>
`bool IsRotating()`<br>
`StopAllMovement()`<br>

`SetDriveInput(float value)`<br>
`SetRotationInput(float value)`<br>
`StopDriving()`<br>
`StopRotating()`<br>

`MoveForSeconds(float speed, float seconds)`<br>
`RotateForSeconds(float speed, float seconds)`<br>

`float GetRotation()`<br>

#### <ins>Direction Utilities</ins>  
`Vector2 GetDirectionTo(Vector2 targetPosition)`<br>
`Vector2 GetDirectionTo(IDetectableObject target)`<br>
`Vector2 GetDirectionToNearestTank()`<br>
`Vector2 GetDirectionToNearestProjectile()`<br>

#### <ins>Distance Utilities</ins>
`Vector2 GetDistanceTo(Vector2 targetPosition)`<br>
`Vector2 GetDistanceTo(IDetectableObject target)`<br>
`Vector2 GetDistanceToNearestTank()`<br>
`Vector2 GetDistanceToNearestProjectile()`<br>

#### <ins>Detection</ins>

`List<IDetectableObject> GetObjectsInViewCone()`<br>
`List<IDetectableTank> GetTanksInViewCone()`<br>
`List<IDetectableProjectile> GetProjectilesInViewCone()`<br>

`IDetectableTank GetClosestTank()`<br>
`IDetectableProjectile GetClosestProjectile()`<br>
`bool IsEnemyProjectileInView()`<br>
`bool IsTankShootableAhead(float range = 10)`<br>

#### <ins>Utilities</ins>
`bool HasLineOfSightTo(IDetectableObject target)`<br>
`bool IsObjectWithinRange(IDetectableObject target, float range)`<br>
`bool IsTankVisible()`<br>
`bool IsProjectileVisible()`<br>
`bool IsAnyObjectVisible()`<br>
`float GetHealth()`<br>
`Shoot()`<br>



### Also...
You have access to methods from the `Brain` class, most notably being able to override the `Update()` method which runs every frame, using
`public override void Update()` in your cpu class.

If you have issues make sure to check out the `ExampleCPUBrain` class.

## Current Roadmap
- Smooth movement
- Make UI stay upright and above the player
- Polish the game
  - Add recoil to shooting
  - Add SFX

## Description

This program implements a PID (Proportional-Integral-Derivative) controller within a 3D rocket simulation environment. Under specific restrictions and assumptions, the controller is designed to manage the rocket's stability, rotation and movement with respect to a target location. The simulation operates within the physics engine of the Unity game engine.

## Preview

### Heat Equation
<img src="assets/preview-1.gif" alt="Alt Text" width="600" height="350" />

Descriptiion

## Assumptions

To simplify some of the complexities of the rocket's movement, we have certain restrictions and assumptions:
* The rocket does not have built-in sensors monitoring its orientation; instead, it has access to the position and rotation data provided by the Unity engine.
* The rotation around the $y$-axis is locked. This avoids the complexities of rotating around the $x$- or $z$-axis when the $y$ rotation is non-zero.
* Instead of using thrusters for rotation, the rocket directly modifies the rotation parameter of the game object in Unity. As a result, rotation is not handled by the physics engine.
* The rocket's rotations around the x- and z-axes are limited to some extent prevent the rocket from flipping upside down.

## Caution

Please note that the code in this project is poorly written and lacks sufficient comments. It has not been rewritten or refactored to adhere to standard coding practices. Therefore, we recommend using the concepts and ideas presented rather than relying on the code itself.

## Implementations

### PID Controllers

In total, there are three different PID controllers for the rocket: one for the altitude, one for the x rotation and one for the z rotation. Each of the controllers rely on the PID pseudocode given by:

$\textbf{START}$

  // Calculate error
  
  error = target - current
  
  // Proportional term
  
  proportional = kp * error
  
  // Integral term
  
  integral += error * deltaTime
  integralTerm = ki * integral
  
  // Derivative term
  
  derivative = (error - previousError) / deltaTime
  derivativeTerm = kd * derivative
  
  // Update previous error
  
  previousError = error
  
  // Calculate PID output
  
  output = proportional + integralTerm + derivativeTerm

$\textbf{END}$

Depending on the controller, the values of 'current' and 'target' are the altitudes of the rocket and target position respectively or the distance in x or z direction. The PID controllers will then give an output trying to reach the target as fast and efficient as possible. By efficient we mean without and extensive amount of osciliating.

## Dependencies

Uses the Unity Engine (version 2022.3.29f1) along with the following Unity store assets:
* Free 3D Missile by LunarCats Studio.
* Simple Urban Buildings Pack 1 by Rune Studios.
* Wispy Skybox by Mundus.

## Author
Marius H. Naasen, originally created summer of 2017.

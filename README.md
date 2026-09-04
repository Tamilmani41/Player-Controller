[README.md](https://github.com/user-attachments/files/31844391/README.md)
# Modular 2D Player Controller

A modular, physics-based 2D player controller for Unity.

The controller separates gameplay logic from Unity-specific physics
operations. Gameplay modules are implemented as plain C# classes, while
the `RigidbodyAdapter` handles the engine-facing physics API.

## Architecture

![Player Controller Architecture](Documentation/architecture.png)

The system is split into two main parts:

-   **Gameplay layer** --- `PlayerController` and its modules.
-   **Engine layer** --- `RigidbodyAdapter`, which communicates with
    Unity's `Rigidbody2D` and physics API.

The goal is to keep gameplay modules independent from direct Unity
physics API calls. If the Unity API changes, the adapter can be modified
or replaced without requiring the gameplay modules to be rewritten.

------------------------------------------------------------------------

## Core Flow

`PlayerController` is the main orchestrator.

During `Update()`, it:

1.  Reads player input.
2.  Ticks the required modules in their defined execution order.
3.  Reads the results produced by each module.
4.  Stores those results as controller state.

The `RigidbodyAdapter` runs during `FixedUpdate()` and consumes the
stored state from the controller.

This creates an explicit boundary between the frame update and the
physics update.

``` text
Player Input
     |
     v
PlayerController.Update()
     |
     +--> Movement
     |
     +--> Jump
     |
     +--> Ground Detector
     |
     +--> External Forces
     |
     v
Stored Controller State / Values
     |
     |  Update -> FixedUpdate
     v
RigidbodyAdapter.FixedUpdate()
     |
     v
Rigidbody2D / Physics API
```

The modules currently produce their results independently. The
`RigidbodyAdapter` applies those results individually.

Velocity composition is a possible future architectural extension.

------------------------------------------------------------------------

## Modules

All gameplay modules are implemented as plain C# classes rather than
`MonoBehaviour`s.

Each module is responsible for its own calculation and produces a result
for the `PlayerController` to store.

### Movement

Handles horizontal player movement.

The module currently calculates the player's final horizontal velocity.

Future movement-related extensions may include air resistance and other
movement behaviour.

### Jump

Handles the player's jump logic and produces the jump velocity.

Current features:

-   Grounded jump
-   Coyote time
-   Jump buffering
-   Jump request handling

The current implementation uses the configured jump force directly as
the jump velocity.

Variable jump height is a possible future extension.

### Ground Detector

Determines whether the player is grounded.

The ground detector does not directly access Unity's physics API.
Instead, it requests the required physics operation through
`PhysicsQueries`.

``` text
GroundDetector
      |
      v
PhysicsQueries
      |
      v
RigidbodyAdapter
      |
      v
Unity Physics / Raycast
```

The result is returned through the same abstraction:

``` text
Raycast Result
      |
      v
RigidbodyAdapter
      |
      v
PhysicsQueries
      |
      v
GroundDetector
      |
      v
PlayerController
```

This keeps the ground detection logic separate from the engine-specific
physics implementation.

### External Forces

`ExternalForces` is an orchestrator for forces originating outside the
normal movement and jump systems.

Its intended structure is:

``` text
ExternalForces
      |
      +--> Moving Platform
      +--> Knockback
      +--> Environmental Forces
      |
      v
Final External Force
```

For v0.1, only the **Moving Platform** module is implemented.

Knockback, environmental forces, and other external-force sources are
planned as future extensions.

------------------------------------------------------------------------

## Rigidbody Adapter

`RigidbodyAdapter` is the Unity-facing part of the controller.

It currently handles:

-   Applying movement velocity.
-   Applying jump velocity.
-   Applying external-force results.
-   Performing physics operations such as raycasts.

The adapter reads the state produced by `PlayerController` during
`FixedUpdate()` rather than having individual gameplay modules access
the `Rigidbody2D` directly.

This keeps Unity-specific physics API usage isolated to the adapter.

If the Unity physics API changes in the future, the adapter can be
modified or replaced while keeping the gameplay modules largely
unchanged.

------------------------------------------------------------------------

## Player Settings

`PlayerSettings` contains configuration data used by the player modules.

The required settings are passed to the modules during construction.

This keeps module dependencies explicit and prevents the modules from
having to retrieve their configuration themselves.

Examples of configurable values include:

-   Movement speed
-   Jump force
-   Coyote time
-   Jump buffer duration

------------------------------------------------------------------------

## Update / FixedUpdate Synchronization

The controller and adapter intentionally run on different Unity ticks:

``` text
PlayerController
      |
   Update()
      |
      v
Calculate and store state
      |
      | State persists
      v
RigidbodyAdapter
      |
 FixedUpdate()
      |
      v
Consume state
      |
      v
Rigidbody2D
```

This state persistence is important because `Update()` and
`FixedUpdate()` do not necessarily run at the same frequency.

For example, when a jump request is accepted during `Update()`, the
controller stores the request until the physics step consumes it.

``` text
Input
  |
  v
PlayerController.Update()
  |
  v
JumpModule
  |
  v
Jump request stored
  |
  | wait for physics tick
  v
RigidbodyAdapter.FixedUpdate()
  |
  v
Apply jump velocity
  |
  v
Reset consumed jump state
```

This prevents transient requests from being lost between the frame and
physics ticks.

------------------------------------------------------------------------

## Player Controller Construction

The player modules are constructed during the player's initialization
phase.

Configuration data such as `PlayerSettings` is passed into the modules
during construction.

The resulting structure keeps the modules as normal C# objects while the
`PlayerController` manages their lifetime and execution order.

------------------------------------------------------------------------

## Current Status --- v0.1

### Implemented

-   [x] Modular player controller
-   [x] Horizontal movement
-   [x] Ground detection
-   [x] Physics query abstraction
-   [x] Rigidbody adapter
-   [x] Jump system
-   [x] Coyote time
-   [x] Jump buffering
-   [x] Moving platform external force
-   [x] Update / FixedUpdate state handling

### Planned / Future Extensions

-   [ ] Air resistance
-   [ ] Variable jump height
-   [ ] Knockback
-   [ ] Environmental forces
-   [ ] Additional external-force modules
-   [ ] Velocity / force composition

The exact scope of future extensions may change as the controller
develops.

------------------------------------------------------------------------

## Design Goals

The architecture is primarily designed around separation of
responsibilities and isolation of engine-specific code.

Gameplay modules should focus on calculating gameplay behaviour rather
than directly interacting with Unity's physics API.

The intended dependency direction is:

``` text
Gameplay Modules
       |
       v
PlayerController
       |
       v
Physics / Engine Abstraction
       |
       v
RigidbodyAdapter
       |
       v
Unity Physics
```

This allows the engine-facing implementation to change without requiring
the gameplay modules to depend directly on those changes.

The architecture also provides a structure for adding new gameplay
modules without turning the player controller into one large class.

------------------------------------------------------------------------

## Project Structure

``` text
Assets/
└── Player/
    ├── Config/
    │   └── PlayerSettings.cs
    │
    ├── Core/
    │   ├── PhysicsQueries.cs
    │   ├── PlayerController.cs
    │   ├── PlayerInput.cs
    │   └── RigidbodyAdapter.cs
    │
    ├── Debug/
    │   └── PlayerDebug.cs
    │
    └── Modules/
        ├── Movement/
        ├── Jump/
        ├── GroundDetection/
        └── ExternalForces/
```

------------------------------------------------------------------------

## Tech Stack

-   Unity
-   C#
-   Unity 2D Physics
-   `Rigidbody2D`
-   Unity Input System

## Status

**In development --- v0.1**

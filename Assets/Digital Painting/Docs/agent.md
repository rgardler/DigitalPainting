# Agents

Agents are the "characters" in the scene. Unlike `Things` they are able to move and perform actions in order to interact with the environment.

## BaseAgentController

The `BaseAgentController` provides the most basic of functionality for an Agent. There are other controllers that extend the `BaseAgentController`.

### AIAgentController

The `AIAgentController` is an agent controller that can be assigned AI behaviours in order to have it act autonomously in the scene.

## Creating an Agent

The minimum required to create an agent are:

  1. Create a Game Object that represents your agent in the world.
  2. Add a controller script, such as `BaseAgentController`
  3. Add a rigid body with "is kinematic" set to true
  4. Add an `AbilityCollection` component.
  5. Add an `InventoryManager` component

### Abilities

The `AbilityCollection` contains all the abilities that the agent currently has available to them. 

### Inventory

This is the inventory used by the agent to carry things around. It is usually pretty small.

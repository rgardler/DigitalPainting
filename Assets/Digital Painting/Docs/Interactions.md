# Interactions

When an agent encounters some kinds of Things they mat want to interact with that thing. For example, they may want to open it, or pick it up. To do this we need to add Interactions to the system. This document describes how to make Things interactable.

# `BaseInteraction` Class

The `BaseInteraction` class is used to describe a type of interaction. It implements the effects of the interactions, but the `BaseInteraction` is very limited, in almost every case you will want to extend this class to create a richer interaction. 

For example, the `PickUp` interaction allows agents to pickup the object and carry it away. To implement this create a `Pickup` class which extends the `BaseInteraction` and override the `Interaction` method. You can see this as an example interaction in the asset.

# `InteractionManager` Class

The `InteractionManager` class is used to manage the interactions that an agent can perform. Drag this class onto the `Agent` game object. This class will coordinate interactions with `Things` in the game. In order to do so it must know what interactions the agent is able to perform. To do this we need to drag interactions onto the `Agent`. When the Interaction component initializes it will register itself with the `InteractionManager` component.

Having the interaction manager and all associated interactions on the game object itself can be quite cumbersome, therefore it is possible to create a child object of the `Agent` and add these components, including the collider, to that instead.

# Interactable Objects

In order to trigger the `InteractionManager` objects must have a rigid body
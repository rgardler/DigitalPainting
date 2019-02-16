# Interactions

When an agent encounters some kinds of Things they may want to interact with that thing. For example, they may want to open it, or pick it up. To do this we need to add Interactions to the system. Interactions are managed through a combination of the `Interactable` component and the Timeline. This document describes how to make Things interactable.

# `Interactable` Component

The `Interactable` class is added to a GameObject that has the `Thing` component, it indicates the `Thing` can be interacted with.

Through this component you can set the location characters should stand in order to interact with the `Thing`. At the time of writing this is a point, but it really ought to be an area to accommodate different kinds of agents (patches welcome against [Issue 44](https://github.com/DigitalPainting/DigitalPainting/issues/44)).

# Interactions

Interactions are define in a timeline. This means that an `Interactable` must have a `PlayableDirector` component attached. A good example that illustrates the core concepts of building an interaction timeline can be seen in `Pick Up Item Timeline`.

At the time of writing only a single interaction can be attached to the interactable. In a future version we will be able to offer different interactions.

## Creating an Interactable Thing

In order to be interactable you need a GameObject in the scene. This should have the `Thing` and `Interactable` components added. The `Thing` component provides good defaults, but you will need to configure the `Interactable` component as follows:

`Interaction Location` is the position from which an agent can interact with the item. See Issue 44 for a description of how this will change in the future (or has changed if the issue is closed - in which case please help us by updating this section of the documentation).

`Sprite` is an optional field (at time of writing) that provides an icon that will be used when displaying this thing.

`Default Playable Asset` is the timeline that will be played whenever an agent interacts with this `Thing`. In the next section we'll look at how to create this timeline.

## Creating an Interaction Timeline

In this section we will take a look at how to create a new Interaction timeline. In this example we will create a timeline to attach to a storage box that allows us to put an item into the box.

Create a new timeline object by right clicking on the `Timeline` folder and selecting `Create -> Timeline`. Give the timeline a name, such as "Place Item in Storage Timeline".

During development it's a good idea to have a small scene that allows you to test the timeline. There is a scene in which you can use for this in `Scenes/DevTest/Interacting`, so open that scene. Add your storage container object into the scene.

On the `Interaction` component set "isTrigger" to true. This will cause the Interaction to fire whenever an agent enters the trigger area.

### Output a debug message

The Digital Painting asset comes with some PlayableAssets that you can use in your timelines. One of them, the `NarrationControlAsset` can be used to manage narration within your scene. By default this will output all narration events to the log. We'll use this asset to ensure our interaction is being triggered.

In the Timeline editor click "Add" and select `wizardscode.interaction` -> `Narration Control Track`. Now add a `NarrationControlAsset` to this track and set the message in the inspector. Note that color will not affect the log output.

If you play the scene now you can walk into the trigger zone of the storage container object and your narration message will appear in the debug log.

At this point we've shown ourselves that the interaction is being triggered, but it doesn't really do anything useful. Lets fix that. By adding an ability to put something into the storage container. But first we will need something to put into it.

There is already a coin in this scene that has a pick-up interaction. Walking to within a unit will result in the agent casting a levitate spell to bring the coin to the agent, it is then put into the agents inventory. Once we have the coin we want to be able to walk up to the storage chest and use the same levitate spell to put it into the chest.

Coins that are in the agents inventory have been disabled, we therefore need to enable it. In the Timeline editor for the `Place Item In Storage Timeline` click "Add" and select `wizardscode.interaction` -> `InteractableControlAsset` at the start of the track and ensure the `activeState` property is true. This only needs to run once so it can be very short, even as short as a single frame (though it will barely be visible if it is that short).

In the Timeline editor for the `Place Item In Storage Timeline` click "Add" and select `wizardscode.interaction` -> `Digital Painting Manager Control Track`. Now add a `Drop Item Control Asset` to this track. In the inspector set the `Ability` to `Weak Levitate Item Spell`. This will move the item to the desired location, but it won't put it into a Storage Inventory Slot. To do that we need one more behaviour in our timeline.

Add an `InventoryAddItemControlAsset`



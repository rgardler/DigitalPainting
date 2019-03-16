# Interactions

When an agent encounters some kinds of Things they may want to interact with that thing. For example, they may want to open it or pick it up. To do this we need to add Interactions to the Thing. Interactions are managed through a combination of the `Interactable` component and the Timeline. This document describes how to make Things interactable.

# `Interactable` Component

The `Interactable` component is added to a GameObject that has the `Thing` component, it indicates the `Thing` can be interacted with.

Through the Interactable component you can set the location characters should stand in order to interact with the `Thing`. At the time of writing this is a point, but it really ought to be an area to accommodate different kinds of agents (patches welcome against [Issue 44](https://github.com/DigitalPainting/DigitalPainting/issues/44)).

# Interactions

Interactions are define in a timeline. This means that an `Interactable` must have a `PlayableDirector` component attached. A good example that illustrates the core concepts of building an interaction timeline can be seen in `Pick Up Item Timeline`.

At the time of writing only a single interaction can be attached to the interactable. In a future version we will be able to offer different interactions.

## Creating an Interactable Thing

In order to be interactable you need a GameObject in the scene. This should have the `Thing` and `Interactable` components added. The `Thing` component provides good defaults, but you will need to configure the `Interactable` component as follows:

`Interaction Location` is the position from which an agent can interact with the item. See Issue 44 for a description of how this will change in the future (or has changed if the issue is closed - in which case please help us by updating this section of the documentation).

`Sprite` is an optional field (at time of writing) that provides an icon that will be used when displaying this thing in the UI.

Each interactable can have one or more interactions attached to it. Each interaction can have one more conditions that must be satisfied for the interaction to be an available option. 

### Defining Interaction Collections

To create a new interaction set select the `Resources/Interactions` folder, right click and select `Create -> Wizards Code -> Interactions -> Collection`. Give your new collection a name. In the inspector type a name for your first interaction in this set and click the '+' sign to add it.  By default there are no conditions assigned to an interaction, but you can assign one by adding a condition collection to the `Required Conditions` field. The next section describes how to create a new condition collection.

`Description` a short description of the interaction. 

`Playable Asset` is the timeline that will be played whenever an agent interacts with this `Thing` using this interaction. In the next section we'll look at how to create this timeline.

### Defining Condition Collections

To create a new condition set select the `Resources/Interactions/Conditions` folder, right click and select `Create -> Wizards Code -> Interactions -> Conditions -> Collection`. Select the type of condition you want to use, give your a name and click the '+' sign to add it.  

#### Create New Condition Types

If the conditional test you need is not already available you will need to create one. Here we will look at how we can create a condition that will return true if the agent interacting with the object has a specific ability, in this case the ability to cast the `LevitateItemSpell`.

In `Scripts/Interaction/Condition` create a new script and call it `AbilityCondition` - this is a type of condition that will test whether an agent has a specific ability or not. Open the file for editing, make it derive from `Condition` and remove the boilerplate MonoBehaviour methods.

Now we need to override the `Satisfied` method and write code to test whether the condition is satisfied for a given paid of interactor and interactable. For example:

```
public override bool Satisfied(BaseAgentController interactor, Interactable interactable)
{
    return interactor.HasAbility(ability);
}
```

Having created a condition we need to ensure that it appears in the UI when we want to add it to a collection. At the time of writing this needs to be hard coded in the `ConditionCollectionEditor`. First add a suitable condition type name to `typeNames` in the `OnEnable` method. Then add an case to the switch statement in `AddCondition` method.

To make the new condition editable you will need to create an Editor for your condition in `ConditionEditor`

## Creating an Interaction Timeline

In this section we will take a look at how to create a new Interaction timeline. In this example we will create a timeline to attach to a storage box that allows us to put an item into the box.

Create a new timeline object by right clicking on the `Timeline` folder and selecting `Create -> Timeline`. Give the timeline a name, such as "Place Item in Storage Timeline".

During development it's a good idea to have a small scene that allows you to test the timeline. There is a scene in which you can use for this in `Scenes/DevTest/Interacting`, so open that scene. Add your storage container object into the scene.

### Output a debug message

The Digital Painting asset comes with some PlayableAssets that you can use in your timelines. One of them, the `NarrationControlAsset` can be used to manage narration within your scene. By default this will output all narration events to the log. We'll use this asset to ensure our interaction is being triggered.

In the Timeline editor click "Add" and select `wizardscode.interaction` -> `Narration Control Track`. Now add a `NarrationControlAsset` to this track (using the drop down menu on the narration track) and set the message in the inspector. Note that color will not affect the log output.

If you play the scene now you can walk into the trigger zone of the storage container object and your narration message will appear in the debug log.

At this point we've shown ourselves that the interaction is being triggered, but it doesn't really do anything useful. Lets fix that. By adding an ability to put something into the storage container. But first we will need something to put into it.

There is already a coin in this scene that has a pick-up interaction. Walking to within a unit will result in the agent casting a levitate spell to bring the coin to the agent, it is then put into the agents inventory. Once we have the coin we want to be able to walk up to the storage chest and use the same levitate spell to put it into the chest. For this to work we first need to equip the coin.

In the Timeline editor for the `Place Item In Storage Timeline` click "Add" and select `wizardscode.interaction` -> `Digital Painting Manager Control Track`. Now add a `Drop Item Control Asset` to this track. In the inspector set the `Ability` to `Weak Levitate Item Spell`. When triggered this interaction will drop the item, using the Levitate spell, into the storage container, adding it to the inventory.

## Creating New PlayableAssets for use in Interaction Timelines

TODO: Rewrite for the new Base Class Structure

You may need to create new playable assets for use in your timelines. Lets take a look at how to do that. In the previous section we used existing playable assets to create a timeline for storing items in a storage container. Lets suppose we want to automatically take an item out of the players inventory to add to storage. To do this we will need a new playable asset.

In the best location for the scripts, in this case `scripts/interaction/Timeline/DigitalPainting/Agent` right click and select `create -> playables -> Playable Behaviour c# script` and give it a name, here `SelectFromInventoryControlAsset`. Open this file for editing and add the following code:

```
private bool isFirstFrame = true;
    private DigitalPaintingManager manager;
    private BaseAgentController agent;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (isFirstFrame)
        {
            manager = playerData as DigitalPaintingManager;
            agent = manager.AgentWithFocus;
            isFirstFrame = false;
        }

        if (agent.Using = null) {
            agent.Using = agent.Inventory.GetItem(0);
        }
}
```

In the same directory right click and select `create -> playables -> playable asset c# script` and give it a name, here `SelectFromInventoryControlAsset`. Open this file for editing and add the following `CreatePlayable` method implementation.

```
public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
{
    var playable = ScriptPlayable<SelectFromInventoryControlBehaviour>.Create(graph);

    SelectFromInventoryControlBehaviour behaviour = playable.GetBehaviour();
    return playable;
}
```

Now you have your PlayableBehaviour but it needs to be able to get access to the `DigitalPaintingManager` as `PlayerData'. The easiest way to do this is to make it possible to add it to the `DigitalPaintingControllTrack`. To do this open `scripts/interaction/Timeline/DigitalPaintingControlTrack` in your editor and add the following line before the class definition:

```
[TrackClipType(typeof(SelectFromInventoryControlAsset))]
```

Now add this to the timeline and test your scene.

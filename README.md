# UnityTFSM

This is demo project of the simple Finate State Machine aproach used in my last project. This is not complete code but just some of important parts which I want to share.

### Events

Event - _is method call with message container as an argument._ All message containers based on same class BaseEvent. The function declaration loks like:

```
void OnEvent(object evt)
{
}
```

In the body of this function we can request type of argument, cast it and use as the data source. The container can be used for storing result of the method as well.

This aproach requre custom dispatching by case switch, but it has ome benefits.

### Spawning 

Template - _class used for instantiating prefab._ When we create instance of prefab the instance receive message OnSpawned. The message contains refference to template which is located in the scene. The template modify the instance for some exact case. At same time the template can be used by more that one instantiated prefabs.

Instance of the object can be placed to the scene by editor, and anyway initialized by template. In some cases the instance can be used without template.

There are two methods for used by spawning system OnSpawned, OnDespawned. 

```
void OnSpawned(OnSpawnEvent evt)
{
  // Initialize the object. Start FSM if it needs
}

void OnDespawned()
{
  // Deinitialize the object
}
```

After initialization every instance have the pointer to template. Can be used any method of spawning: 

- GameObject.Instantiate
- Custom Objects Pool
- Or just initialization of object class for 3D object already existed in the scene.



### FSM

To make state machine we have to declarate enum with states of it.

```
public enum ElevatorStates
{
    Undefined,
    WaitingState,
    MovingState
}
```

Now can be declarated the behaviour.

```
public class Elevator : TFsmEntity<ElevatorStates>
{
}
```

Now we can declarate the states.

```
public class Elevator : TFsmEntity<ElevatorStates>
{
  IEnumerator WaitingState()
  {
  }
  IEnumerator MovingState()
  {
  }
}
```

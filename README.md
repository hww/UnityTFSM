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

### Spawning Unspawning

Template - _class used for instantiating prefab._ When we create instance of prefab the instance receive message OnSpawned. The message contains refference to template which is located in the scene. The template modify the instance for some exact case. At same time the template can be used by more that one instantiated prefabs._

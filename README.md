# UnityTFSM

### Events

```
void OnEvent(object evt)
{
}
```

### Spawning Unspawning

Template - _class used for instantiating prefab. When we create instance of prefab the instance receive message OnSpawned. The message contains refference to template which is located in the scene. The template modify the instance for some exact case. At same time the template can be used by more that one instantiated prefabs._

using UnityEngine;
using System.Collections;

/// <summary>
/// The template is container of data used to initialize entities
/// It need to be used when more thatone entity uses same settings
/// Template is the object alocated in the scene. When we span any
/// entity we can tell which template to use for this entity.
/// 
/// Other way to look at the templates: template is the way to
/// modify prefab when we spawn it. 
/// </summary>
public class BaseTemplate : BaseBehaviour {

    public GameObject prefab;

    public GameObject Spawn(Vector3 position, Quaternion rotation, OnSpawnEvent evt)
    {
        GameObject go = GameObject.Instantiate(prefab, position, rotation) as GameObject;
        go.SendMessage("OnSpawned", evt);
        return go;
    }

    public GameObject Spawn(OnSpawnEvent evt)
    {
        return Spawn(Vector3.zero, Quaternion.identity, evt);
    }

    public void Despawn(GameObject go)
    {
        go.SendMessage("OnDespawned");
        Destroy(go);
    }
}


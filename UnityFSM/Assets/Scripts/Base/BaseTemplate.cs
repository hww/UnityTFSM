using UnityEngine;
using System.Collections;

/// <summary>
/// The template is container of data used to initialize entities
/// </summary>
public class BaseTemplate : BaseBehaviour {

    public GameObject prefab;

    /// <summary>
    /// Spawn the entity
    /// </summary>
    /// <param name="position">The position of entity</param>
    /// <param name="rotation">The rotation of entity</param>
    /// <param name="evt">On spawn entity message</param>
    /// <returns>New entity</returns>
    public GameObject Spawn(Vector3 position, Quaternion rotation, OnSpawnEvent evt)
    {
        GameObject go = GameObject.Instantiate(prefab, position, rotation) as GameObject;
        go.SendMessage("OnSpawned", evt);
        return go;
    }
    /// <summary>
    /// Spawn the entity at default position
    /// </summary>    
    /// <param name="evt">On spawn entity message</param>
    /// <returns>New entity</returns>
    public GameObject Spawn(OnSpawnEvent evt)
    {
        return Spawn(Vector3.zero, Quaternion.identity, evt);
    }
    /// <summary>
    /// Destroy the entity
    /// </summary>
    /// <param name="go">The object to destroy</param>
    public void Despawn(GameObject go)
    {
        go.SendMessage("OnDespawned");
        Destroy(go);
    }
}


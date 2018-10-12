using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBehaviour : MonoBehaviour
{
    /// <summary>
    /// Called immediately after  object spawning
    /// </summary>
    /// <param name="evt">Message object</param>
    public virtual void OnSpawned(object evt)
    {
    }

    /// <summary>
    /// Called before destroy object
    /// </summary>
    public virtual void OnDespawned()
    {
    }
}

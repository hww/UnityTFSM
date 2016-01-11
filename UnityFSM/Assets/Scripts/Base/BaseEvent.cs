using UnityEngine;
using System.Collections;

/// <summary>
/// Use as the base class for all events
/// </summary>
public class BaseEvent
{
    /// <summary>
    /// Event sender
    /// </summary>
    object sender;

    public BaseEvent(object sender)
    {
        this.sender = sender;
    }
}

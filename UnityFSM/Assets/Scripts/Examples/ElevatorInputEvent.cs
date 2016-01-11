using UnityEngine;
using System.Collections;

public class ElevatorInputEvent : BaseEvent
{
    /// <summary>
    /// All input events for elevator are here
    /// </summary>
    public enum EName
    {
        OnPushUpButton,
        OnPushDownButton
    }

    // The event name
    public EName name;

    // Current elevator's level
    // Have value of destination level when start moving
    // and current level when stop
    public int level;

    public ElevatorInputEvent(ElevatorInputController sender, EName name) : base(sender)
    {
        this.name = name;
    }

}
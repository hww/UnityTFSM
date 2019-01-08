public class ElevatorEvent : BaseEvent
{

    public enum EName
    {
        OnStartMoving,
        OnEndMoving,
    }

    // The event name
    public EName name;

    // Current elevator's level
    // Have value of destination level when start moving
    // and current level when stop
    public int level;

    public ElevatorEvent(Elevator elevator, EName name) : base(elevator)
    {
        this.name = name;
    }

    public ElevatorEvent(Elevator elevator, EName name, int level) : this(elevator, name)
    {
        this.level = level;
    }

}

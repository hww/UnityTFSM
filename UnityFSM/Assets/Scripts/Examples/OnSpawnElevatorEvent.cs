
/// <summary>
/// Example of ElevatorSpawn message
/// </summary>
public class OnSpawnElevatorEvent : OnSpawnEvent {

	// where to spawn the elevator
	public int level;

    public OnSpawnElevatorEvent(ElevatorTemplate temp)
        : base(temp)
    {
        this.level = 0; // default spawn at level 0
    }

    public OnSpawnElevatorEvent(ElevatorTemplate temp, int level)
        : base(temp)
    {
        this.level = level;
    }

    public ElevatorTemplate GetTemplate {  get { return template as ElevatorTemplate; } }

}
/// This is base event for the spawning system
public class OnSpawnEvent : BaseEvent
{
    /// <summary>
    /// Pointer to template which contains initialization
    /// settings for spawned instance
    /// </summary>
    public BaseTemplate template;

    /// <summary>
    /// Create event message
    /// </summary>
    /// <param name="temp">The initialization template</param>
    public OnSpawnEvent(BaseTemplate temp, object sender = null) : base(sender)
    {
        template = temp;
    }
}

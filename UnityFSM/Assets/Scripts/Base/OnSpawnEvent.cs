/// This is base event for the spawning system
public class OnSpawnEvent 
{
	// Pointer to template which contains initialization 
	// settings for spawned instance
	public BaseTemplate template;

	public OnSpawnEvent(BaseTemplate temp) 
    {
		template = temp;
	}
}

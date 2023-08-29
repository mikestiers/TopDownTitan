using UnityEngine.Events;
using Playniax.Ignition;

public class Initializer : EngineBehaviour
{
    public UnityEvent events;

    public override void OnStart()
    {
        events.Invoke();
    }
}

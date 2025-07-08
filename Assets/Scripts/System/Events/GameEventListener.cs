using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("`이벤트를 수신할 GameEvent`입니다. 이 이벤트가 발생하면 Response가 호출됩니다.")]
    public GameEvent Event;

    [Tooltip("`이벤트가 발생했을 때 호출될 Response`입니다. 이벤트가 발생하면 이 UnityEvent가 호출됩니다.")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}

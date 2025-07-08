using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("イベントを受信する`GameEvent`です。")]
    public GameEvent Event;

    [Tooltip("イベントが発生したときに呼び出される`UnityEvent`です。")]
    public UnityEvent Response;

    /// <summary>
    /// イベントリスナーがEnable状態になったとき自動的にEventに登録します。
    /// </summary>
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    /// <summary>
    /// イベントリスナーがDisable状態になったとき自動的にEventから登録を解除します。
    /// </summary>
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    /// <summary>
    /// イベントが発生したときに呼び出されるメソッドです。
    /// このメソッドは、登録されている`Response`を呼び出します。
    /// </summary>
    public void OnEventRaised()
    {
        Response.Invoke();
    }
}

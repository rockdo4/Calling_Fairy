using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 'GameEvent'はゲーム内で発生するイベントを定義するScriptableObjectです。
/// このイベントは`GameEventListener`を通じて受信され、イベントが発生すると
/// 登録されたすべてのリスナーに通知が送信されます。
/// </summary>
[CreateAssetMenu(menuName = "System/Events/Game Event")]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> listeners = new List<GameEventListener>();

    /// <summary>
    /// イベントが発生したときに呼び出されるメソッドです。
    /// このメソッドは、登録されているすべてのリスナーに通知を送信します。
    /// </summary>
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    /// <summary>
    /// リスナーを登録します。
    /// </summary>
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    /// <summary>
    /// リスナーの登録を解除します。
    /// </summary>
    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}

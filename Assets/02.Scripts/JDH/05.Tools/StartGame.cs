using UnityEngine;
using UnityEngine.Events;

public class StartGame : MonoBehaviour
{
    public UnityEvent StartOption;
    void Start()
    {
        StartOption.Invoke();
    }
}

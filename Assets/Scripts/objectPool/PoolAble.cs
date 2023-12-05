using UnityEngine;
using UnityEngine.Pool;

public class PoolAble : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    public bool IsPooled { get; set; } = false;

    public void ReleaseObject()
    {
        Pool.Release(gameObject);
        IsPooled = true;
    }
    public void RetrieveFromPool()
    {
        IsPooled = false; 
    }
}
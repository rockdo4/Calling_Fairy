using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Effects : MonoBehaviour
{
    protected InGameEffectPool pool;
    public EffectType effectType;

    protected void Awake()
    {
        pool = GameObject.FindWithTag(Tags.EffectPool).GetComponent<InGameEffectPool>();        
    }
    public virtual void SetPositionAndRotation(Vector3 position, bool isFlip = false, Vector3 rotation = new Vector3())
    {
        transform.position = position;
        transform.eulerAngles = rotation;
        if(isFlip)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    protected virtual void OnDisable()
    {
        pool.ReturnEffect(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private ParticleSystem effect;
    public EffectType effectType;
    public InGameEffectPool pool;

    private void Awake()
    {
        effect = GetComponent<ParticleSystem>();        
        pool = GameObject.FindWithTag(Tags.EffectPool).GetComponent<InGameEffectPool>();
        var main = effect.main;
        main.loop = false;
    }
    public void SetData(bool isLoop)
    {
        var main = effect.main;
        main.loop = isLoop;
    }
    private void OnEnable()
    {
        effect.Stop();
        effect.Play();
    }
    private void Update()
    {
        if (!effect.isPlaying)
        {
            pool.ReleaseEffect(gameObject);
        }
    }
    public void SetPositionAndRotation(Vector3 position, Vector3 rotation = new Vector3())
    {
        transform.position = position;
        transform.eulerAngles = rotation;
    }

    public void Stop()
    {
        effect.Stop();
    }
    private void OnDisable()
    {
        pool.ReleaseEffect(gameObject);
    }
}

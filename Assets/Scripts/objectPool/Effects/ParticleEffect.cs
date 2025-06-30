using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : Effects
{
    protected ParticleSystem effect;

    protected new void Awake()
    {
        base.Awake();
        effect = GetComponentInChildren<ParticleSystem>();
        var main = effect.main;
        main.loop = false;
    }
    public void SetLoop()
    {
        var main = effect.main;
        main.loop = true;
    }
    protected virtual void OnEnable()
    {
        effect.Stop();
        effect.Play();
    }
    protected void Update()
    {
        if (!effect.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
    public void Stop()
    {
        effect.Stop();
    }
}

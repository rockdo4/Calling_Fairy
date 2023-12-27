using UnityEngine;

public class SkillEffects : ParticleEffect
{
    protected bool isFollow;
    protected Transform parents;

    protected new void Awake()
    {
        base.Awake();
        effect = GetComponentInChildren<ParticleSystem>();
        var main = effect.main;
        main.loop = false;
        isFollow = false;
        parents = gameObject.transform.parent;
    }
    private void OnEnable()
    {
        base.OnEnable();
        isFollow = false;
    }
    public void SetPositionAndRotation(Vector3 position, bool isFairy, Vector3 rotation = new Vector3(), GameObject parents = null)
    {
        if (parents != null)
        {
            isFollow = false;
            gameObject.transform.parent = parents.transform;
        }
        else
        {
            isFollow = true;
        }
        base.SetPositionAndRotation(position, !isFairy, rotation);
    }

    protected void OnDisable()
    {
        base.OnDisable();
        if (isFollow)
        {
            gameObject.transform.parent = parents;
        }
    }
}

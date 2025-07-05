using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public enum State
{
    Idle,
    Move,
    Stun,
    AttackNormal,
    AttackBow,
    SkillNormal,
    SkillMagic,
}

public class TownCharMove : MonoBehaviour
{
    private const string ani = "townAni";

    private BoxCollider2D boxCollider;
    private Vector2 moveMax;
    private Vector2 moveMin;
    public State state;
    private float idleTime;
    public float speed = 2.0f;
    private Vector2 destination;
    private Animator animator;
    public int minIdleTime = 1;
    public int maxIdleTime = 4;
    public int stunRecoverTime = 3;
    private GameObject town;
    //public AnimatorController newController;
    private void Start()
    {
        town = GameObject.FindWithTag(Tags.Town);
        boxCollider = town.GetComponentInParent<BoxCollider2D>();
        //Debug.Log(boxCollider.name);
        var myBoxCollider = GetComponent<BoxCollider2D>();
        myBoxCollider.isTrigger = true;
        transform.tag = Tags.Player;
        gameObject.layer = LayerMask.NameToLayer(Layers.Player);
        var sortingGroup = transform.AddComponent<SortingGroup>();
        animator = GetComponentInChildren<Animator>();
        //var newAnimator = town.GetComponent<FirstTownCharSetting>().animatorController;
        animator.runtimeAnimatorController = ani.GetAsset<RuntimeAnimatorController>();
        //animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/TownAnimator");
        //animator.runtimeAnimatorController = newAnimator;
        animator.transform.AddComponent<TownAnimationConnector>();
        //animator.runtimeAnimatorController = newController;
        var animatorconnector = GetComponentInChildren<AnimationConnector>();
        Destroy(animatorconnector);
        moveMax = boxCollider.bounds.max;
        moveMin = boxCollider.bounds.min;

        var cf = gameObject.AddComponent<ClickAndFollow>();
        cf.SetOffset(new Vector3(0, -125, 0));
        cf.OnStartHolding += StartHolding;
        cf.OnEndHolding += EndHolding;

        //test
        cf.OnStartHolding += () => Debug.Log("Start Holding");
        cf.OnEndHolding += () => Debug.Log("End Holding");
        //

        state = State.Idle;
    }

    private void Update()
    {
        //Debug.Log(state);
        switch (state)
        {
            case State.Idle:
                IdleInTown();
                animator.SetBool(Triggers.IsMoving, false);
                break;
            case State.Move:
                MoveInTown();
                break;
            case State.Stun:
                StunInTown();
                break;
            case State.AttackNormal:
                AttackNormalInTown();
                break;
            case State.AttackBow:
                AttackBowInTown();
                break;
            case State.SkillNormal:
                SkillNormalInTown();
                break;
            case State.SkillMagic:
                SkillMagicInTown();
                break;
        }
    }

    private void SkillMagicInTown()
    {
        animator.SetTrigger(Triggers.ReinforcedSkill);
    }

    private void SkillNormalInTown()
    {
        animator.SetTrigger(Triggers.NormalSkill);
    }

    private void AttackBowInTown()
    {
        animator.SetTrigger(Triggers.ProjectileAttack);
    }

    private void AttackNormalInTown()
    {
        animator.SetTrigger(Triggers.MeleeAttack);
        
    }
    public void EndAnimation()
    {
        state = State.Idle;
    }
    private void StunInTown()
    {
        animator.SetBool(Triggers.IsStunning, true);
        idleTime += Time.deltaTime;
        if (idleTime > stunRecoverTime)
        {
            state = State.Idle;
            idleTime = 0.0f;
            animator.SetBool(Triggers.IsStunning, false);
        }
    }

    private void IdleInTown()
    {
        idleTime += Time.deltaTime;
        int randomIdleTime = Random.Range(minIdleTime, maxIdleTime);
        if (idleTime > randomIdleTime)
        {
            state = State.Move;
            idleTime = 0.0f;
            SetTargetPos();
        }
    }

    private void SetTargetPos()
    {
        destination = new Vector2(Random.Range(moveMin.x, moveMax.x), Random.Range(moveMin.y, moveMax.y));
    }

    private void MoveInTown()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if ((Vector2)transform.position == destination)
        {
            state = State.Idle;
            return;
        }
        if (transform.position.x - destination.x >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        animator.SetBool(Triggers.IsMoving, true);
    }

    private void StartHolding()
    {
        animator.SetBool(Triggers.IsStunning, true);
        stunRecoverTime = Int32.MaxValue;
    }

    private void EndHolding()
    {
        animator.SetBool(Triggers.IsStunning, false);
        stunRecoverTime = 1;
    }

}
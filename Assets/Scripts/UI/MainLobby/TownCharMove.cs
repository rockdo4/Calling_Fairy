using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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

    private void Start()
    {
        town = GameObject.FindWithTag(Tags.Town);
        boxCollider = town.GetComponentInParent<BoxCollider2D>();

        var myBoxCollider = GetComponent<BoxCollider2D>();
        myBoxCollider.isTrigger = true;
        transform.tag = Tags.Player;
        var sortingGroup = transform.AddComponent<SortingGroup>();
        animator = GetComponentInChildren<Animator>();
 
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/TownAnimator");

        animator.transform.AddComponent<TownAnimationConnector>();

        var animatorconnector = GetComponentInChildren<AnimationConnector>();
        Destroy(animatorconnector);
        moveMax = boxCollider.bounds.max;
        moveMin = boxCollider.bounds.min;
        state = State.Idle;
    }

    private void Update()
    {
        //Debug.Log(state);
        switch (state)
        {
            case State.Idle:
                IdleInTown();
                animator.SetBool("IsMoving", false);
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
        animator.SetTrigger("ReinforcedSkill");
    }

    private void SkillNormalInTown()
    {
        animator.SetTrigger("NormalSkill");
    }

    private void AttackBowInTown()
    {
        animator.SetTrigger("ProjectileAttack");
    }

    private void AttackNormalInTown()
    {
        animator.SetTrigger("MeleeAttack"); 
        
    }
    public void EndAnimation()
    {
        state = State.Idle;
    }
    private void StunInTown()
    {
        animator.SetBool("IsStunning", true);
        idleTime += Time.deltaTime;
        if (idleTime > stunRecoverTime)
        {
            state = State.Idle;
            idleTime = 0.0f;
            animator.SetBool("IsStunning", false);
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
        animator.SetBool("IsMoving", true);
    }
}
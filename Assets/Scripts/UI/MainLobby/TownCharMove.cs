using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

public enum State
{
    Idle,
    Move,
    Attack,
}

public class TownCharMove : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector2 moveMax;
    private Vector2 moveMin;
    private State state;
    private float idleTime;
    public float speed = 2.0f;
    private Vector2 destination;
    private Animator animator;
    //public AnimatorController newController;
    private void Start()
    {
        boxCollider = GameObject.FindWithTag(Tags.Town).GetComponentInParent<BoxCollider2D>();
        //Debug.Log(boxCollider.name);
        var myBoxCollider = GetComponent<BoxCollider2D>();
        myBoxCollider.isTrigger = true;
        var sortingGroup = transform.AddComponent<SortingGroup>();
        animator = GetComponentInChildren<Animator>();
        //animator.runtimeAnimatorController = newController;
        var animatorconnector = GetComponentInChildren<AnimationConnector>();
        Destroy(animatorconnector);
        moveMax = boxCollider.bounds.max;
        moveMin = boxCollider.bounds.min;
        state = State.Idle;
    }

    private void Update()
    {
        Debug.Log(state);
        switch (state)
        {
            case State.Idle:
                IdleInTown();
                animator.SetBool("IsMoving", false);
                break;
            case State.Move:
                MoveInTown();
                break;
        }
    }


    private void IdleInTown()
    {
        idleTime += Time.deltaTime;
        int randomIdleTime = Random.Range(1, 4);
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
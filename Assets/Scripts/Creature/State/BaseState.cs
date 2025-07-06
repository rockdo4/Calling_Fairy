using UnityEngine;

public abstract class BaseState
{
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
}

public class CreatureBase : BaseState
{
    protected CreatureController creatureController;
    protected Creature creature;
    protected float timer;
    private int layerMask;

    protected CreatureBase(CreatureController creatureController)
    {
        this.creatureController = creatureController;
        creature = creatureController.creature;
        //var isPlayer = creature.CompareTag(Tags.Player);
        layerMask = LayerMask.GetMask(Layers.Monster, Layers.Player);
    }

    public override void OnEnter()
    {
        timer = 0f;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
    }

    public override void OnFixedUpdate()
    {
    }

    /// <summary>
    /// 범위 내 타겟이 있는지 확인 및 타겟 대상 최신화
    /// </summary>
    /// <returns>범위 내 타겟이 있는지 여부</returns>
    protected bool CheckRange()
    {
        creature.targets.Clear();

        var pos = creature.transform.position;
        var allTargets = Physics2D.OverlapCircleAll(pos, creature.Status.attackRange, layerMask);
        foreach (var target in allTargets)
        {
            if (target.CompareTag(creature.tag) ||
                !target.TryGetComponent<Creature>(out var script))
                continue;

            creature.targets.Add(script);
        }

        return creature.targets.Count != 0;
    }
}
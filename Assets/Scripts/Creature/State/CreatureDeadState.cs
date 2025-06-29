using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CreatureDeadState : CreatureBase
{
    private SpriteRenderer[] spriteRenderers;
    private List<Color> colors = new();
    public Color turnInto = new(1,1,1,0);

    public CreatureDeadState(CreatureController creatureController) : base(creatureController)
    {
    }
    public override void OnEnter()
    {
        //Revival revival = null;
        base.OnEnter();
        //foreach(var buff in creature.activedBuffs)
        //{
        //    revival = buff as Revival;
        //    if (revival != null)
        //        break;
        //}
        //if (revival != null)
        //{
        //    creature.activedBuffs.Remove(revival);
        //    return;
        //}
        creature.animator.SetTrigger(Triggers.Dead);
        creature.isDead = true;
        creature.gameObject.layer = LayerMask.NameToLayer(Layers.Dead);
        creature.GetComponentInChildren<SortingGroup>().sortingLayerID = SortingLayer.NameToID("DeadCreature");
        creature.HPBars.SetActive(false);
        spriteRenderers = creature.GetComponentsInChildren<SpriteRenderer>();
        foreach(var spriteRenderer in spriteRenderers)
        {
            colors.Add(spriteRenderer.color);
        }
        if(creature is Fairy)
        {
            turnInto = new Color(0.5f, 0.5f, 0.5f, 1);
        }    
    }
    public override void OnExit()
    {
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            //TODO: 이거 쉐이터 코드같은걸로 수정 할 방법 찾기
            spriteRenderers[i].color = Color.Lerp(colors[i], turnInto, timer / Creature.DieSpeed);
        }
    }
    public override void OnFixedUpdate()
    {
    }
}

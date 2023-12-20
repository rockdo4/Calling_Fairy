using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDeadState : CreatureBase
{
    private SpriteRenderer[] spriteRenderers;
    private List<Color> colors = new();
    public Color limpidity = new(1,1,1,0);

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
        creature.Animator.SetTrigger("Dead");        
        creature.isDead = true;
        creature.gameObject.layer = LayerMask.NameToLayer(Layers.Dead);
        creature.HPBars.SetActive(false);
        spriteRenderers = creature.GetComponentsInChildren<SpriteRenderer>();
        foreach(var spriteRenderer in spriteRenderers)
        {
            colors.Add(spriteRenderer.color);
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
            spriteRenderers[i].color = Color.Lerp(colors[i], limpidity, timer / creature.DieSpeed);
        }
    }
    public override void OnFixedUpdate()
    {
    }
}

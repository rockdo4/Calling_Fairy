using UnityEngine;

public class AnimationConnector : MonoBehaviour
{
    Creature creature;
    private void Start()
    {
        creature = GetComponentInParent<Creature>();
    }
    public void Attack()
    {
        creature.Attack();
    }
    public void AttckFinished()
    {
        creature.AttckFinished();
    }
    public void CastNormalSkill()
    {
        creature.CastNormalSkill();
    }
    public void CastReinforcedSkill()
    {
        creature.CastReinforcedSkill();
    }
    public void CastSpecialSkill()
    {
        creature.CastSpecialSkill();
    }
    public void SkillDone()
    {
        creature.SkillDone();
    }
}

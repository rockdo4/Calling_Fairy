using UnityEngine;

public class ProjectileSkill : SkillBase
{
    public override void Active()
    {
        var projectile = Object.Instantiate(GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>().skillProjectile, owner.transform.position, Quaternion.identity);
        projectile.layer = owner.gameObject.layer;
        projectile.tag = owner.gameObject.tag;
        var table = DataTableMgr.GetTable<SkillProjectileTable>();
        var projectileData = table.dic[skillData.skill_projectileID];
        Projectile script;
        if (projectileData.proj_highest == 0)
        {
            script = projectile.AddComponent<SkillProjectileDirect>();
        }
        else
        {
            script = projectile.AddComponent<SkillProjectileHowitzer>();
        }        
        script.SetData(projectileData, attackInfos, skillData);
        script.SetTargetPos(targets[0][0]);
    }
}

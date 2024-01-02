using UnityEngine;

public class ProjectileSkill : SkillBase
{
    public override void Active()
    {
        var table = DataTableMgr.GetTable<SkillProjectileTable>();
        var projectileData = table.dic[skillData.skill_projectileID];
        var offset = new Vector3(projectileData.proj_startOffsetX, projectileData.proj_startOffsetY);
        var projectile = Object.Instantiate(GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>().skillProjectile, (Vector3)owner.Rigidbody.worldCenterOfMass + offset, Quaternion.identity);
        projectile.layer = owner.gameObject.layer;
        projectile.tag = owner.gameObject.tag;
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
        GetTargets();
        if(script is SkillProjectileHowitzer)
        {
            if(targets[0].Count != 0)
            {
                script.SetTargetPos(targets[0][0]);
            }
        }
    }
}

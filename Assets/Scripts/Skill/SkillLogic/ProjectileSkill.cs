using UnityEngine;

public class ProjectileSkill : SkillBase
{
    public override void Active()
    {
        Debug.Log("ProjectileSkill Active");
        var projectile = Object.Instantiate(GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>().skillProjectile, owner.transform.position, Quaternion.identity);
        projectile.layer = owner.gameObject.layer;
        projectile.tag = owner.gameObject.tag;
        var script = projectile.AddComponent<SkillProjectile>();
        var table = DataTableMgr.GetTable<SkillProjectileTable>();
        var projectileData = table.dic[skillData.skill_projectileID];
        script.SetData(projectileData, attackInfos, skillData);
        script.SetTargetPos(targets[0][0]);
    }
}

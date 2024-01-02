using UnityEngine;

public class SkillButtonEffect : MonoBehaviour
{
    Vector3 pTransform;
    SkillSpawn spPos;
    ObjectPoolManager objPool;
    // Start is called before the first frame update
    private void Awake()
    {
        spPos = GameObject.FindWithTag(Tags.SkillSpawner).GetComponentInParent<SkillSpawn>();
        objPool= GameObject.FindWithTag(Tags.ObjectPoolManager).GetComponent<ObjectPoolManager>();
    }
    
    public void DieEffectOn()   
    {
        pTransform = transform.position;
        var go = objPool.GetGo("ButtonParticle");
        go.transform.position = pTransform;
        go.transform.SetParent(spPos.transform);
        //1�ʵڿ� ������Ʈ Ǯ�� ����
        //if (addTime >= 1f)
        //{
        //    go.transform.SetParent(objPool.transform);
        //    objPool.ReturnGo(go);
        //    addTime = 0;
        //}
    }
}

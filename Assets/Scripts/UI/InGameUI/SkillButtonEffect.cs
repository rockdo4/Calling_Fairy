using UnityEngine;

public class SkillButtonEffect : MonoBehaviour
{
    public ParticleSystem oneButtonUseParticle;
    public GameObject particle1;
    Vector3 pTransform;
    SkillSpawn spPos;
    ObjectPoolManager objPool;
    float addTime;
    // Start is called before the first frame update
    private void Awake()
    {
        spPos = GameObject.FindWithTag(Tags.SkillSpawner).GetComponentInParent<SkillSpawn>();
        objPool= GameObject.FindWithTag(Tags.ObjectPoolManager).GetComponent<ObjectPoolManager>();
    }
    public void DieEffectOn()
    {
        addTime += Time.deltaTime;
        pTransform = transform.position;
        //var Go = Instantiate(particle1, pTransform, Quaternion.identity);
        
        var go = objPool.GetGo("ButtonParticle");
        go.transform.position = pTransform;
        go.transform.SetParent(spPos.transform);
        //1초뒤에 오브젝트 풀로 리턴
        if (addTime >= 1f)
        {
            objPool.ReturnGo(go);
            addTime = 0;
        }
    }
}

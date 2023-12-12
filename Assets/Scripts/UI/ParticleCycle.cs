using UnityEngine;

public class ParticleCycle : MonoBehaviour
{
    ObjectPoolManager objPool;
    float addTime;
    private void Awake()
    {
        objPool = GameObject.FindWithTag(Tags.ObjectPoolManager).GetComponent<ObjectPoolManager>();    
    }

    // Update is called once per frame
    void Update()
    {
        this.addTime += Time.deltaTime;
        if (this.addTime >= 1f)
        {
            this.gameObject.transform.SetParent(objPool.transform);
            this.objPool.ReturnGo(gameObject);
            this.addTime = 0;
        }
    }
}

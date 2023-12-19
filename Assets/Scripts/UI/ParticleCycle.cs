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
    private void OnEnable()
    {
        transform.localScale = new Vector3(12, 12);
        //transform.localScale = new Vector3(120, 120);
    }
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

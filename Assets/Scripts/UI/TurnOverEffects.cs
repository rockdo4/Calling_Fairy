using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnOverEffects : MonoBehaviour
{
    ObjectPoolManager objPool;
    float addTime;
    Color color;
    private void Awake()
    {
        objPool = GameObject.FindWithTag(Tags.ObjectPoolManager).GetComponent<ObjectPoolManager>();
        Debug.Log($"처음: {transform.localScale}");
        this.transform.localScale = Vector3.one;
        Debug.Log($"중간 이전: {transform.localScale}");
        color = GetComponent<Image>().color;
    }
    private bool firstSet = false;
    // Update is called once per frame
    private void OnEnable()
    {
        this.transform.localScale = Vector3.one;
    }
    void Update()
    {
        
        //this.transform.localScale = Vector3.one;
        this.addTime += Time.deltaTime;
        Debug.Log("지남");
        TurnOverEffectStart();
        if (this.addTime >= 0.5f)
        {
            
            this.gameObject.transform.SetParent(objPool.transform);
            this.objPool.ReturnGo(gameObject);
            this.transform.localScale = Vector3.one;
            this.addTime = 0;
        }
    }

    void TurnOverEffectStart()
    {
        this.transform.localScale = new Vector3(transform.localScale.x * (1 - addTime), transform.localScale.y * (1 - addTime));
        //color.a = 1 - addTime;
    }
}

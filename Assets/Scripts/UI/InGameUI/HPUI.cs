using UnityEngine;
using UnityEngine.UI;
public class HPUI : MonoBehaviour
{
    // Start is called before the first frame update
    StageManager stageManager;
    public Image[] HpUI = new Image[3];
    float[] MaxHp = new float[3];
    float[] curHp = new float[3];
    bool isFirst = false;
    private void Awake()
    {
       stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isFirst)
        {
            HpUIFirst();
            isFirst = true;
        }


    }

    public void HpUIFirst()
    {
        for (int i = 0; i < stageManager.playerParty.Count; i++)
        {
            MaxHp[i] = stageManager.playerParty[i].Status.hp;
            curHp[i] = stageManager.playerParty[i].curHP;
            //Debug.Log($"{i} 번째 캐릭터의 체력은 {curHp[i]} 입니다.");
        }
    }
    //데미지 받았을 때 호출해야하는 함수
    //When GetDamage call this function
    public void HPUIUpdate()
    {
        for (int i = 0; i < stageManager.playerParty.Count; i++)
        {
            curHp[i] = stageManager.playerParty[i].curHP;
            HpUI[i].fillAmount = curHp[i] / MaxHp[i];
           // Debug.Log($"{i} 번째 캐릭터의 체력은 {curHp[i]} 입니다.");
        }
    }
}

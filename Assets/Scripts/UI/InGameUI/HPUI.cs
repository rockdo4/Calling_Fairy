using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPUI : MonoBehaviour
{
    // Start is called before the first frame update
    StageManager stageManager;
    public Image[] HpUI = new Image[3];
    float[] MaxHp = new float[3];
    float[] curHp = new float[3];
    private void Awake()
    {
       stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }
    void Start()
    {
        for(int i = 0; i<stageManager.playerParty.Count;i++)
        {
            MaxHp[i] = stageManager.playerParty[i].Status.hp;
            curHp[i] = stageManager.playerParty[i].curHP;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<stageManager.playerParty.Count;i++)
        {
            HpUI[i].fillAmount = curHp[i]/MaxHp[i];
        }
        
    }
}

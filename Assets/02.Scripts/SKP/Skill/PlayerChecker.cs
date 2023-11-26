using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    public static PlayerChecker Instance;
    GameObject[] fairy = new GameObject[3];
    public bool[] fairyDieCheck = new bool[3];
    bool getFairyInfo = false;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //최초 1회 요정 정보 받아오기
        if (!getFairyInfo)
            GetFairyDataFirst();
        CheckFairyHp();

    }

    void GetFairyDataFirst()
    {
        fairy = GameObject.FindGameObjectsWithTag("Player");
        if (fairy == null)
        {
            for (int i = 0; i < fairy.Length; i++)
            {
                fairyDieCheck[i] = false;
            }
        }
        getFairyInfo = true;
    }

    //플레이어 죽은지 확인
    void CheckFairyHp()
    {
        for (int i = 0; i < fairy.Length; i++)
        {
            if (fairy[i].GetComponent<Fairy>().curHP <= 0)//조건 변경   
            {
                fairyDieCheck[i] = true;
                Debug.Log($"{i}번째 요정 사망");
                //ChanageSkillImage();
            }
        }
    }

    //플레이어 스킬 변경
    



}

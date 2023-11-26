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
        //���� 1ȸ ���� ���� �޾ƿ���
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

    //�÷��̾� ������ Ȯ��
    void CheckFairyHp()
    {
        for (int i = 0; i < fairy.Length; i++)
        {
            if (fairy[i].GetComponent<Fairy>().curHP <= 0)//���� ����   
            {
                fairyDieCheck[i] = true;
                Debug.Log($"{i}��° ���� ���");
                //ChanageSkillImage();
            }
        }
    }

    //�÷��̾� ��ų ����
    



}

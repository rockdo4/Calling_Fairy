using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct EquipSocket
{
    public int weapon1Id;
    public int weapon2Id;
    public int armor1Id;
    public int armor2Id;
    public int jewelry1Id;
    public int jewelry2Id;
}

public class FairyCard : Card
{
    //장비 소켓
    public EquipSocket socket;
    public int experience;
    public int starLv;
    public int rankId;

    //private Button button;
    public FairyCard(int id)
    {
        ID =  id;
        //ID에서 캐릭터 고유 번호만 추출
        PrivateID = id;
    }

    private void Awake()
    {
        //button = GetComponent<Button>();
        //button.onClick +=     //성장 UI 활성화
    }

}

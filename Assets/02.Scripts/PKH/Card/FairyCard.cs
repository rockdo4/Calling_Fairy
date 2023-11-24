using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public uint experience;
    public int starLv;
    public int rankId;
    public FairyCard(int id)
    {
        ID =  id;
        //ID에서 캐릭터 고유 번호만 추출
        PrivateID = id;
    }

}

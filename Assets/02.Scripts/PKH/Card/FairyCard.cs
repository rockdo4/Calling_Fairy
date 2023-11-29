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
    public int grade = 1;   //임시값
    public int rankId;

    //private Button button;
    public FairyCard(int id)
    {
        ID =  id;
        //ID에서 캐릭터 고유 번호만 추출
        PrivateID = id;
    }


    public void LevelUp(int ex)
    {
        Experience += ex;
        var table = DataTableMgr.GetTable<CharacterTable>(); 
        if (Experience < table.dic[ID.ToString()].CharExp)
            return;

        if (grade >= table.dic[table.dic[ID.ToString()].CharNextLevel.ToString()].CharMinGrade)
        {
            Experience = table.dic[ID.ToString()].CharExp;
            return;
        }
           
        Experience -= table.dic[ID.ToString()].CharExp;
        ID = table.dic[ID.ToString()].CharNextLevel;
    }

}

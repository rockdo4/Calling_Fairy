using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IDestructable
{
    private int monsterDroptableId;
    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }

    public void SetData(int monsterDroptableId)
    {
        this.monsterDroptableId = monsterDroptableId;
    }

    public void OnDestructed()
    {
        if(CheckDrop(GameManager.Instance.StageId))
        {
            GetItem();
        }        
    }

    private bool CheckDrop(int id)
    {
        var table = stageManager.thisIsStageData;
        var stagetable = table.dic[id];
        var dropRate = stagetable.stageDorpPercent;
        return Random.Range(0, 100) < dropRate;
    }

    private void GetItem()
    {
        var table = stageManager.thisIsMonsterDropData;
        var stagetable = table.dic[monsterDroptableId];
        var randVal = Random.Range(0, 100);
        int sum = 0;
        foreach (var item in stagetable.Drops)
        {
            if (item.Item2 == 0)
                continue;
            sum += item.Item2;
            if (randVal < sum)
            {
                var itemTable = stageManager.thisIsItemData;
                var itemData = itemTable.dic[item.Item1];
                switch( itemData.sort) 
                {
                    case 4:
                        InvManager.AddItem(new EquipmentPiece(itemData.ID));                        
                        break;
                    case 6:
                    case 7:
                    case 8:
                        InvManager.AddItem(new SpiritStone(itemData.ID));
                        break;
                    default:
                        InvManager.AddItem(new Item(itemData.ID));
                        break;
                }
                InvManager.ingameInv.AddItem(new Item(itemData.ID));
                return;
            }
        }
    }
}

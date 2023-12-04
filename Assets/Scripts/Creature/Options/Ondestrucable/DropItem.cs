using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IDestructable
{
    private int monsterDroptableId;

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
        var table = DataTableMgr.GetTable<StageTable>();
        var stagetable = table.dic[id];
        var dropRate = stagetable.stageDorpPercent;
        return Random.Range(0, 100) < dropRate;
    }

    private void GetItem()
    {
        var table = DataTableMgr.GetTable<MonsterDropTable>();
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
                var itemTable = DataTableMgr.GetTable<ItemTable>();
                var itemId = itemTable.dic[item.Item1];
                //æ∆¿Ã≈€ »πµÊ ∞¸∑√ √≥∏Æ
                return;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FairyInvView : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private GrowthSystem growthSystem;
    [SerializeField]
    private TabGroup tabGroup;
    [SerializeField]
    private List<UnityEvent<Transform>> seters = new List<UnityEvent<Transform>>();
    [SerializeField]
    private List<Transform> contents = new List<Transform>();


    public enum SortOption
    {
        LevelAsc,
        LevelDesc,
        NameAsc,
        NameDesc,
        GradeAsc,
        GradeDesc,
        RankAsc,
        RankDesc,
        BattlePowerAsc,
        BattlePowerDesc,
    }

    private void Start()
    {
        dropdown?.onValueChanged?.AddListener(SetInvView);
    }

    private void OnEnable()
    {
        FairyInvOrderBy(0);
    }

    public void SetInvView(int option)
    {
        var resultEnumerable = CategorizeByProperty(FairyInvOrderBy((SortOption)option));

        for (int i = 0; i < contents.Count; i++)
        {
            
        }
    }

    public IEnumerable<FairyCard> FairyInvOrderBy(SortOption option)
    {
        switch (option)
        {
            case SortOption.LevelAsc:
                return InvManager.fairyInv.Inven.Values.OrderBy(failyCard => failyCard.Level);
            case SortOption.LevelDesc:
                return InvManager.fairyInv.Inven.Values.OrderByDescending(failyCard => failyCard.Level);
            case SortOption.NameAsc:
                return InvManager.fairyInv.Inven.Values.OrderBy(fairyCard => fairyCard.Name);
            case SortOption.NameDesc:
                return InvManager.fairyInv.Inven.Values.OrderByDescending(fairyCard => fairyCard.Name);
            case SortOption.GradeAsc:
                return InvManager.fairyInv.Inven.Values.OrderBy(fairyCard => fairyCard.Grade);
            case SortOption.GradeDesc:
                return InvManager.fairyInv.Inven.Values.OrderByDescending(fairyCard => fairyCard.Grade);
            case SortOption.RankAsc:
                return InvManager.fairyInv.Inven.Values.OrderBy(fairyCard => fairyCard.Rank);
            case SortOption.RankDesc:
                return InvManager.fairyInv.Inven.Values.OrderByDescending(fairyCard => fairyCard.Rank);
            case SortOption.BattlePowerAsc:
                return InvManager.fairyInv.Inven.Values.OrderBy(fairyCard => fairyCard.FinalStat.battlePower);
            case SortOption.BattlePowerDesc:
                return InvManager.fairyInv.Inven.Values.OrderByDescending(fairyCard => fairyCard.FinalStat.battlePower);
            default:
                // 유효하지 않은 입력에 대한 예외 발생
                throw new ArgumentOutOfRangeException(nameof(option), "Invalid sorting option");
        }
    }

    public IEnumerable<IGrouping<int, FairyCard>> CategorizeByProperty(IEnumerable<FairyCard> sortedInv)
    {
        return sortedInv.GroupBy(FairyCard => DataTableMgr.GetTable<CharacterTable>().dic[FairyCard.ID].CharProperty);
    }


    public void Clear()
    {
        foreach (var content in contents)
        {
            ResetAndReturnPoolableObjects(content);
        }
    }

    private void ResetAndReturnPoolableObjects(Transform content)
    {
        var poolableObjects = content.GetComponentsInChildren<PoolAble>();
        foreach (var poolableObject in poolableObjects)
        {
            ResetButton(poolableObject);
            ReturnToPool(poolableObject);
        }
    }

    private void ResetButton(PoolAble poolableObject)
    {
        var button = poolableObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.enabled = true;
        }
    }

    private void ReturnToPool(PoolAble poolableObject)
    {
        UIManager.Instance.objPoolMgr.ReturnGo(poolableObject.gameObject);
        poolableObject.transform.SetParent(UIManager.Instance.objPoolMgr.transform);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyInvView : MonoBehaviour
{
    [SerializeField]
    private UI growthUI;
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private GrowthController growthContoller;
    [SerializeField]
    private TabGroup tabGroup;

    [SerializeField]
    private List<Category> categorys = new List<Category>();
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

    public enum Category
    {
        All = 0,
        Dealer,
        Tanker,
        Buffer,
    }

    private void Start()
    {
        dropdown?.onValueChanged?.AddListener(SetInvView);
    }

    private void OnEnable()
    {
        SetInvView(0);
    }


    public void SetInvView(int option)
    {
        if (categorys.Count != contents.Count)
        {
            Debug.LogError("카테고리와 컨텐츠의 개수가 일치하지 않습니다.");
            return;
        }

        Clear();

        var sortedFairyCards = CategorizeByProperty(FairyInvOrderBy((SortOption)option));
        int totalCategoryIndex = categorys.FindIndex(category => category == Category.All);

        foreach (var group in sortedFairyCards)
        {
            int categoryIndex = categorys.FindIndex(category => category == (Category)group.Key);
            CreateAndSetupFairyIcons(group, categoryIndex);

            if (totalCategoryIndex != -1)
            {
                CreateAndSetupFairyIcons(group, totalCategoryIndex);
            }
        }
    }

    private void CreateAndSetupFairyIcons(IGrouping<int, FairyCard> group, int parentIndex)
    {
        foreach (var fairyCard in group)
        {
            var fairyIcon = GetFairyIcon(fairyCard, contents[parentIndex]);
            SetupButtonActions(fairyIcon);
        }
    }

    private GameObject GetFairyIcon(FairyCard card, Transform parent)
    {
        var go = UIManager.Instance.objPoolMgr.GetGo("FairyIcon");
        go.transform.SetParent(parent);
        go.GetComponent<FairyIcon>().Init(card);
        return go;
    }

    private void SetupButtonActions(GameObject fairyIcon)
    {
        var button = fairyIcon.GetComponent<Button>();
        button?.onClick.AddListener(() => growthContoller.SelectFairy = fairyIcon.GetComponent<FairyIcon>().inventoryItem as FairyCard);
        button?.onClick.AddListener(growthUI.ActiveUI);

        // 주석 처리된 버튼 리스너 할당 로직을 여기에 추가
        // button?.onClick.AddListener(() => fairyGrowthUI.Init(card as FairyCard));
        // button?.onClick.AddListener(fairyGrowthUI.GetComponent<UI>().ActiveUI);
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

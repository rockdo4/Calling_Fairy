using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FairyGrowthUI : UI
{
    [Header("Common")]
    public GameObject itemButtonPrefab;
    public GameObject itemIconPrefab;
    public View leftCardView;
    public View leftEquipView;
    public TextMeshProUGUI infoPanel;
    public TabGroup tabGroup;
    public List<Tab> tabButtons;
    public int tempExp = 0;

    public FairyCard Card { get; set; }

    private CharData charData;
    private ExpTable expTable;

    [Header("Stat Info")]
    public View statInfoView;

    [Header("LvUp")]
    public View lvUpView;
    
    public TextMeshProUGUI lvText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI pDefenceText;
    public TextMeshProUGUI mDefenceText;
    public TextMeshProUGUI expText;
    public Image expSlider;
    //
    public Transform spiritStoneSpace;
    public bool LvUpLock { get; set; }

    private int sampleLv;
    private int sampleExp;
    private List<ItemButton> itemButtons = new List<ItemButton>();


    [Header("BreakLimit")]
    public TextMeshProUGUI originGrade;
    public TextMeshProUGUI nextGrade;
    public TextMeshProUGUI pieceProgress;
    public ItemIcon pieceIcon;

    [Header("EquipView")]
    public GameObject equipView;
    public Text equipName;
    public Image equipPieceImage;
    public Image pieceCountSlider;
    public Text pieceCountText;
    public Text equipAttackText;
    public Text equipHpText;
    public Text equipPDefenceText;
    public Text equipMDefenceText;
    public Button equipButton;
    public Button equipSupButton;

    [Header("Equip")]
    public GameObject equipGrowthView;
    public Text equipName2;
    public Image equipImage;
    public Image equipExpSlider;
    public Text equipExpText;
    public Text equipLvText;
    public Text equipAttackText2;
    public Text equipHpText2;
    public Text equipPDefenceText2;
    public Text equipMDefenceText2;
    public Transform enforceStoneSpace;

    public EquipSlot SelectedSlot { get; set; } = null;

    private int equipSampleLv;
    private int equipSampleExp;
    private List<ItemButton> enforceStoneButtons = new List<ItemButton>();


    public void Awake()
    {
        expTable = DataTableMgr.GetTable<ExpTable>();
    }

    public override void ActiveUI()
    {
        base.ActiveUI();
    }

    //선택한 카드로 UI 초기화
    public void Init(FairyCard card)
    {
        Card = card;
        charData = DataTableMgr.GetTable<CharacterTable>().dic[Card.ID];
        tabGroup?.OnTabSelected(tabButtons?[0]);
        SelectedSlot = null;
        tempExp = 0;
        SetLeftPanel();
        SetRightPanel();
    }
   
    public void SetLeftPanel()
    {
        if (tabGroup.selectedTab.Equals(tabButtons?[3]))
        {
            leftCardView.gameObject.SetActive(false);
            leftEquipView.gameObject.SetActive(true);
            leftEquipView.Init(Card);
        }
        else
        {
            leftEquipView.gameObject.SetActive(false);
            leftCardView.gameObject.SetActive(true);
            leftCardView.Init(Card);
        } 
    }

    public void SetRightPanel()
    {
        if (tabGroup.selectedTab == tabButtons[0])
        {
            statInfoView.Init(Card);
        }
        else
        if (tabGroup.selectedTab == tabButtons[1])
        {
            SetLvUpView();
        }
        else if (tabGroup.selectedTab == tabButtons[2])
        {
            SetBreakLimitView();
        }
        else if (tabGroup.selectedTab == tabButtons[3])
        {
            SetEquipView();
        }
    }

    //객체화 하기
    public void SetCardInfoView()
    {
        infoPanel.text = $"Name: {charData.CharName,-20}Grade: {Card.Grade,-10}{charData.CharProperty}/{charData.CharPosition}\n" +
            $"Lv {Card.Level,-20}{Card.Experience}/{expTable.dic[Card.Level].Exp}";
    }

    #region LvUP

    public void SetSample()
    {
        tempExp = 0;
        sampleLv = Card.Level;
        sampleExp = Card.Experience;
    }
    public void UpdateStatText(int level, int exp)
    {
        var stat = StatCalculator(charData, level);
        lvText.text = level.ToString();
        attackText.text = stat.attack.ToString();
        hpText.text = stat.hp.ToString();
        pDefenceText.text = stat.pDefence.ToString();
        mDefenceText.text = stat.mDefence.ToString();
        expText.text = $"{exp} / {expTable.dic[level].Exp}";
        expSlider.fillAmount = (float)exp / expTable.dic[level].Exp;
    }
    public void SetLvUpView()
    {
        ClearSpiritStoneScrollView();
        SetSample();
        UpdateStatText(Card.Level, Card.Experience);
        SetSpiritStoneScroolView();
    }

    public void SetSpiritStoneScroolView()
    {
        foreach (var dir in InvManager.spiritStoneInv.Inven)
        {
            if (dir.Value.Count == 0)
            {
                continue;
            }

            var go = Instantiate(itemButtonPrefab, spiritStoneSpace);
            var itemButton = go.GetComponent<ItemButton>();
            itemButtons.Add(itemButton);
            itemButton.Init(dir.Value);
            itemButton.OnClick += Simulation;
        }
    }

    public void ClearSpiritStoneScrollView()
    {
        for (int i = spiritStoneSpace.childCount - 1; i >= 0; i--)
        {
            var child = spiritStoneSpace.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public bool Simulation(Item item)
    {
        var table = DataTableMgr.GetTable<ExpTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        
        if (!CheckGrade(Card.Grade, sampleLv))
        {
            UIManager.Instance.modalWindow.OpenPopup("알림", "더 이상 정령석을 사용할 수 없습니다.");
            return false;
        }

        if (itemTable.dic.TryGetValue(item.ID, out var itemData))
        {
            sampleExp += itemData.value2;
            tempExp += itemData.value2;
        }
        
        if (sampleExp >= table.dic[sampleLv].Exp)
        {
            sampleExp -= table.dic[sampleLv].Exp;
            sampleLv++;
        }
        UpdateStatText(sampleLv, sampleExp);
        return true;
    }

    public void LvUp()
    {
        if (!CheckGrade(Card.Grade, Card.Level))
            return;

        if (sampleLv == Card.Level)
            return;

        var statsName = "Level\n공격력\n최대HP\n물리 방어력\n마법 방어력";
        UIManager.Instance.lvUpModal.OpenPopup("레벨업", "획득 경험치" + tempExp, sampleExp,
            DataTableMgr.GetTable<ExpTable>().dic[sampleLv].Exp, statsName, GetLvUpResult(Card.Level, sampleLv), "확인", null);

        Card.LevelUp(sampleLv, sampleExp);

        foreach (var button in itemButtons)
        {
            button.UseItem();
        };

        tempExp = 0;
        SaveLoadSystem.AutoSave();
        SetLvUpView();
        leftCardView.Init(Card);
    }

    public string GetLvUpResult(int beforeLv, int afterLv)
    {
        var beforeStat = StatCalculator(charData, beforeLv);
        var afterStat = StatCalculator(charData, afterLv);

        return $"{beforeLv} -> {afterLv}\n" +
            $"{beforeStat.attack} -> {afterStat.attack}\n" +
            $"{beforeStat.hp} -> {afterStat.hp}\n" +
            $"{beforeStat.pDefence} -> {afterStat.pDefence}\n" +
            $"{beforeStat.mDefence} -> {afterStat.pDefence}";
    }

    public bool CheckGrade(int grade, int level)
    {
        return grade * 10 + 10 > level;
    }

    public Stat StatCalculator(CharData data, int lv)
    {
        Stat result = new Stat();

        result.attack = data.CharAttack + data.CharAttackIncrease * lv;
        result.pDefence = data.CharPDefence + data.CharPDefenceIncrease * lv;
        result.mDefence = data.CharMDefence + data.CharMDefenceIncrease * lv;
        result.hp = data.CharMaxHP + data.CharHPIncrease * lv;

        return result;
    }
    #endregion

    #region Break Limit
    public void UpdataGradeText(int grade, TextMeshProUGUI text)
    {
        text.text = $"Grade: {grade}\nMaxLv: {grade * 10 + 10}";
    }

    public void SetPieceIcon()
    {
        if (InvManager.itemInv.Inven.TryGetValue(10003, out Item item))  //memoryPiece ID
        {
            var itemIcon = pieceIcon.GetComponent<ItemIcon>();
            itemIcon.Init(item);
        }
    }

    public void SetGradeProgress()
    {
        var table = DataTableMgr.GetTable<BreakLimitTable>();
        if (InvManager.itemInv.Inven.TryGetValue(10003, out Item item))
        {
            pieceProgress.text = $"{item.Count} / {table.dic[Card.Grade].CharPieceNeeded}";
        }
        else
        {
            pieceProgress.text = $"0 / {table.dic[Card.Grade].CharPieceNeeded}";
        }
    }

    public void SetBreakLimitView()
    {
        UpdataGradeText(Card.Grade, originGrade);
        int ng = Card.Grade > 5 ? Card.Grade : Card.Grade + 1;
        UpdataGradeText(ng, nextGrade);
        SetPieceIcon();
        SetGradeProgress();
    }

    public void BreakLimit()
    {
        var table = DataTableMgr.GetTable<BreakLimitTable>();
        if (InvManager.itemInv.Inven[10003].Count >= table.dic[Card.Grade].CharPieceNeeded)
        {
            InvManager.RemoveItem(InvManager.itemInv.Inven[10003], table.dic[Card.Grade].CharPieceNeeded);
            
            Card.GradeUp();

            UIManager.Instance.breakLimitModal.OpenPopup("한계 돌파", Card.Grade.ToString(), (Card.Grade + 1).ToString());

            SetLeftPanel();
            SetBreakLimitView();
        } 
    }

    #endregion

    #region Equip View

    public void SetEquipView()
    {
        if (SelectedSlot == null)
        {
            equipView.SetActive(true);
            equipGrowthView.SetActive(false);

            InitEquipInfoBox();
        }
        else
        {
            if (SelectedSlot.Equipment == null)
            {
                equipView.SetActive(true);
                equipGrowthView.SetActive(false);

                var charData = DataTableMgr.GetTable<CharacterTable>().dic[Card.ID];
                var position = charData.CharPosition;
                var rank = Card.Rank;

                var equipTable = DataTableMgr.GetTable<EquipTable>();
                var key = Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{rank}");

                SetEquipInfoBox(equipTable.dic[key]);
            }
            else
            {
                equipView.SetActive(false);
                equipGrowthView.SetActive(true);

                ClearEnforceStoneScrollView();
                SetEnforceStoneScroolView();
                SetEquipSample(SelectedSlot?.Equipment);
                SetEquipGrowthInfoBox(DataTableMgr.GetTable<EquipTable>().dic[SelectedSlot.Equipment.ID], equipSampleLv, equipSampleExp);
                leftEquipView.Init(Card);
            }
        }
    }

    public void InitEquipInfoBox()
    {

        equipName.text = "장비 이름";
        equipPieceImage.sprite = Resources.Load<Sprite>("StatStatus/Empty");
        pieceCountSlider.fillAmount = 0;
        pieceCountText.text = $"0 / 0";
        attackText.text = "0";
        hpText.text = "0";
        pDefenceText.text = "0";
        mDefenceText.text = "0";
    }

    public void SetEquipInfoBox(EquipData equipData)
    {
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();

        if (itemTable.dic.TryGetValue(equipData.EquipPiece, out ItemData itemData))
        {
            equipPieceImage.sprite = Resources.Load<Sprite>(itemData.icon);
        }
        equipName.text = stringTable.dic[equipData.EquipName].Value;
        if (InvManager.equipPieceInv.Inven.TryGetValue(equipData.EquipPiece, out EquipmentPiece piece))
        {
            pieceCountSlider.fillAmount = (float)piece.Count / equipData.EquipPieceNum;
            pieceCountText.text = $"{piece.Count} / {equipData.EquipPieceNum}";
        }
        else
        {
            pieceCountSlider.fillAmount = 0f / equipData.EquipPieceNum;
            pieceCountText.text = $"0 / {equipData.EquipPieceNum}";
        }
        equipAttackText.text = equipData.EquipAttack.ToString();
        equipHpText.text = equipData.EquipMaxHP.ToString();
        equipPDefenceText.text = equipData.EquipPDefence.ToString();
        equipMDefenceText.text = equipData.EquipMDefence.ToString();
    }

    public void SetEquip()
    {
        if (SelectedSlot == null)
            return;

        var position = charData.CharPosition;
        var key = Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{Card.Rank}");
        var newEquipment = new Equipment(key);

        SelectedSlot.CreateAndSetEquipment(newEquipment);
        Card.SetEquip(SelectedSlot.slotNumber, newEquipment);
    }

    public void RankUp()
    {
        if (Card.equipSocket.Count == 6)
            return;

        foreach (var value in Card.equipSocket.Values)
        {
            if (value == null)
                return;
        }
        Card.RankUp();
        SetLeftPanel();
        SetEquipView();
    }

    public void OpenItemDropStageInfoPopup()
    {
        if (SelectedSlot == null)
            return;

        var position = charData.CharPosition;
        var key = Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{Card.Rank}");
        var equipTable = DataTableMgr.GetTable<EquipTable>();

        UIManager.Instance.stageInfoModal.OpenPopup("드랍 스테이지 정보", equipTable.dic[key].EquipPiece);
    }

    #endregion

    #region EquipGrowthTap

    public void SetEquipGrowthInfoBox(EquipData equipData, int sampleLv, int sampleExp)
    {
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();
        var expTable = DataTableMgr.GetTable<EquipExpTable>();

        equipLvText.text = $"{sampleLv}";
        if (itemTable.dic.TryGetValue(equipData.EquipPiece, out ItemData itemData))
        {
            //장비 이미지로 변경 (현재 장비 조각 이미지)
            equipImage.sprite = Resources.Load<Sprite>(itemData.icon);
        }
        equipName2.text = stringTable.dic[equipData.EquipName].Value;
        equipExpSlider.fillAmount = (float)sampleExp / expTable.dic[sampleLv].Exp;
        equipExpText.text = $"{sampleExp} / {expTable.dic[sampleLv].Exp}";

        var stat = StatCalculator(equipData, sampleLv);

        equipAttackText2.text = stat.attack.ToString();
        equipHpText2.text = stat.hp.ToString();
        equipPDefenceText2.text = stat.pDefence.ToString();
        equipMDefenceText2.text = stat.mDefence.ToString();
    }

    public Stat StatCalculator(EquipData data, int lv)
    {
        Stat result = new Stat();

        result.attack = data.EquipAttack + data.EquipAttackIncrease * lv;
        result.pDefence = data.EquipPDefence + data.EquipPDefenceIncrease * lv;
        result.mDefence = data.EquipMDefence + data.EquipMDefenceIncrease * lv;
        result.hp = data.EquipMaxHP + data.EquipHPIncrease * lv;

        return result;
    }

    public void SetEnforceStoneScroolView()
    {
        Set(10004);
        Set(10005);
        Set(10006);
        
        void Set(int id)
        {
            if (InvManager.itemInv.Inven.TryGetValue(id, out Item enforceStone))
            {
                if (enforceStone.Count > 0)
                {
                    var go = Instantiate(itemButtonPrefab, enforceStoneSpace);
                    var itemButton = go.GetComponent<ItemButton>();
                    enforceStoneButtons.Add(itemButton);
                    itemButton.Init(enforceStone);
                    itemButton.OnClick += EquipSimulation;
                }
            }
        }
    }

    public void ClearEnforceStoneScrollView()
    {
        for (int i = enforceStoneSpace.childCount - 1; i >= 0; i--)
        {
            var child = enforceStoneSpace.GetChild(i);
            Destroy(child.gameObject);
        }
        enforceStoneButtons.Clear();
    }

    public bool EquipSimulation(Item item)
    {
        if (SelectedSlot == null || SelectedSlot.Equipment == null)
            return false;

        if (equipSampleLv >= 30)
            return false;

        var expTable = DataTableMgr.GetTable<EquipExpTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        if (itemTable.dic.TryGetValue(item.ID, out ItemData itemData))
        {
            equipSampleExp += itemData.value2;
            tempExp += itemData.value2;
        }

        if (equipSampleExp >= expTable.dic[equipSampleLv].Exp)
        {
            equipSampleExp -= expTable.dic[equipSampleLv].Exp;
            equipSampleLv++;
        }
        SetEquipGrowthInfoBox(DataTableMgr.GetTable<EquipTable>().dic[SelectedSlot.Equipment.ID], equipSampleLv, equipSampleExp);

        return true;
    }

    public void SetEquipSample(Equipment equipment)
    {
        if (equipment == null) 
            return;

        tempExp = 0;
        equipSampleLv = equipment.Level;
        equipSampleExp = equipment.Exp;
    }

    public void EquipLvUp()
    {
        if (SelectedSlot == null || SelectedSlot.Equipment == null)
            return;

        if (equipSampleLv >= 30)
            return;

        var statsName = "Level\n공격력\n최대HP\n물리 방어력\n마법 방어력";
        UIManager.Instance.lvUpModal.OpenPopup("레벨업", "획득 경험치" + tempExp, equipSampleExp,
            DataTableMgr.GetTable<EquipExpTable>().dic[equipSampleLv].Exp, statsName, GetLvUpResult(SelectedSlot.Equipment.Level, equipSampleLv), "확인", null);

        SelectedSlot.Equipment.LevelUp(equipSampleLv, equipSampleExp);

        foreach (var button in enforceStoneButtons)
        {
            button.UseItem();
        }
        SaveLoadSystem.AutoSave();
        leftEquipView.Init(Card);
    }

    #endregion
}
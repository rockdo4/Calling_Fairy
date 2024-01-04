using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyGrowthUI : UI
{
    [Header("Common")]
    public GameObject itemButtonPrefab;
    public GameObject equipItemButtonPrefab;
    public GameObject itemIconPrefab;
    public View leftCardView;
    public View leftEquipView;
    public TabGroup tabGroup;
    public List<Tab> tabButtons;
    private int tempExp = 0;

    public FairyCard Card { get; set; }

    private CharData charData;
    private ExpTable expTable;

    [Header("Stat Info")]
    public View statInfoView;
    public ScrollRect statInfoScrollView;

    [Header("LvUp")]
    public View lvUpView;
    public TextMeshProUGUI lvText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI pDefenceText;
    public TextMeshProUGUI mDefenceText;
    public TextMeshProUGUI expText;
    public Image expSlider;
    public Transform spiritStoneSpace;
    public Button lvUpButton;
    public ParticleSystem lvUpParticle;
    public bool LvUpLock { get; set; }

    private int sampleLv;
    private int sampleExp;
    private int isBonusCount;
    private List<ItemButton> itemButtons = new List<ItemButton>();


    [Header("BreakLimit")]
    public Image currentGrade;
    public Image nextGrade;
    public TextMeshProUGUI currentLimitLevel;
    public TextMeshProUGUI nextLimitLevel;
    public GameObject lastGradeUI;
    public ItemIcon memoriePieceIcon;
    public Gauge memoriePieceGauge;
    public Button limitBreakButton;
    public ParticleSystem breakLimitParticle;

    [Header("EquipView")]
    public GameObject equipView;
    public TextMeshProUGUI equipName;
    public Image equipPieceImage;
    public Image pieceCountSlider;
    public Text pieceCountText;
    public TextMeshProUGUI equipAttackText;
    public TextMeshProUGUI equipHpText;
    public TextMeshProUGUI equipPDefenceText;
    public TextMeshProUGUI equipMDefenceText;


    [Header("Equip")]
    public GameObject equipGrowthView;
    public TextMeshProUGUI equipName2;
    public Image equipImage;
    public Image equipExpSlider;
    public Text equipExpText;
    public TextMeshProUGUI equipLvText;
    public TextMeshProUGUI equipAttackText2;
    public TextMeshProUGUI equipHpText2;
    public TextMeshProUGUI equipPDefenceText2;
    public TextMeshProUGUI equipMDefenceText2;
    public Transform enforceStoneSpace;
    public Button equipLvUpButton;
    public List<ParticleSystem> rankUpParticles;

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
        tabGroup.OnTabSelected(tabGroup.tabButtons[0]);
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
            SetStatView();
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

    public void SetStatView()
    {
        statInfoView.Init(Card);
        statInfoScrollView.verticalNormalizedPosition = 1f;
    }


    #region LvUP

    public void SetSample()
    {
        tempExp = 0;
        isBonusCount = 0;
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

    // 선택한 Item을 Stack으로 가지고 있게 해서 계산하는 방식으로 수정 필요.
    public bool Simulation(Item item, bool isPositive)
    {
        var table = DataTableMgr.GetTable<ExpTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        if (itemTable.dic.TryGetValue(item.ID, out var itemData))
        {
            if (isPositive) // 증가
            {
                // 시작 레벨이 한계 레벨을 넘어가면 아이템 사용 불가
                if (!CheckGrade(Card.Grade, sampleLv))
                {
                    UIManager.Instance.modalWindow.OpenPopup(GameManager.stringTable[407].Value, GameManager.stringTable[328].Value);
                    return false;
                }

                // 보너스 경험치 적용 여부 확인
                if (charData.CharProperty == itemData.value1)
                {
                    sampleExp += (int)(itemData.value2 * 1.5f);
                    tempExp += (int)(itemData.value2 * 1.5f);
                    isBonusCount++;
                }
                else
                {
                    sampleExp += itemData.value2;
                    tempExp += itemData.value2;
                }

                // 경험치가 최대 경험치를 넘어가면 레벨업
                while (sampleExp >= table.dic[sampleLv].Exp)
                {
                    sampleExp -= table.dic[sampleLv].Exp;
                    sampleLv++;
                }

                // 사용 아이템으로 인해 최대 레벨을 돌파할 경우 아이템 사용 불가
                if (!CheckGrade(Card.Grade, sampleLv))
                {
                    // 더해준 경험치 빼주기
                    if (charData.CharProperty == itemData.value1)
                    {
                        sampleExp -= (int)(itemData.value2 * 1.5f);
                        tempExp -= (int)(itemData.value2 * 1.5f);
                        isBonusCount--;
                    }
                    else
                    {
                        sampleExp -= itemData.value2;
                        tempExp -= itemData.value2;
                    }

                    while (sampleExp < 0)
                    {
                        sampleLv--;
                        sampleExp += table.dic[sampleLv].Exp;
                    }

                    UIManager.Instance.modalWindow.OpenPopup(GameManager.stringTable[407].Value, GameManager.stringTable[328].Value);
                    return false;
                }

            }
            else    // 감소
            {
                if (charData.CharProperty == itemData.value1)
                {
                    sampleExp -= (int)(itemData.value2 * 1.5f);
                    tempExp -= (int)(itemData.value2 * 1.5f);
                    isBonusCount--;
                }
                else
                {
                    sampleExp -= itemData.value2;
                    tempExp -= itemData.value2;
                }

                while (sampleExp < 0)
                {
                    sampleLv--;
                    sampleExp += table.dic[sampleLv].Exp;
                }
            }

            lvUpButton.interactable = sampleLv != Card.Level || sampleExp != Card.Experience;
        }

        UpdateStatText(sampleLv, sampleExp);
        return true;
    }

    public void TryShowLevelUpEffect()
    {
        if (!CheckGrade(Card.Grade, Card.Level))
            return;

        lvUpParticle.Play();    
    }

    public void LevelUp()
    {
        Card.LevelUp(sampleLv, sampleExp);

        foreach (var button in itemButtons)
        {
            button.UseItem();
        };

        tempExp = 0;
        lvUpButton.interactable = false;
        SaveLoadSystem.AutoSave();
        SetLvUpView();
        leftCardView.Init(Card);
    }

    public void OpenLevleUpPopup()
    {
        if (lvUpParticle.particleCount <= 1)
        {
            var stringTable = DataTableMgr.GetTable<StringTable>();
            var statsName = $"{stringTable.dic[305].Value}\n{stringTable.dic[306].Value}\n{stringTable.dic[307].Value}\n{stringTable.dic[308].Value}\n{stringTable.dic[313].Value}";

            UIManager.Instance.lvUpModal.OpenPopup(stringTable.dic[332].Value, stringTable.dic[330].Value + tempExp, sampleExp,
                DataTableMgr.GetTable<ExpTable>().dic[sampleLv].Exp, statsName, GetLvUpResult(Card.Level, sampleLv), stringTable.dic[1].Value, null, isBonusCount > 0);

            LevelUp();
        }
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
        return grade * 10 + 10 >= level;
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

    public void SetGradeInfoBox(int currentGrade)
    {
        var charTable = DataTableMgr.GetTable<ExpTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();
        string maxLevelString = stringTable.dic[320].Value;

        if (currentGrade < 5)
        {
            SetActiveGO(true);
            limitBreakButton.interactable = true;

            this.currentGrade.sprite = Resources.Load<Sprite>($"UIElement/{currentGrade}star");
            var sizeDelta = this.currentGrade.rectTransform.sizeDelta;
            this.currentGrade.rectTransform.sizeDelta = new Vector2(sizeDelta.y * currentGrade, sizeDelta.y);
            currentLimitLevel.text = $"{maxLevelString} {currentGrade * 10 + 10}";

            nextGrade.sprite = Resources.Load<Sprite>($"UIElement/{currentGrade + 1}star");
            nextGrade.rectTransform.sizeDelta = new Vector2(sizeDelta.y * (currentGrade + 1), sizeDelta.y);
            nextLimitLevel.text = $"{maxLevelString} {(currentGrade + 1) * 10 + 10}";
        }
        else
        {
            SetActiveGO(false);
            limitBreakButton.interactable = false;
        }

        void SetActiveGO(bool value)
        {
            lastGradeUI.SetActive(!value);

            this.currentGrade.gameObject.SetActive(value);
            currentLimitLevel.gameObject.SetActive(value);
            nextGrade.gameObject.SetActive(value);
            nextLimitLevel.gameObject.SetActive(value);
        }
    }

    
    public void SetMemoriePieceBox(int currentGrade)
    {
        if (!InvManager.itemInv.Inven.ContainsKey(10003))
        {
            InvManager.AddItem(new Item(10003, 0));
        }
        memoriePieceIcon.Init(InvManager.itemInv.Inven[10003]);
        memoriePieceGauge.SetGauge(InvManager.itemInv.Inven[10003].Count, DataTableMgr.GetTable<BreakLimitTable>().dic[currentGrade].CharPieceNeeded);
    }

    public void SetBreakLimitView()
    {
        SetGradeInfoBox(Card.Grade);
        SetMemoriePieceBox(Card.Grade);
    }

    public void TryShowBreakLimitEffect()
    {
        var table = DataTableMgr.GetTable<BreakLimitTable>();

        if (InvManager.itemInv.Inven[10003].Count >= table.dic[Card.Grade].CharPieceNeeded)
        {
            breakLimitParticle.Play(); 
        } 
    }

    public void TryBreakLimit()
    {
        if (breakLimitParticle.particleCount <= 1)
        {
            var table = DataTableMgr.GetTable<BreakLimitTable>();
            var stringTable = DataTableMgr.GetTable<StringTable>();

            InvManager.RemoveItem(InvManager.itemInv.Inven[10003], table.dic[Card.Grade].CharPieceNeeded);

            Card.GradeUp();

            UIManager.Instance.breakLimitModal.OpenPopup(stringTable.dic[303].Value, (Card.Grade - 1).ToString(), (Card.Grade).ToString());

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

                if (equipTable.dic.TryGetValue(key, out EquipData equipData))
                {
                    SetEquipInfoBox(equipData);
                }
                else
                {
                    Debug.LogError("테이블에 장비 데이터 없음");
                }
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

    // 장비 장착
    public void EquipItem()
    {
        if (SelectedSlot == null)
            return;

        var position = charData.CharPosition;
        var key = Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{Card.Rank}");
        var newEquipment = new Equipment(key);

        SelectedSlot.CreateAndSetEquipment(newEquipment);
        Card.SetEquip(SelectedSlot.slotNumber, newEquipment);
        SetEquipView();
    }

    public void TryShowRankUpEffect()
    {
        if (Card.equipSocket.Count != 6)
            return;

        foreach (var particle in rankUpParticles)
        {
            particle.Play();
        }
    }

    public void RankUp()
    {
        foreach (var value in Card.equipSocket.Values)
        {
            if (value == null)
                return;
        }

        Card.RankUp();
        SelectedSlot = null;
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

    public void OpenEquipDetailInfoPopup()
    {
        if (SelectedSlot == null) 
            return;

        var position = charData.CharPosition;
        var key = Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{Card.Rank}");

        if (SelectedSlot.Equipment == null)
        {
            UIManager.Instance.detailStatModal.OpenPopup("장비 상세 정보", new Equipment(key));
        }
        else
        {
            UIManager.Instance.detailStatModal.OpenPopup("장비 상세 정보", SelectedSlot.Equipment);
        }
    }

    #endregion

    #region EquipGrowthTap

    public void SetEquipGrowthInfoBox(EquipData equipData, int sampleLv, int sampleExp)
    {
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();
        var expTable = DataTableMgr.GetTable<EquipExpTable>();

        equipLvText.text = $"{sampleLv}";
        equipImage.sprite = Resources.Load<Sprite>(equipData.EquipIcon);
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
                    var go = Instantiate(equipItemButtonPrefab, enforceStoneSpace);
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

    public bool EquipSimulation(Item item, bool isPositive)
    {
        if (SelectedSlot == null || SelectedSlot.Equipment == null)
            return false;

        if (equipSampleLv >= 30)
            return false;

        var expTable = DataTableMgr.GetTable<EquipExpTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        if (itemTable.dic.TryGetValue(item.ID, out ItemData itemData))
        {
            if (isPositive)
            {
                equipSampleExp += itemData.value2;
                tempExp += itemData.value2;

                while (equipSampleExp >= expTable.dic[equipSampleLv].Exp)
                {
                    equipSampleExp -= expTable.dic[equipSampleLv].Exp;
                    equipSampleLv++;
                    // 정령 경험치 테이블은 최종 레벨에도 샘플 경험치가 있지만 장비 경험치 테이블에는 없음
                }

                if (equipSampleLv > 30)
                {
                    equipSampleExp -= itemData.value2;
                    tempExp -= itemData.value2;

                    while (equipSampleExp < 0)
                    {
                        equipSampleLv--;
                        equipSampleExp += expTable.dic[equipSampleLv].Exp;
                    }
                    return false;
                }
            }
            else // 감소
            {
                equipSampleExp -= itemData.value2;
                tempExp -= itemData.value2;

                while(equipSampleExp < 0)
                {
                    equipSampleLv--;
                    equipSampleExp += expTable.dic[equipSampleLv].Exp;
                }
            }
        }
        equipLvUpButton.interactable = equipSampleLv != SelectedSlot.Equipment.Level || equipSampleExp != SelectedSlot.Equipment.Exp;
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

        if (equipSampleLv > 30)
            return;

        var strigTable = DataTableMgr.GetTable<StringTable>();

        var statsName = $"{strigTable.dic[305].Value}\n{strigTable.dic[306].Value}\n{strigTable.dic[307].Value}\n{strigTable.dic[308].Value}\n{strigTable.dic[313].Value}";
        UIManager.Instance.lvUpModal.OpenPopup($"{strigTable.dic[302].Value}", $"{strigTable.dic[330].Value} " + tempExp, equipSampleExp,
            DataTableMgr.GetTable<EquipExpTable>().dic[equipSampleLv].Exp, statsName, GetLvUpResult(SelectedSlot.Equipment.Level, equipSampleLv), strigTable.dic[1].Value, null, false);

        SelectedSlot.Equipment.LevelUp(equipSampleLv, equipSampleExp);

        foreach (var button in enforceStoneButtons)
        {
            button.UseItem();
        }

        equipLvUpButton.interactable = false;
        SaveLoadSystem.AutoSave();
        leftEquipView.Init(Card);
    }

    #endregion
}
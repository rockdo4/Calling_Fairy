using Coffee.UIExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyGrowthUI : UI
{
    [Header("Data & Models")]
    public FairyCard Card { get; private set; }
    public EquipSlot SelectedSlot { get; set; } = null;
    private CharData charData;
    private ExpTable expTable;

    [Header("Views & Panels")]
    public UIView leftCardView;
    public UIView leftEquipView;
    public UIView statInfoView;
    public UIView breakLimitView;
    public UIView equipCreateView; // 장비 제작 정보 패널
    public GameObject equipGrowthView; // 장비 성장 정보 패널 (시뮬레이션 로직으로 인해 UIView로 미전환)
    public TabGroup tabGroup;
    public List<Tab> tabButtons;
    public ScrollRect statInfoScrollView;

    [Header("Prefabs")]
    public GameObject itemButtonPrefab;
    public GameObject equipItemButtonPrefab;
    public GameObject itemIconPrefab;

    // --- 시뮬레이션 및 복잡한 로직으로 인해 유지되는 필드들 ---
    #region Simulation Fields
    private int tempExp = 0;

    [Header("LvUp (Simulation)")]
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

    [Header("BreakLimit (Simulation/Button)")]
    public Button limitBreakButton;
    public ParticleSystem breakLimitParticle;

    [Header("EquipView (Simulation)")]
    public TextMeshProUGUI equipName;
    public Image equipPieceImage;
    public Image pieceCountSlider;
    public Text pieceCountText;
    public TextMeshProUGUI equipAttackText;
    public TextMeshProUGUI equipHpText;
    public TextMeshProUGUI equipPDefenceText;
    public TextMeshProUGUI equipMDefenceText;

    [Header("EquipGrowth (Simulation)")]
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
    public GameObject rankUpAttractors;
    public List<ParticleSystem> rankUpParticles;
    public ParticleSystem equipExpParticle;
    public ParticleSystem fairyAttractorParticle2;
    private int equipParticleCount = 0;
    private int equipSampleLv;
    private int equipSampleExp;
    private List<ItemButton> enforceStoneButtons = new List<ItemButton>();
    #endregion

    public void Awake()
    {
        expTable = DataTableMgr.GetTable<ExpTable>();
    }

    public override void ActiveUI()
    {
        base.ActiveUI();
        tabGroup.OnTabSelected(tabGroup.tabButtons[0]);
    }

    public void Init(FairyCard card)
    {
        Card = card;
        charData = DataTableMgr.GetTable<CharacterTable>().dic[Card.ID];
        SelectedSlot = null;
        tempExp = 0;
        
        // 탭 그룹을 첫번째로 초기화하고, 이에 맞는 패널을 설정
        tabGroup?.OnTabSelected(tabButtons?[0]);
        SetLeftPanel();
        SetRightPanel();
    }
   
    public void SetLeftPanel()
    {
        bool isEquipTab = tabGroup.selectedTab.Equals(tabButtons?[3]);
        leftCardView.gameObject.SetActive(!isEquipTab);
        leftEquipView.gameObject.SetActive(isEquipTab);

        if (isEquipTab)
        {
            leftEquipView.Bind(Card);
        }
        else
        {
            leftCardView.Bind(Card);
        }
    }

    public void SetRightPanel()
    {
        if (tabGroup.selectedTab == tabButtons[0]) SetStatView();
        else if (tabGroup.selectedTab == tabButtons[1]) SetLvUpView();
        else if (tabGroup.selectedTab == tabButtons[2]) SetBreakLimitView();
        else if (tabGroup.selectedTab == tabButtons[3]) SetEquipView();
    }

    public void SetStatView()
    {
        statInfoView.Bind(Card);
        statInfoScrollView.verticalNormalizedPosition = 1f;
    }
    
    public void SetBreakLimitView()
    {
        breakLimitView.Bind(Card);
        // 버튼 인터랙션과 같은 로직은 유지
        limitBreakButton.interactable = Card.Grade < 5;
    }

    public void SetEquipView()
    {
        bool isEquipSelected = SelectedSlot != null;
        bool hasEquip = isEquipSelected && SelectedSlot.Equipment != null;

        equipCreateView.gameObject.SetActive(isEquipSelected && !hasEquip);
        equipGrowthView.SetActive(isEquipSelected && hasEquip);
        rankUpAttractors.SetActive(!isEquipSelected); // 랭크업 파티클은 장비 미선택 시에만

        if (isEquipSelected)
        {
            if (hasEquip)
            {
                // 장비 성장 뷰 초기화 (시뮬레이션 로직 유지)
                ClearEnforceStoneScrollView();
                SetEnforceStoneScroolView();
                SetEquipSample(SelectedSlot.Equipment);
                SetEquipGrowthInfoBox(DataTableMgr.GetTable<EquipTable>().dic[SelectedSlot.Equipment.ID], equipSampleLv, equipSampleExp);
            }
            else
            {
                // 장비 제작 뷰 바인딩 (미리보기 데이터 표시 로직 유지)
                var key = Convert.ToInt32($"30{charData.CharPosition}{SelectedSlot.slotNumber}0{Card.Rank}");
                var equipData = DataTableMgr.GetTable<EquipTable>().dic[key];
                SetEquipInfoBox(equipData); // 이 부분은 미리보기라 DataBinder로 처리하기 까다로워 유지
            }
        }
        else
        {
            // 아무것도 선택하지 않았을 때의 초기화 로직
            InitEquipInfoBox();
        }
    }

    #region Retained Simulation and Complex Logic
    // 이 아래의 모든 메소드들은 UI 요소의 값을 실시간으로 계산하고 미리 보여주는 등
    // 단순 데이터 바인딩으로 처리하기 어려운 복잡한 로직을 포함하고 있으므로
    // 이번 리팩토링 단계에서는 변경하지 않고 그대로 유지합니다.

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

        foreach (var value in Card.equipSocket.Values)
        {
            if (value == null)
                return;
        }

        rankUpAttractors.SetActive(true);

        foreach (var particle in rankUpParticles)
        {
            UIManager.Instance.blockPanel.SetActive(true);
            particle.Play();
        }
    }

    public void TryRankUp()
    {
        equipParticleCount++;

        if (equipParticleCount < 6)
            return;
        
        StartCoroutine(WaitForParticleCompletionThenRankUp(fairyAttractorParticle2));
    }

    IEnumerator WaitForParticleCompletionThenRankUp(ParticleSystem particle)
    {   

        yield return new WaitForSeconds(particle.main.duration);

        equipParticleCount = 0;
        Card.RankUp();
        SelectedSlot = null;
        SetLeftPanel();
        SetEquipView();
        rankUpAttractors.SetActive(false);
        UIManager.Instance.blockPanel.SetActive(false);
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
    
    public void InitEquipInfoBox()
    {
        equipName.text = "장비 이름";
        equipPieceImage.sprite = Resources.Load<Sprite>("StatStatus/Empty");
        pieceCountSlider.fillAmount = 0;
        pieceCountText.text = $"0 / 0";
        equipAttackText.text = "0";
        equipHpText.text = "0";
        equipPDefenceText.text = "0";
        equipMDefenceText.text = "0";
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

    public void SetLvUpView()
    {
        ClearSpiritStoneScrollView();
        SetSample();
        UpdateStatText(Card.Level, Card.Experience);
        SetSpiritStoneScroolView();
    }

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

    public bool Simulation(Item item, bool isPositive)
    {
        var table = DataTableMgr.GetTable<ExpTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        if (itemTable.dic.TryGetValue(item.ID, out var itemData))
        {
            if (isPositive) // 증가
            {
                if (!CheckGrade(Card.Grade, sampleLv))
                {
                    UIManager.Instance.modalWindow.OpenPopup(GameManager.stringTable[407].Value, GameManager.stringTable[328].Value);
                    return false;
                }

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

                while (sampleExp >= table.dic[sampleLv].Exp)
                {
                    sampleExp -= table.dic[sampleLv].Exp;
                    sampleLv++;
                }

                if (!CheckGrade(Card.Grade, sampleLv))
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

        UIManager.Instance.blockPanel.SetActive(true);
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
        // SaveLoadSystem.AutoSave(); // 이벤트 시스템으로 대체
        SetLvUpView();
        leftCardView.Bind(Card);
    }

    public void OpenLevleUpPopup()
    {
        if (lvUpParticle.particleCount <= 1)
        {
            UIManager.Instance.blockPanel.SetActive(false);

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
    
    public void TryShowBreakLimitEffect()
    {
        var table = DataTableMgr.GetTable<BreakLimitTable>();

        if (InvManager.itemInv.Inven[10003].Count >= table.dic[Card.Grade].CharPieceNeeded)
        {
            UIManager.Instance.blockPanel.SetActive(true);
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

            UIManager.Instance.blockPanel.SetActive(false);
        }
    }

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

    public void TryShowEquipLvUpEffect()
    {
        if (SelectedSlot == null || SelectedSlot.Equipment == null)
            return;

        if (equipSampleLv > 30)
            return;

        UIManager.Instance.blockPanel.SetActive(true);
        equipExpParticle.Play();
    }   

    public void EquipLvUp()
    {
        if (equipExpParticle.particleCount <= 1)
        {
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
            // SaveLoadSystem.AutoSave(); // 이벤트 시스템으로 대체
            leftEquipView.Bind(Card);

            UIManager.Instance.blockPanel.SetActive(false);
        }
    }
    #endregion
}

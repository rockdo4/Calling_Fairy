using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Skip : MonoBehaviour
{
    [SerializeField]
    private Button skipNumUp;
    [SerializeField]
    private Button skipNumDown;
    [SerializeField]
    private TextMeshProUGUI skipNumText;
    [SerializeField]
    private Button skipButton;
    [SerializeField]
    private Button skipInitButton;
    [SerializeField]
    private TextMeshProUGUI skipStaminaText;
    [SerializeField]
    private int skipInfoStringId;
    [SerializeField]
    private int skipTicketId;

    public static int skipNum = 0;
    private int skipStamina = 0;
    private int skipTicketCount = 0;


    private void Awake()
    {
        skipNumUp.onClick.AddListener(() => 
        { 
            skipNum++; 
            if(skipNum * skipStamina > Player.Instance.Stamina || skipNum > skipTicketCount)
            {
                skipNum--;
            }
            UpdateText(); 
        });
        skipNumDown.onClick.AddListener(() => 
        { 
            skipNum--;
            if (skipNum < 0)
            {
                skipNum = 0;
            }
            UpdateText();
        });
    }

    private void OnEnable()
    {
        var stageId = GameManager.Instance.StageId;
        var stageData = DataTableMgr.GetTable<StageTable>().dic[stageId];
        skipStamina = stageData.useStamina;
        if (InvManager.itemInv.Inven.ContainsKey(skipTicketId))
        {
            skipTicketCount = InvManager.itemInv.Inven[skipTicketId].Count;
        }
        Init();
    }

    private void UpdateText()
    {
        skipNumText.text = $"{skipNum}{GameManager.stringTable[skipInfoStringId].Value}";
        skipStaminaText.text = $"{skipNum * skipStamina} / {Player.Instance.Stamina}";
        if(skipNum <= 0)
        {
            skipInitButton.interactable = false;
        }
        else
        {
            skipInitButton.interactable = true;
        }
    }

    public void SkipStage()
    {
        var stageData = DataTableMgr.GetTable<StageTable>().dic[GameManager.Instance.StageId];
        var monseterDropTable = DataTableMgr.GetTable<MonsterDropTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        var waveTable = DataTableMgr.GetTable<WaveTable>();
        var monstertable = DataTableMgr.GetTable<MonsterTable>();
        var dropRate = stageData.stageDorpPercent;
        var monsterDrops = new List<int>();
        InvManager.ingameInv.Inven.Clear();
        int counter = 0;

        //몬스터 찾기
        foreach(var wave in new[] { stageData.wave1, stageData.wave2, stageData.wave3, stageData.wave4, stageData.wave5, stageData.wave6 })
        {
            if (wave == 0)
                continue;
            foreach(var monster in waveTable.dic[wave].Monsters)
            {
                if (monster == 0)
                    continue;
                monsterDrops.Add(monstertable.dic[monster].dropItem);
            }
            
        }

            //드랍 보기
        do 
        { 
            foreach (var monsterDrop in monsterDrops)
            {
                if (Random.Range(0, 100) > dropRate)
                    continue;
                int sum = 0;
                var drops = monseterDropTable.dic[monsterDrop].Drops;
                var randVal = Random.Range(0, 100);
                foreach (var item in drops)
                {
                    if (item.Item2 == 0)
                        continue;
                    sum += item.Item2;
                    if (randVal < sum)
                    {
                        var itemData = itemTable.dic[item.Item1];
                        switch (itemData.sort)
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
                        //스킵인벤처럼쓰면됨
                        InvManager.ingameInv.AddItem(new Item(itemData.ID));
                        break;
                    }
                }
            }
            counter++;
        } while (counter <= skipNum);
        Player.Instance.GainGold(stageData.gainGold * skipNum);
        Player.Instance.GetExperience(stageData.gainPlayerExp * skipNum);
        InvManager.AddItem(new SpiritStone(stageData.gainExpStone, stageData.gainExpStoneValue * skipNum));
        InvManager.ingameInv.AddItem(new Item(stageData.gainExpStone, stageData.gainExpStoneValue * skipNum));
        InvManager.RemoveItem(new Item(skipTicketId), skipNum);
        Player.Instance.UseStamina(stageData.useStamina * skipNum);
        
        UpdateText();
     }

    private void Init()
    {
        skipNum = 0;       
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            InvManager.AddItem(new Item(skipTicketId));
        }
    }
}

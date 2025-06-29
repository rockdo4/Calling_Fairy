using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDataVC = SaveDataV8;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    private static object _lock = new object();
    private static bool applicationIsQuitting = false;
    public static Dictionary<int, StringData> stringTable = new();

    private static bool isInitialized = false;

    public Mode gameMode;

    public FairyCard[] StoryFairySquad { get; private set; } = new FairyCard[3];
    public int StorySquadLeaderIndex { get; set; } = -1;
    public FairyCard[] DailyFairySquad { get; private set; } = new FairyCard[3];
    public int DailySquadLeaderIndex { get; set; } = -1;
    public int[] SelectedValue { get; set; } = new int[3];
    public int BGSNum { get; set; } = 0;
    public int StageId;
    public int MyBestStageID { get; private set; } = 9000;
    public StringTable.Language language;
    public float[] Volume { get; set; } = new float[3] { 100, 100, 100 };
    public static GameManager Instance
    {
        get
        {
            // «¢«×«ê«±?«·«ç«óª¬ðûÖõª·ªÆª¤ªëíÞùê¡¢nullªòÚ÷ª¹
            // ¾îÇÃ¸®ÄÉÀÌ¼ÇÀÌ Á¾·áµÈ °æ¿ì nullÀ» ¹ÝÈ¯
            if (applicationIsQuitting)
            {
                Debug.LogWarning(string.Format(@"[Singleton] Instance '{0}' already 
                    destroyed on application quit. Won't create again. returning null.",
                    typeof(GameManager)));
                return null;
            }

            // ÞªÔÒÑ¢??
            // ºñµ¿±â ´ëÀÀ
            lock (_lock)
            {
                // ªÞªÀ«¤«ó«¹«¿«ó«¹ª¬ðíî¤ª·ªÊª¤íÞùê¡¢«·?«óª«ªé÷®ª¹
                // ¾ÆÁ÷ ÀÎ½ºÅÏ½º°¡ Á¸ÀçÇÏÁö ¾Ê´Â °æ¿ì, ¾À¿¡¼­ Ã£À½
                if (_instance == null)
                {
                    // GameManagerª¬2ËÁì¤ß¾ªÎíÞùê¡¢ìéªÄªòð¶ª¤ªÆîïÝ»Þûð¶ª¹ªë
                    // GameManager°¡ 2°³ ÀÌ»óÀÎ °æ¿ì, ÇÏ³ª¸¦ Á¦¿ÜÇÏ°í ¸ðµÎ »èÁ¦
                    var gameManagers = FindObjectsOfType(typeof(GameManager));
                    if (gameManagers.Length >= 1)
                    {
                        _instance = gameManagers[0] as GameManager;

                        for (int i = gameManagers.Length - 1; i > 0; i--)
                        {
                            Destroy(gameManagers[i]);
                            Debug.LogError(string.Format(@"[Singleton] Multiple instances 
                                of {0} detected. Destroying duplicate.",
                                typeof(GameManager)));
                        }
                        return _instance;
                    }

                    // «·?«óªËGameManagerª¬ðíî¤ª·ªÊª¤íÞùê¡¢ãæª·ª¤GameObjectªòíÂà÷
                    // ¾À¿¡ GameManager°¡ Á¸ÀçÇÏÁö ¾Ê´Â °æ¿ì, »õ·Î¿î GameObject¸¦ »ý¼º
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<GameManager>();
                        singleton.name = "(singleton) " + typeof(GameManager).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log(string.Format(@"[Singleton] An instance of {0} is 
                            needed in the scene, so '{1}' was created with DontDestroyOnLoad",
                            typeof(GameManager), singleton));
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                    }
                    return _instance;
                }
                return _instance;
            }
        }
    }

    public static void Init()
    {
        if (isInitialized)
            return;

        Instance.LoadData();
    }

    private void Awake()
    {
        stringTable = DataTableMgr.GetTable<StringTable>().dic;
    }

    public void OnDestroy()
    {
        // «²?«à«ª«Ö«¸«§«¯«Èª¬÷òÑ¥ªµªìªëªÈª­ªË¡¢applicationIsQuitting«Õ«é«°ªòtrueªËàâïÒ
        // °ÔÀÓ ¿ÀºêÁ§Æ®°¡ ÆÄ±«µÉ ¶§, applicationIsQuitting ÇÃ·¡±×¸¦ true·Î ¼³Á¤
        if (_instance == this) applicationIsQuitting = true;
    }

    public void SaveData()
    {
        if (StageId > MyBestStageID)
        {
            if (MyBestStageID != 9000)
                MyBestStageID = StageId;
        }
        SaveLoadSystem.SaveData.MyClearStageInfo = MyBestStageID;
        SaveLoadSystem.AutoSave();
    }


    public void LoadData()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
#elif UNITY_ANDROID
        var loadData = SaveLoadSystem.Load("cryptoSaveData.json") as SaveDataVC;
#endif

        if (loadData == null)
        {
            SelectedValue = new int[3] { 1, 2, 3 };
            Player.Instance.Init(new PlayerSaveData(DataTableMgr.GetTable<PlayerTable>()));

            InvManager.AddCard(new FairyCard(100001));
            InvManager.AddCard(new FairyCard(100002));
            InvManager.AddCard(new FairyCard(100003));
            InvManager.AddCard(new FairyCard(100006));
            InvManager.AddCard(new FairyCard(100009));
            InvManager.InitFairyCards();
        }
        else // loadData != null
        {
            StringTable.ChangeLanguage(loadData.Language);
            language = loadData.Language;
            SaveLoadSystem.SaveData.Language = StringTable.Lang;
            Player.Instance.Init(loadData.PlayerData);
            SelectedValue = loadData.MainScreenChar;
            BGSNum = loadData.BackGroundValue;
            Volume = loadData.volumeValue;
            SaveLoadSystem.SaveData.PlayerData = Player.Instance.SaveData;
            if (loadData.FairyInv.Count != 0)
            {
                InvManager.fairyInv.Inven = loadData.FairyInv;
                InvManager.InitFairyCards();
            }

            if (loadData.ItemInv.Count != 0)
            {
                InvManager.itemInv.Inven = loadData.ItemInv;
            }

            if (loadData.EquipInv.Count != 0)
            {
                InvManager.equipPieceInv.Inven = loadData.EquipInv;
            }

            if (loadData.SpiritStoneInv.Count != 0)
            {
                InvManager.spiritStoneInv.Inven = loadData.SpiritStoneInv;
            }

            MyBestStageID = loadData.MyClearStageInfo;
            SaveLoadSystem.SaveData.MyClearStageInfo = MyBestStageID;


            {   // ½ºÅä¸® Æí¼º Á¤º¸ ·Îµå
                StorySquadLeaderIndex = loadData.StorySquadLeaderIndex;
                for (int i = 0; i < loadData.StoryFairySquadData.Length; i++)
                {
                    if (InvManager.fairyInv.Inven.TryGetValue(loadData.StoryFairySquadData[i], out var fairyCard))
                    {
                        StoryFairySquad[i] = fairyCard;
                    }
                    else
                    {
                        Debug.LogWarning("StoryFairySquadData Error");
                        StoryFairySquad.Initialize();
                        StorySquadLeaderIndex = -1;
                        break;
                    }
                }
            }

            {   // µ¥ÀÏ¸® Æí¼º Á¤º¸ ·Îµå
                DailySquadLeaderIndex = loadData.DailySquadLeaderIndex;
                for (int i = 0; i < loadData.DailyFairySquadData.Length; i++)
                {
                    if (InvManager.fairyInv.Inven.TryGetValue(loadData.DailyFairySquadData[i], out var fairyCard))
                    {
                        DailyFairySquad[i] = fairyCard;
                    }
                    else
                    {
                        Debug.LogWarning("DailyFairySquadData Error");
                        DailyFairySquad.Initialize();
                        DailySquadLeaderIndex = -1;
                        break;
                    }
                }
            }
        }

    }

    public void ClearStage()
    {
        Debug.Log("stageClear");
        //isStageClear = true;
        //backgroundController.ActiveTailBackground();
        if (MyBestStageID < StageId)
            MyBestStageID = StageId;
        SaveData();
    }

    public void SetStoryFairySquad(FairyCard[] fairyArray, int leader)
    {
        Array.Clear(StoryFairySquad, 0, StoryFairySquad.Length);
        StoryFairySquad = fairyArray;
        StorySquadLeaderIndex = leader;
    }

    public void SetDailyFairySquad(FairyCard[] fairyArray, int leader)
    {
        Array.Clear(DailyFairySquad, 0, DailyFairySquad.Length);
        DailyFairySquad = fairyArray;
        DailySquadLeaderIndex = leader;
    }
}
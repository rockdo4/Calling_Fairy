using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDataVC = SaveDataV6;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    private static object _lock = new object();

    public static Dictionary<int, StringData> stringTable = new();

    public float ScaleFator { get; set; }

    public FairyCard[] StoryFairySquad { get; private set; } = new FairyCard[3];
    public int StorySquadLeaderIndex { get; set; } = -1;
    public FairyCard[] DailyFairySquad { get; private set; } = new FairyCard[3];
    public int DailySquadLeaderIndex { get; set; } = -1;
    public int[] SelectedValue { get; set; } = new int[3];
    public int StageId;
    public int MyBestStageID { get; private set; } = 9000;
    public static GameManager Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(GameManager) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (GameManager)FindObjectOfType(typeof(GameManager));

                    if (FindObjectsOfType(typeof(GameManager)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<GameManager>();
                        singleton.name = "(singleton) " + typeof(GameManager).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(GameManager) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    private void Awake()
    {
        ScaleFator = Camera.main.pixelHeight / 1080f;
        stringTable = DataTableMgr.GetTable<StringTable>().dic;
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }

    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName); ;
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
        }
        else // loadData != null
        {
            Player.Instance.Init(loadData.PlayerData);
            SelectedValue = loadData.MainScreenChar;
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


            {   // 스토리 편성 정보 로드
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

            {   // 데일리 편성 정보 로드
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
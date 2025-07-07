using System;
using System.Collections.Generic;
using UnityEngine;

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
            // アプリケーションが終了している場合、nullを返す
            // 어플리케이션이 종료된 경우 null을 반환
            if (applicationIsQuitting)
            {
                Debug.LogWarning(string.Format(@"[Singleton] Instance '{0}' already 
                    destroyed on application quit. Won't create again. returning null.",
                    typeof(GameManager)));
                return null;
            }

            // 非同期対応
            // 비동기 대응
            lock (_lock)
            {
                // まだインスタンスが存在しない場合、シーンから探す
                // 아직 인스턴스가 존재하지 않는 경우, 씬에서 찾음
                if (_instance == null)
                {
                    // GameManagerが2個以上の場合、一つを除いて全部削除する
                    // GameManager가 2개 이상인 경우, 하나를 제외하고 모두 삭제
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

                    // シーンにGameManagerが存在しない場合、新しいGameObjectを作成
                    // 씬에 GameManager가 존재하지 않는 경우, 새로운 GameObject를 생성
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

        Instance.SetSaveData();
    }

    private void Awake()
    {
        stringTable = DataTableMgr.GetTable<StringTable>().dic;
    }

    public void OnDestroy()
    {
        // ゲームオブジェクトが破棄されるときに、applicationIsQuittingフラグをtrueに設定
        // 게임 오브젝트가 파괴될 때, applicationIsQuitting 플래그를 true로 설정
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

    /// <summary>
    /// セーブデータをロードして、ゲームの状態を初期化します。
    /// Save Data를 로드하여 게임 상태를 초기화합니다.
    /// </summary>
    public void SetSaveData()
    {
        var saveData = SaveLoadSystem.SaveData;

        if (saveData == null)
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
        else
        {
            StringTable.ChangeLanguage(saveData.Language);
            language = saveData.Language;
            SaveLoadSystem.SaveData.Language = StringTable.Lang;
            Player.Instance.Init(saveData.PlayerData);
            SelectedValue = saveData.MainScreenChar;
            BGSNum = saveData.BackGroundValue;
            Volume = saveData.volumeValue;
            SaveLoadSystem.SaveData.PlayerData = Player.Instance.SaveData;

            // TODO: 調査＆修正
            if (saveData.FairyInv.Count != 0)
            {
                InvManager.InitFairyCards();
            }

            MyBestStageID = saveData.MyClearStageInfo;
            SaveLoadSystem.SaveData.MyClearStageInfo = MyBestStageID;

            {   // 스토리 편성 정보 로드
                StorySquadLeaderIndex = saveData.StorySquadLeaderIndex;
                for (int i = 0; i < saveData.StoryFairySquadData.Length; i++)
                {
                    if (InvManager.fairyInv.Inven.TryGetValue(saveData.StoryFairySquadData[i], out var fairyCard))
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
                DailySquadLeaderIndex = saveData.DailySquadLeaderIndex;
                for (int i = 0; i < saveData.DailyFairySquadData.Length; i++)
                {
                    if (InvManager.fairyInv.Inven.TryGetValue(saveData.DailyFairySquadData[i], out var fairyCard))
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
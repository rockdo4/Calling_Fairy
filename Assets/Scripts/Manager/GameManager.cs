using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDataVC = SaveDataV2;
public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    private static object _lock = new object();

    public float ScaleFator { get; set; }
    public FairyCard[] Team { get; set; } = new FairyCard[3];
    public int StageId;
    public int MyBestStageID { get; private set; }
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
            MyBestStageID = StageId;
        }
        SaveLoadSystem.SaveData.MyClearStageInfo = MyBestStageID;
        SaveLoadSystem.AutoSave();
    }

    public void LoadData()
    {
#if UNITY_EDITOR
        var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
#elif UNITY_ANDROID
        var loadData = SaveLoadSystem.Load("cryptoSaveData.json") as SaveDataVC;
#endif
        if (loadData == null)
            return;

        if (loadData?.PlayerSaveData != null)
            Player.Instance.Init(loadData.PlayerSaveData);
        if (loadData.FairyInv != null)
        {
            InvManager.fairyInv.Inven = loadData.FairyInv;
            InvManager.InitFairyCards();
        }
        if (loadData.SupInv != null)
            InvManager.supInv.Inven = loadData.SupInv;
        if (loadData.ItemInv != null)
            InvManager.itemInv.Inven = loadData.ItemInv;
        if (loadData?.EquipInv != null)
            InvManager.equipPieceInv.Inven = loadData.EquipInv;
        if (loadData?.SpiritStoneInv != null)
            InvManager.spiritStoneInv.Inven = loadData.SpiritStoneInv;
        if (loadData?.MyClearStageInfo != null)
            MyBestStageID = loadData.MyClearStageInfo;
    }

    public void ClearStage()
    {
        Debug.Log("stageClear");
        //isStageClear = true;
        //backgroundController.ActiveTailBackground();
        if(MyBestStageID<StageId)
            MyBestStageID = StageId;
        SaveData();
    }
}
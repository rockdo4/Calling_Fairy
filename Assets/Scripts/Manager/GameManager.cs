using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDataVC = SaveDataV1;
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
        //---------testCode-----------------
        StageId = 9003;
        Team[0] = new FairyCard(100006);
        Team[1] = new FairyCard(100001);
        Team[2] = new FairyCard(100009);
        //----------------------------------
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
        //var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
        var saveData = new SaveDataVC();
        saveData.EquipInv = InvManager.equipPieceInv.Inven;
        saveData.FairyInv = InvManager.fairyInv.Inven;
        saveData.SupInv = InvManager.supInv.Inven;
        saveData.ItemInv = InvManager.itemInv.Inven;
        saveData.SpiritStoneInv = InvManager.spiritStoneInv.Inven;
        //var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
        //if (loadData == null)
        //    return;
        if (StageId > MyBestStageID)
        {
            MyBestStageID = StageId;
        }
        saveData.MyClearStageInfo = MyBestStageID;
        SaveLoadSystem.Save(saveData, "saveData.json");
    }
    public void LoadData()
    {
        var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
        if (loadData == null)
            return;
        InvManager.equipPieceInv.Inven = loadData?.EquipInv;
        InvManager.fairyInv.Inven = loadData?.FairyInv;
        InvManager.supInv.Inven = loadData?.SupInv;
        InvManager.itemInv.Inven = loadData?.ItemInv;
        InvManager.spiritStoneInv.Inven = loadData?.SpiritStoneInv;
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
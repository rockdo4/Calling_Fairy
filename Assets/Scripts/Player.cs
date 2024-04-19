using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    private static Player _instance;
    private static bool applicationIsQuitting = false;
    private static object _lock = new object();

    [Tooltip("")]
    public int staminaRecoveryAmount = 1;
    [Tooltip("")]
    public float staminaRecoveryInterval = 300;
    public const int MaxLevel = 60;
    //public Action OnStatUpdate;

    public bool IsInit { get; private set; } = false;

    public string Name { get; set; } = "NoName";
    public int Level { get; private set; } = 1;
    public int Experience { get; private set; } = 0;
    public int MaxExperience { get; private set; }
    public int Stamina { get; set; }
    public int MaxStamina { get; private set; }
    public DateTime LastRecoveryTime { get; private set; }
    public int MainFairyID { get; private set; }
    public int Gold { get; private set; }

    // PlayerSaveData ��ü�� �����ϴ� �� �ƴ϶� Player�� �ʵ带 PlayerSaveData������ ��ȯ�ؼ� ��ȯ�ϱ� ������ ��뿡 ����!
    public PlayerSaveData SaveData
    {
        get
        {
            return new PlayerSaveData()
            {
                Name = Name,
                Level = Level,
                Experience = Experience,
                MaxExperience = MaxExperience,
                MaxStamina = MaxStamina,
                Stamina = Stamina,
                LastRecoveryTime = LastRecoveryTime,
                SummonStone = SummonStone,
                MainFairyID = MainFairyID,
            };
        }
    }

    public int SummonStone { get; private set; }

    public static Player Instance
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
                    _instance = (Player)FindFirstObjectByType(typeof(Player));
                    var Players = FindObjectsOfType(typeof(Player));
                    if (Players.Length > 1)
                    {
                        foreach (var player in Players)
                        {
                            if (!ReferenceEquals(_instance, (Player)player))
                            {
                                Destroy(player);
                            }
                        }
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<Player>();
                        singleton.name = "(singleton) " + typeof(Player).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(Player) +
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
        set
        {
            if (_instance != null)
                Destroy(_instance);
            _instance = value;
        }
    }

    private void Update()
    {
        if (Stamina >= MaxStamina)
            return;

        RecoveryStamina();
    }

    public void Init(PlayerSaveData saveData)
    {
        Name = saveData.Name;
        Level = saveData.Level;
        Experience = saveData.Experience;
        MaxExperience = saveData.MaxExperience;
        MaxStamina = saveData.MaxStamina;
        Stamina = saveData.Stamina;
        LastRecoveryTime = saveData.LastRecoveryTime;
        SummonStone = saveData.SummonStone;
        MainFairyID = saveData.MainFairyID;

        IsInit = true;
    }

    public void UseStamina(int amount)
    {
        Stamina -= amount;
        LastRecoveryTime = DateTime.Now;
        if (Stamina < 0)
            Stamina = 0;

        SaveLoadSystem.SaveData.PlayerData = SaveData;

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            UIManager.Instance.OnMainSceneUpdateUI();
        }
        SaveLoadSystem.AutoSave();
    }

    public void RecoveryStamina()
    {

        if (DateTime.Now >= LastRecoveryTime.AddSeconds(staminaRecoveryInterval))
        {
            var interval = DateTime.Now - LastRecoveryTime;
            var count = (int)interval.TotalSeconds / staminaRecoveryInterval;

            LastRecoveryTime = DateTime.Now;

            for (int i = 0; i < count; i++)
            {
                Stamina += staminaRecoveryAmount;
                
                if (Stamina > MaxStamina)
                {
                    Stamina = MaxStamina;
                    break;
                }
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                UIManager.Instance.OnMainSceneUpdateUI?.Invoke();
            }
            SaveLoadSystem.SaveData.PlayerData = SaveData;
            SaveLoadSystem.AutoSave();
        }
    }

    public bool UseSummonStone(int amount)
    {
        if (SummonStone < amount)
        {
            //��ȭ ���� �� ����ó��.
            return false;
        }
        SummonStone -= amount;
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            UIManager.Instance.OnMainSceneUpdateUI?.Invoke();
        }
        SaveLoadSystem.SaveData.PlayerData = SaveData;
        SaveLoadSystem.AutoSave();
        return true;
    }

    public void GetSummonStone(int amount)
    {
        SummonStone += amount;
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            UIManager.Instance.OnMainSceneUpdateUI?.Invoke();
        }
        SaveLoadSystem.SaveData.PlayerData = SaveData;
        SaveLoadSystem.AutoSave();
    }

    public void GetExperience(int exp)
    {
        if (Level >= MaxLevel)
            return;

        Experience += exp;

        var table = DataTableMgr.GetTable<PlayerTable>();
        while (Experience >= table.dic[Level].PlayerExp && Level < MaxLevel)
        {
            Experience -= table.dic[Level].PlayerExp;
            LevelUp(table);
        }

        SaveLoadSystem.SaveData.PlayerData = SaveData;
        SaveLoadSystem.AutoSave();
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            UIManager.Instance.OnMainSceneUpdateUI?.Invoke();
        }

    }

    private void LevelUp(PlayerTable table)
    {
        if (Level >= MaxLevel)
            return;

        Level++;
        MaxExperience = table.dic[Level].PlayerExp;
        MaxStamina = table.dic[Level].PlayerMaxStamina;
        Stamina += MaxStamina;

        foreach (var fairyCard in InvManager.fairyInv.Inven.Values)
        {
            fairyCard.SetStat();
        }

        /*
        if (OnStatUpdate != null)
            OnStatUpdate();
        */
    }

    public void GainGold(int amount)
    {
        Gold += amount;
        SaveGoldData();
    }

    private void SaveGoldData()
    {
        SaveLoadSystem.SaveData.Gold = Gold;
        SaveLoadSystem.AutoSave();
    }

    public void SetMainFairy(int id)
    {
        MainFairyID = id;

        SaveLoadSystem.SaveData.PlayerData = SaveData;
        SaveLoadSystem.AutoSave();
    }

    //public void GetSummonStone(int amount)
    //{
    //    SummonStone += amount;
    //    SaveSummonStoneData();
    //}

    //private void SaveSummonStoneData()
    //{
    //    SaveLoadSystem.SaveData.SummonStone = SummonStone;
    //    SaveLoadSystem.AutoSave();
    //}
}

using System;
using UnityEngine;


public class Player : MonoBehaviour
{
    private static Player _instance;
    private static bool applicationIsQuitting = false;
    private static object _lock = new object();
    private static Observable observable = new();

    [Tooltip("회복 스태미너")]
    public int staminaRecoveryAmount = 10;
    [Tooltip("스태미터 회복 시간(초)")]
    public float staminaRecoveryInterval = 10;
    public const int MaxLevel = 60;
    public Action OnStatUpdate;

    public int Level { get; set; }
    public int Experience { get; set; }
    public int MaxExperience { get; set; }
    public int Stamina { get; set; }
    public int MaxStamina { get; set; }
    public DateTime LastRecoveryTime { get; set; }


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

    private void Awake()
    {
        RecoveryStamina();
    }

    private void Update()
    {
        if (Stamina >= MaxStamina)
            return;

        RecoveryStamina();
    }

    public void Init(PlayerSaveData saveData)
    {
        Level = saveData.Level;
        Experience = saveData.Experience;
        MaxExperience = saveData.MaxExperience;
        MaxStamina = saveData.MaxStamina;
        Stamina = saveData.Stamina;
        LastRecoveryTime = saveData.LastRecoveryTime;
    }

    public void RecoveryStamina()
    {
        Debug.Log("RecoveryStamina");
        if (DateTime.Now >= LastRecoveryTime.AddSeconds(staminaRecoveryInterval))
        {
            var interval = DateTime.Now - LastRecoveryTime.AddSeconds(staminaRecoveryInterval);
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
            SaveLoadSystem.AutoSave();
        }
    }


    //반올림 또는 내림, 올림으로 정수 줄 것 요망
    public void GetExperience(int exp)
    {
        if (Level >= MaxLevel)
            return;

        Experience += exp;
        SaveLoadSystem.AutoSave();
        var table = DataTableMgr.GetTable<PlayerTable>();
        if (Experience >= table.dic[Level].PlayerExp)
        {
            Experience -= table.dic[Level].PlayerExp;
            LevelUp(table);
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

        if (OnStatUpdate != null)
            OnStatUpdate();

        SaveLoadSystem.AutoSave();
    }

    #region Observable Method
    public void AttachObserver(IObserver observer)
    {
        observable.Attach(observer);
    }

    public void DetachObserver(IObserver observer)
    {
        observable.Detach(observer);
    }

    // 예시로 상태가 변경되는 메서드
    public void ChangePlayerState()
    {
        // 상태 변경 로직
        // ...

        // 상태 변경 후 옵저버에게 알림
        observable.Notify();
    }
    #endregion
}

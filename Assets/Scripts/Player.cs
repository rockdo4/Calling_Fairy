using System;
using UnityEngine;


public class Player : MonoBehaviour
{
    private static Player instance;

    [Tooltip("회복 스태미너")]
    public int staminaRecoveryAmount;
    [Tooltip("스태미터 회복 시간(초)")]
    public float staminaRecoveryInterval;

    public const int MaxLevel = 60;

    public int Level { get; private set; }
    public int Experience { get; private set; }
    public int MaxExperience { get; private set; }
    public int Stamina { get; private set; }
    public int MaxStamina { get; private set; }
    public DateTime LastRecoveryTime { get; private set; }

    public static Player Instance
    {
        get
        {
            instance = FindAnyObjectByType<Player>();
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<Player>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public Player()
    {
        var table = DataTableMgr.GetTable<PlayerTable>();

        Level = 1;
        Experience = 0;
        MaxExperience = table.dic[Level].PlayerExp;
        MaxStamina = table.dic[Level].PlayerMaxStamina;
        Stamina = MaxStamina;
        LastRecoveryTime = DateTime.Now;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        RecoveryStamina();
    }

    private void Update()
    {
        if (Stamina >= MaxStamina)
            return;

        RecoveryStamina();
    }


    public void RecoveryStamina()
    {
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
        SaveLoadSystem.AutoSave();
    }
}

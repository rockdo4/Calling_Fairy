using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterData charData; //얘는 구조체 이름이다.
    public string CharID { get; set; } // 얘는 데이터 불러올
    private CharacterTable.Data characterData;
    CharacterStatus characterStatus;
    private void Awake()
    {
        characterStatus = FindObjectOfType<CharacterStatus>();
        CharID = "1010100101";
    }
    
    private void Start()
    {
        LoadCharacterData();
        ApplyCharacterData();
    }

    void LoadCharacterData()
    {
        characterData = characterStatus.GetCharacterData(CharID);
    }

    void ApplyCharacterData()
    {
        if (characterData != null)
        {
            charData.CharID = characterData.CharID;
            charData.CharName = characterData.CharName;
            charData.CharLevel = characterData.CharLevel;
            charData.CharPosition = characterData.CharPosition;
            charData.CharProperty = characterData.CharProperty;
            charData.CharStartingGrade = characterData.CharStartingGrade;
            charData.CharMinGrade = characterData.CharMinGrade;
            charData.CharPAttack = characterData.CharPAttack;
            charData.CharMAttack = characterData.CharMAttack;
            charData.CharSpeed = characterData.CharSpeed;
            charData.CharCritRate = characterData.CharCritRate;
            charData.CharMaxHP = characterData.CharMaxHP;
            charData.CharAccuracy = characterData.CharAccuracy;
            charData.CharPDefence = characterData.CharPDefence;
            charData.CharMDefence = characterData.CharMDefence;
            charData.CharAvoid = characterData.CharAvoid;
            charData.CharResistance = characterData.CharResistance;
            charData.CharExp = characterData.CharExp;
            charData.CharNextLevel = characterData.CharNextLevel;
            charData.CharAttackFactor = characterData.CharAttackFactor;
            charData.CharAttackRange = characterData.CharAttackRange;
            charData.CharSkill = characterData.CharSkill;
        }
        else
        {
            Debug.LogError("Character data not found for ID: " + CharID);
        }
    }
}
public struct CharacterData
{
    public string CharID { get; set; }
    public string CharName { get; set; }
    public int CharLevel { get; set; }
    public int CharPosition { get; set; }
    public int CharProperty { get; set; }
    public int CharStartingGrade { get; set; }
    public int CharMinGrade { get; set; }
    public int CharPAttack { get; set; }
    public int CharMAttack { get; set; }
    public float CharSpeed { get; set; }
    public float CharCritRate { get; set; }
    public int CharMaxHP { get; set; }
    public float CharAccuracy { get; set; }
    public int CharPDefence { get; set; }
    public int CharMDefence { get; set; }
    public float CharAvoid { get; set; }
    public float CharResistance { get; set; }
    public int CharExp { get; set; }
    public int CharNextLevel { get; set; }
    public float CharAttackFactor { get; set; }
    public float CharAttackRange { get; set; }
    public int CharSkill { get; set; }
}
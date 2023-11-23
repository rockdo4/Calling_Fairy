using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    Dictionary<string, CharacterTable.Data> CharData { get; set; }
    CharacterTable charTable;

    private void Awake()
    {
        CharData = new Dictionary<string, CharacterTable.Data>();
        charTable = new CharacterTable();
        LoadAllCharacterData();
    }

    void LoadAllCharacterData()
    {
        var allCharData = charTable.GetAllCharacterData();

        foreach (var data in allCharData)
        {
            CharData[data.CharID] = data;
        }
    }

    public CharacterTable.Data GetCharacterData(string charID)
    {
        if (!CharData.ContainsKey(charID))
        {
            Debug.LogError("Character data not found for ID: " + charID);
            return null;
        }

        return CharData[charID];
    }
}
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    SortedDictionary<int, CharData> charData { get; set; }
    CharacterTable charTable;

    private void Awake()
    {
        charData = new SortedDictionary<int, CharData>();
        charTable = new CharacterTable();
        LoadAllCharacterData();
    }

    void LoadAllCharacterData()
    {
        var allCharData = charTable.GetAllCharacterData();

        foreach (var data in allCharData)
        {
            charData[data.CharID] = data;
        }
    }

    public CharData GetCharacterData(int charID)
    {
        if (!charData.ContainsKey(charID))
        {
            Debug.LogError("Character data not found for ID: " + charID);
            return new CharData();
        }

        return charData[charID];
    }
}
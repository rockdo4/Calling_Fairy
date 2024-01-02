using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;
using SaveDataVC = SaveDataV6;	//������ ���ö����� ������Ʈ

public static class SaveLoadSystem
{
    public static int SaveDataVersion { get; } = 6; //������ ���ö����� ������Ʈ
    public static SaveDataVC SaveData { get; set; } = new SaveDataVC();
    private const string KEY = "rjeorhdiddlwkdekgns";

    public static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    public static void Init()
    {
        SaveData.FairyInv = InvManager.fairyInv.Inven;
        SaveData.SpiritStoneInv = InvManager.spiritStoneInv.Inven;
        SaveData.EquipInv = InvManager.equipPieceInv.Inven;
        SaveData.ItemInv = InvManager.itemInv.Inven;
    }

    //SaveData ���� �� ����
    public static void AutoSave()
    {
        Init();

#if UNITY_EDITOR
        Save(SaveData, "saveData.json");
#elif UNITY_ANDROID || UNITY_STANDALONE_WIN
		Save(SaveData, "cryptoSaveData.json");
#endif
    }

    public static void Save(SaveData data, string filename)
    {
        //������ ��ΰ� ��ũ�� �ִ� ���� ���͸��� �����ϴ����� Ȯ��
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, filename);

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();
            //Collection�� ��� �ް� �־ Add�� ����
            serializer.Converters.Add(new Vector3Converter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Serialize(writer, data);
        }

        var json = File.ReadAllText(path);

#if UNITY_EDITOR
        File.WriteAllText(path, json);
#elif UNITY_ANDROID || UNITY_STANDALONE_WIN
		var cryptodata = EnCryptAES.EncryptAes(json, KEY);
		File.WriteAllText(path, cryptodata);
#endif
    }

    public static SaveData Load(string filename)
    {
        var path = Path.Combine(SaveDirectory, filename);
        if (!File.Exists(path))
        {
            return null;
        }
        SaveData data = null;
        int version = 0;

#if UNITY_EDITOR || UNITY_STANDALONE
        var json = File.ReadAllText(path);
#elif UNITY_ANDROID || UNITY_STANDALONE_WIN
		var cryptoData = File.ReadAllText(path);
		var json = EnCryptAES.DecryptAes(cryptoData, KEY);
#endif

        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var jObg = JObject.Load(reader);
            //.Value<T> ����������� �������Ŀ� ���� 
            version = jObg["Version"].Value<int>();
        }
        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var serialize = new JsonSerializer();
            //Version�� �ش��ϴ� ������ȭ ����
            switch (version)
            {
                case 1:
                    data = serialize.Deserialize<SaveDataV1>(reader);
                    break;
                case 2:
                    data = serialize.Deserialize<SaveDataV2>(reader);
                    break;
                case 3:
                    data = serialize.Deserialize<SaveDataV3>(reader);
                    break;
                case 4:
                    data = serialize.Deserialize<SaveDataV4>(reader);
                    break;
                case 5:
                    data = serialize.Deserialize<SaveDataV5>(reader);
                    break;
                case 6:
                    data = serialize.Deserialize<SaveDataV6>(reader);
                    break;
            }

            while (data.Version < SaveDataVersion)
            {
                data = data.VersionUp();
            }
        }

        return data;
    }
}
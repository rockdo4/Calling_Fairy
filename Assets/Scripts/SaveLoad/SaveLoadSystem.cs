using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;
using SaveDataVC = SaveDataV6;	//새버전 나올때마다 업데이트

public static class SaveLoadSystem
{
    public static int SaveDataVersion { get; } = 6; //새버전 나올때마다 업데이트
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

    //SaveData 변경 시 수정
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
        //지정된 경로가 디스크에 있는 기존 디렉터리를 참조하는지를 확인
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, filename);

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();
            //Collection을 상속 받고 있어서 Add가 가능
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
            //.Value<T> 어떤데이터형을 가져오냐에 따라서 
            version = jObg["Version"].Value<int>();
        }
        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var serialize = new JsonSerializer();
            //Version에 해당하는 역직렬화 수행
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
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class SaveLoadSystem
{
    // 最新バージョンが追加される度に更新
    // 최신버전이 추가될 때마다 업데이트
    public const int saveDataVersion = 8;
    // 暗号化キー
    // 암호화 키
    private const string KEY = "rjeorhdiddlwkdekgns";

    private static SaveDataVC _saveData;
    public static SaveDataVC SaveData
    {
        get
        {
            if (_saveData == null)
            {
                _saveData = Load(SaveFileName) as SaveDataVC;
                if (_saveData == null)
                    _saveData = new SaveDataVC();
            }
            return _saveData;
        }
    }
    public static string SaveFileName
    {
        get
        {
#if UNITY_EDITOR
            return "saveData.json";
#else
            return "cryptoSaveData.json";
#endif
        }
    }

    public static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    // TODO: Save処理に関する処理を追加
    // TODO: Save 처리에 관한 처리를 추가
    public static void AutoSave()
    {
        Save(SaveData, SaveFileName);
    }

    // Editorでは暗号化せず、以外では暗号化して保存する
    // Editor에서는 암호화하지 않고, 그 외에서는 암호화해서 저장
    public static void Save(SaveData data, string filename)
    {
        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        var path = Path.Combine(SaveDirectory, filename);

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new Vector3Converter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Serialize(writer, data);
        }

        var json = File.ReadAllText(path);

#if UNITY_EDITOR
        File.WriteAllText(path, json);
#else
		var cryptodata = EnCryptAES.EncryptAes(json, KEY);
		File.WriteAllText(path, cryptodata);
#endif
    }

    public static SaveData Load(string filename)
    {
        var path = Path.Combine(SaveDirectory, filename);
        if (!File.Exists(path))
            return null;

        SaveData data = null;
        int version = 0;

#if UNITY_EDITOR || UNITY_STANDALONE
        var json = File.ReadAllText(path);
#else
		var cryptoData = File.ReadAllText(path);
		var json = EnCryptAES.DecryptAes(cryptoData, KEY);
#endif

        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var jObg = JObject.Load(reader);
            version = jObg["Version"].Value<int>();
        }
        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var serialize = new JsonSerializer();
            data = version switch
            {
                1 => serialize.Deserialize<SaveDataV1>(reader),
                2 => serialize.Deserialize<SaveDataV2>(reader),
                3 => serialize.Deserialize<SaveDataV3>(reader),
                4 => serialize.Deserialize<SaveDataV4>(reader),
                5 => serialize.Deserialize<SaveDataV5>(reader),
                6 => serialize.Deserialize<SaveDataV6>(reader),
                7 => serialize.Deserialize<SaveDataV7>(reader),
                8 => serialize.Deserialize<SaveDataV8>(reader),
                _ => null
            };

            while (data.Version < saveDataVersion)
            {
                data = data.VersionUp();
            }
        }

        return data;
    }
}
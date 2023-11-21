using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SaveDataVC = SaveDataV1;//새버전 나올때마다 업데이트

public static class SaveLoadSystem
{
	public static int SaveDataVersion { get; } = 1; //새버전 나올때마다 업데이트
    private const string KEY = "rjeorhdiddlwkdekgns";

    public static string SaveDirectory
	{
		get
		{
			return $"{Application.persistentDataPath}/Save";
		}
	}

    //세이브 데이터의 내용을  filename에다 저장함
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
        var cryptoData = EnCryptAES.EncryptAes(json, KEY);
        File.WriteAllText(path, cryptoData);

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

        var cryptoData = File.ReadAllText(path);
        var json = EnCryptAES.DecryptAes(cryptoData, KEY);

        using (var reader = new JsonTextReader(new StringReader(json)))
		{
			var jObg = JObject.Load(reader);
            //.Value<T> 어떤데이터형을 가져오냐에 따라서 
            version = jObg["Version"].Value<int>();
		}
		using (var reader = new JsonTextReader(new StringReader(json)))
		{
			var serialize = new JsonSerializer();
			switch (version)//새버전 나올때마다 추가
			{
				case 1:
					data = serialize.Deserialize<SaveDataV1>(reader);
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

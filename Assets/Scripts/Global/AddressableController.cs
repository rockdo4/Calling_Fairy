using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class AddressableController : Singleton<AddressableController>
{
    private readonly Dictionary<string, Object> _dic = new();
    private readonly HashSet<string> _deleteKeys = new();

    /// <summary>
    /// 씬 종료 시 임시로 로드한 에셋들 언로드하기
    /// </summary>
    private void Awake()
    {
        SceneManager.sceneUnloaded += Scene =>
        {
            foreach (var key in _deleteKeys)
            {
                ReleaseAsset(key);
            }
            _deleteKeys.Clear();
        };
    }

    /// <summary>
    /// 어드레서블을 활용한 에셋 불러오기
    /// </summary>
    /// <param name="key">에셋을 불러울 키의 이름</param>
    /// <param name="isTemp">씬이 바뀐 뒤 삭제할지 여부</param>
    /// <typeparam name="T">불러올 에셋의 형태</typeparam>
    /// <returns>불러온 에셋</returns>
    public async Task<T> GetAsset<T>(string key, bool isTemp = true) where T : Object
    {
        if (_dic.TryGetValue(key, out var value))
            return value as T;

        try
        {
            var obj = Addressables.LoadAssetAsync<Object>(key).WaitForCompletion();
            _dic.Add(key, obj);
            if(isTemp)
                _deleteKeys.Add(key);
            return obj as T;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return null;
    }

    /// <summary>
    /// 어드레서블을 활용한 게임오브젝트 불러오기
    /// </summary>
    /// <param name="key">게임오브젝트를 불러울 키의 이름</param>
    /// <param name="isTemp">씬이 바뀐 뒤 삭제할지 여부</param>
    /// <returns>불러온 게임오브젝트</returns>
    public Task<GameObject> GetGameObject(string key, bool isTemp = true)
    {
        return GetAsset<GameObject>(key, isTemp);
    }

    public void ReleaseAsset(string key)
    {
        _dic.Remove(key, out _);
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        // 오브젝트 이름
        public string objectName;
        // 오브젝트 풀에서 관리할 오브젝트
        public GameObject perfab;
        // 몇개를 미리 생성 해놓을건지
        public int count;
    }

    public static float ScaleFator { get; set; }
    public static ObjectPoolManager instance;

    // 오브젝트풀 매니저 준비 완료표시
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // 생성할 오브젝트의 key값지정을 위한 변수
    private string objectName;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // 활성화된 오브젝트들을 관리할 딕셔너리
    private Dictionary<string, List<GameObject>> activeObjects = new Dictionary<string, List<GameObject>>();
    // 오브젝트풀에서 오브젝트를 새로 생성할때 사용할 딕셔너리
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }


    private void Init()
    {
        IsReady = false;

        ScaleFator = Camera.main.pixelHeight / 1080f;
        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            if (goDic.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} 이미 등록된 오브젝트입니다.", objectInfos[idx].objectName);
                return;
            }

            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        Debug.Log("오브젝트풀링 준비 완료");
        IsReady = true;
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(goDic[objectName]);
        poolGo.name = objectName;
        poolGo.transform.localScale *= ScaleFator;

        poolGo.GetComponent<PoolAble>().Pool = ojbectPoolDic[objectName];
        poolGo.transform.SetParent(transform);
        // 활성화된 오브젝트 리스트에 추가
        if (!activeObjects.ContainsKey(objectName))
        {
            activeObjects[objectName] = new List<GameObject>();
        }
        activeObjects[objectName].Add(poolGo);
        return poolGo;
    }

    // 대여
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);

        activeObjects[poolGo.name].Remove(poolGo);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;

        if (goDic.ContainsKey(goName) == false)
        {
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", goName);
            return null;
        }

        return ojbectPoolDic[goName].Get();
    }

    public void ReturnGo(GameObject go)
    {
        if (go == null)
        {
            Debug.LogWarning("Trying to return a null GameObject to the pool.");
            return;
        }

        // 오브젝트 풀에 등록된 오브젝트인지 확인
        PoolAble poolAble = go.GetComponent<PoolAble>();
        if (poolAble == null || poolAble.Pool == null)
        {
            Debug.LogWarning("Trying to return a GameObject not managed by the object pool.");
            return;
        }

        // 반환
        poolAble.Pool.Release(go);
    }

    public List<GameObject> GetAllActiveObjects(string objectName)
    {
        if (activeObjects.ContainsKey(objectName))
        {
            return new List<GameObject>(activeObjects[objectName]);
        }
        else
        {
            return new List<GameObject>();
        }
    }
}
using System.Collections.Generic;
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
        public GameObject prefab;
        // 몇개를 미리 생성 해놓을건지
        public int count;
    }


    public static ObjectPoolManager instance;

    // 오브젝트풀 매니저 준비 완료표시
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // 생성할 오브젝트의 key값지정을 위한 변수
    private string objectName;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

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

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            if (goDic.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} 이미 등록된 오브젝트입니다.", objectInfos[idx].objectName);
                return;
            }

            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].prefab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;

                PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
                //poolAbleGo.transform.localScale = Vector3.one;
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        Debug.Log("오브젝트풀링 준비 완료");
        IsReady = true;
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(goDic[objectName],transform);
        poolGo.name = objectName;
        //poolGo.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        //poolGo.transform.localScale = Vector3.one;

        poolGo.GetComponent<PoolAble>().Pool = ojbectPoolDic[objectName];
        //poolGo.transform.SetParent(transform);
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
        GameObject go = ojbectPoolDic[goName].Get();
        //go.transform.localScale = Vector3.one;
        go.GetComponent<PoolAble>().RetrieveFromPool();
        return go;
    }

    public void ReturnGo(GameObject go)
    {
        if (go == null)
        {
            Debug.LogWarning("Trying to return a null GameObject to the pool.");
            return;
        }

        PoolAble poolAble = go.GetComponent<PoolAble>();
        if (poolAble != null)
        {
            if (poolAble.Pool==null)
            {
                //Debug.Log($"{go.name} 객체는 이미 반환되었습니다.");
                return;
            }
            else
            {
                //Debug.Log($"{go.name} 객체는 반환되지 않았습니다.");
                poolAble.Pool.Release(go);
                poolAble.IsPooled = true;
            }
        }
    }
}
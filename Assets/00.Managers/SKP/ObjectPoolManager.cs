using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        // ������Ʈ �̸�
        public string objectName;
        // ������Ʈ Ǯ���� ������ ������Ʈ
        public GameObject perfab;
        // ��� �̸� ���� �س�������
        public int count;
    }

    public static float ScaleFator { get; set; }
    public static ObjectPoolManager instance;

    // ������ƮǮ �Ŵ��� �غ� �Ϸ�ǥ��
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // ������ ������Ʈ�� key�������� ���� ����
    private string objectName;

    // ������ƮǮ���� ������ ��ųʸ�
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // Ȱ��ȭ�� ������Ʈ���� ������ ��ųʸ�
    private Dictionary<string, List<GameObject>> activeObjects = new Dictionary<string, List<GameObject>>();
    // ������ƮǮ���� ������Ʈ�� ���� �����Ҷ� ����� ��ųʸ�
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
                Debug.LogFormat("{0} �̹� ��ϵ� ������Ʈ�Դϴ�.", objectInfos[idx].objectName);
                return;
            }

            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // �̸� ������Ʈ ���� �س���
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        Debug.Log("������ƮǮ�� �غ� �Ϸ�");
        IsReady = true;
    }

    // ����
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(goDic[objectName]);
        poolGo.name = objectName;
        poolGo.transform.localScale *= ScaleFator;

        poolGo.GetComponent<PoolAble>().Pool = ojbectPoolDic[objectName];
        poolGo.transform.SetParent(transform);
        // Ȱ��ȭ�� ������Ʈ ����Ʈ�� �߰�
        if (!activeObjects.ContainsKey(objectName))
        {
            activeObjects[objectName] = new List<GameObject>();
        }
        activeObjects[objectName].Add(poolGo);
        return poolGo;
    }

    // �뿩
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);

        activeObjects[poolGo.name].Remove(poolGo);
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;

        if (goDic.ContainsKey(goName) == false)
        {
            Debug.LogFormat("{0} ������ƮǮ�� ��ϵ��� ���� ������Ʈ�Դϴ�.", goName);
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

        // ������Ʈ Ǯ�� ��ϵ� ������Ʈ���� Ȯ��
        PoolAble poolAble = go.GetComponent<PoolAble>();
        if (poolAble == null || poolAble.Pool == null)
        {
            Debug.LogWarning("Trying to return a GameObject not managed by the object pool.");
            return;
        }

        // ��ȯ
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    // 오브젝트 이름
    public string objectName;
    // 오브젝트 풀에서 관리할 오브젝트
    public GameObject prefab;
    // 몇개를 미리 생성 해놓을건지
    public int count;
}
public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    public List<GameObject> SkillObject1 = new List<GameObject>();
    public List<GameObject> SkillObject2 = new List<GameObject>();
    public List<GameObject> SkillObject3 = new List<GameObject>();
    public GameObject enemyBulletPrefab1;
    public GameObject enemyBulletPrefab2;
    public GameObject enemyBulletPrefab3;
    private int amountBullet = 30;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        InitiailizedBullet();
    }
    private void InitiailizedBullet()
    {
        GameObject bullet;
        for (int i = 0; i < amountBullet; i++)
        {
            bullet = Instantiate(enemyBulletPrefab1);
            bullet.SetActive(false);
            SkillObject1.Add(bullet.gameObject);
        }
    }

    public GameObject GetEnemyBullet()
    {

        for (int i = 0; i < amountBullet; i++)
        {
            if (!SkillObject1[i].activeInHierarchy)
            {
                return SkillObject1[i];
            }
        }
        return null;
    }

    public void OnDestroy()
    {
        SkillObject1 = null;
        SkillObject2 = null;
        SkillObject3 = null;
    }

    public void ReturnEnemyBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }


}
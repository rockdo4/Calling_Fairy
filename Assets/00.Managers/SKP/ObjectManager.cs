//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class ObjectInfo
//{
//    // 오브젝트 이름
//    public string objectName;
//    // 오브젝트 풀에서 관리할 오브젝트
//    public GameObject prefab;
//    // 몇개를 미리 생성 해놓을건지
//    public int count;
//}
//public class ObjectManager : MonoBehaviour
//{
//    public static ObjectManager instance;

//    public List<ObjectInfo> SkillObject = new List<ObjectInfo>();
//    public GameObject enemyBulletPrefab;
//    private int amountBullet = 999;

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        InitiailizedBullet();
//    }
//    private void InitiailizedBullet()
//    {
//        SkillObject = new List<ObjectInfo>();
//        GameObject bullet;
//        for (int i = 0; i < amountBullet; i++)
//        {
//            bullet = Instantiate(enemyBulletPrefab);
//            bullet.SetActive(false);
//            SkillObject.Add(bullet.gameObject);
//        }
//    }

//    public GameObject GetEnemyBullet()
//    {

//        for (int i = 0; i < amountBullet; i++)
//        {
//            if (!SkillObject[i].activeInHierarchy)
//            {
//                return SkillObject[i];
//            }
//        }
//        return null;
//    }

//    public void OnDestroy()
//    {
//        SkillObject = null;
//    }
//    public void ReturnEnemyBullet(GameObject bullet)
//    {
//        bullet.SetActive(false);
//    }


//}
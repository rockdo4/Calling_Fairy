using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChainEffector
{
    public SkillInfo[] chains;
    public GameObject man;
}
public class ChainEffect : MonoBehaviour
{
    SkillSpawn go;
    Dictionary<SkillInfo, GameObject> chainObjects = new();
    List<ChainEffector> effectList = new List<ChainEffector>();
    public GameObject prefab;

    private void Awake()
    {
        go = GetComponent<SkillSpawn>();
        go.onChainEffect += CheckChainEffect;
    }

    //public void CheckChainEffect(SkillInfo[] chain)
    //{
    //    for (int i = 0; i < effectList.Count; i++) 
    //    {
    //        for(int j = 0; j< effectList[i].chains.Length; j++) 
    //        {

    //            if(effectList[i].chains.Contains(chain[j]))
    //            {
    //                //Destroy(effectList[i].man);
    //                return;
    //            }
    //        }
    //    }
    //    //if(effectList.Count>0)
    //    //{
    //    //    if (effectList.Any(x => x.chains == chain))
    //    //        return;
    //    //}
    //    var gameObj = Instantiate(prefab);
    //    var newGO = new ChainEffector() { chains = chain, man = gameObj };
    //    gameObj.transform.SetParent(transform);
    //    effectList.Add(newGO);












    //    //foreach (var effect in chain)
    //    //{
    //    //    if (!chainObjects.ContainsKey(effect))
    //    //    {
    //    //        var newGo = Instantiate(prefab);
    //    //        newGo.transform.SetParent(transform);
    //    //        chainObjects.Add(effect, newGo);

    //    //    }
    //    //    var newEffector = new ChainEffector { chains = effect, man = chainObjects[effect] };
    //    //    for(int i = 0; i< effectList.Count; i++)
    //    //    {
    //    //        //if (!chainObjects.Any(effectList[i]))
    //    //        {
    //    //            effectList.Add(newEffector);
    //    //        }

    //    //    }
    //    //}

    //    //Debug.Log(effectList.Count);

    //}
    public void CheckChainEffect(SkillInfo[] chain)
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i].chains.Length == chain.Length && effectList[i].chains.SequenceEqual(chain))
            {
                Destroy(effectList[i].man);
                effectList.RemoveAt(i);
                break;
            }
        }

        // 새로운 체인을 이용해 새로운 게임 오브젝트를 생성
        var gameObj = Instantiate(prefab);
        var newGO = new ChainEffector() { chains = chain, man = gameObj };
        gameObj.transform.SetParent(transform);

        effectList.Add(newGO);
    }


}

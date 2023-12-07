//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using UnityEditor.SceneManagement;
//using UnityEngine;

//public class ChainEffect : MonoBehaviour
//{
//    SkillSpawn go;
//    List<ChainChecker> effectList = new List<ChainChecker>();
//    List<GameObject> newEffectList = new();
//    public GameObject prefab;
//    private GameObject chainEffectGo;
//    Vector3 twoChainSize = new Vector3(403f, 208f);
//    Vector3 threeChainSize = new Vector3(614f, 208f);
//    int chainCount;
//    GameObject chainTag;
//    private void Awake()
//    {
//        go = GetComponent<SkillSpawn>();
//        go.onChainEffect += CheckChainEffect;
//        chainTag = GameObject.FindWithTag(Tags.ChainEffect);
//    }

//    //public void CheckChainEffect(SkillInfo[] chain)
//    //{
//    //    for (int i = 0; i < effectList.Count; i++) 
//    //    {
//    //        for(int j = 0; j< effectList[i].chains.Length; j++) 
//    //        {

//    //            if(effectList[i].chains.Contains(chain[j]))
//    //            {
//    //                //Destroy(effectList[i].man);
//    //                return;
//    //            }
//    //        }
//    //    }
//    //    //if(effectList.Count>0)
//    //    //{
//    //    //    if (effectList.Any(x => x.chains == chain))
//    //    //        return;
//    //    //}
//    //    var gameObj = Instantiate(prefab);
//    //    var newGO = new ChainEffector() { chains = chain, man = gameObj };
//    //    gameObj.transform.SetParent(transform);
//    //    effectList.Add(newGO);












//    //    //foreach (var effect in chain)
//    //    //{
//    //    //    if (!chainObjects.ContainsKey(effect))
//    //    //    {
//    //    //        var newGo = Instantiate(prefab);
//    //    //        newGo.transform.SetParent(transform);
//    //    //        chainObjects.Add(effect, newGo);

//    //    //    }
//    //    //    var newEffector = new ChainEffector { chains = effect, man = chainObjects[effect] };
//    //    //    for(int i = 0; i< effectList.Count; i++)
//    //    //    {
//    //    //        //if (!chainObjects.Any(effectList[i]))
//    //    //        {
//    //    //            effectList.Add(newEffector);
//    //    //        }

//    //    //    }
//    //    //}

//    //    //Debug.Log(effectList.Count);

//    //}

//    public void DeleteEffect(ChainChecker chain)
//    {
//        if (effectList.Contains(chain))
//        {
//            var index = effectList.FindIndex(c => c == chain);
//            effectList.RemoveAt(index);
//            Destroy(effectList[index].chainsObj);
//        }
//    }


//    public void CheckChainEffect(ChainChecker chain)
//    {
//        chainCount = chain.chains.Length;
//        if (!effectList.Contains(chain))
//        {
//            for (int i = 0; i < chainCount; i++)
//            {
//                for (int j = 0; j < effectList.Count; j++)
//                {
//                    if(effectList[i].chains[0].SkillObject == chain.chains[j].SkillObject)
//                    {
//                        if (effectList[i].chains.Length == chain.chains.Length)
//                        {
//                            Debug.Log("찾았나");
//                        }
//                    }

//                }
//            }
//        }







//        {
//            effectList.Add(chain);
//            chain.chainsObj = Instantiate(prefab);
//            chainEffectGo = chain.chainsObj;
//            var pos = (chain.chains[0].SkillObject.transform.position + chain.chains[chainCount - 1].SkillObject.transform.position) / 2;
//            var chainPos = new Vector3(pos.x, pos.y);
//            chainEffectGo.transform.position = chainPos;
//            chainEffectGo.transform.SetParent(chainTag.transform);
//            Debug.Log("여기들름");
//            switch (chainCount)
//            {
//                case 2:
//                    chainEffectGo.transform.GetComponent<RectTransform>().sizeDelta = twoChainSize;

//                    break;

//                case 3:
//                    chainEffectGo.transform.GetComponent<RectTransform>().sizeDelta = threeChainSize;
//                    break;
//            }
//        }





















//        //chainEffectGo = chain.chainsObj;
//        //chainEffectGo = Instantiate(prefab);
//        ////chainEffectGo.transform.localScale = size;
//        //if (effectList.Count > 0)
//        //{
//        //    for (int i = effectList.Count - 1; i >= 0; i--)
//        //    {
//        //        if (effectList[i].gameObject == chainEffectGo)
//        //        {
//        //            Destroy(effectList[i].gameObject);
//        //            effectList.RemoveAt(i);
//        //        }
//        //    }
//        //}
//        //effectList.Add(chainEffectGo);
//        //chain.chains..transform.SetParent(chainEffectGo.transform);

//        //for (int i = 0; i < effectList.Count; i++)
//        //{
//        //    if (effectList[i].chains.Length == chain.chains.Length && effectList[i].chains.SequenceEqual(chain.chains))
//        //    {
//        //        //effectList[i].gameObject
//        //        Destroy(effectList[i].chain.chainsObj);
//        //        effectList.RemoveAt(i);
//        //        break;
//        //    }
//        //}

//        //// 새로운 체인을 이용해 새로운 게임 오브젝트를 생성
//        //chain.chainsObj = Instantiate(prefab);
//        //var newGO = new ChainChecker() { chains = chain.chains, chainsObj = chain.chainsObj };

//        //effectList.Add(newGO.chainsObj);
//    }


//}

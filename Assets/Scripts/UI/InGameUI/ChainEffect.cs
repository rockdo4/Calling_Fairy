using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChainEffector
{
    public SkillInfo chains;
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

    public void CheckChainEffect(SkillInfo[] chain)
    {
        foreach (var effect in chain)
        {
            if (!chainObjects.ContainsKey(effect))
            {
                var newGo = Instantiate(prefab);
                newGo.transform.SetParent(transform);
                chainObjects.Add(effect, newGo);

            }
            var newEffector = new ChainEffector { chains = effect, man = chainObjects[effect] };
            for(int i = 0; i< effectList.Count; i++)
            {
                if (!chainObjects.Any(effectList[i]))
                {
                    effectList.Add(newEffector);
                }

            }
        }

        Debug.Log(effectList.Count);
    }

    public void DeletechainEffect(SkillInfo[] chain)
    {

    }

}

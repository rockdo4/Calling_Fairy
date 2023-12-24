using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackground : MonoBehaviour
{
    [SerializeField]
    private GameObject farBackgrounds1;
    [SerializeField]
    private GameObject farBackgrounds2;
    [SerializeField]
    private GameObject farBackgrounds3;
    [SerializeField]
    private GameObject nearBackgrounds;
    [SerializeField]
    private GameObject SkyBackground;
    public void SetBackgroundData(string bgName)
    {
        var mapName = bgName;
        var baseSprite = Resources.Load<Sprite>($"Background/{mapName}/Texture/Base");
        var farA = Resources.Load<Sprite>($"Background/{mapName}/Texture/FarA");
        var farB = Resources.Load<Sprite>($"Background/{mapName}/Texture/FarB");
        var farC = Resources.Load<Sprite>($"Background/{mapName}/Texture/FarC");
        var sky = Resources.Load<Sprite>($"Background/{mapName}/Texture/Sky");

        farBackgrounds1.GetComponentInChildren<SpriteRenderer>().sprite = farA;
        farBackgrounds2.GetComponentInChildren<SpriteRenderer>().sprite = farB;
        farBackgrounds3.GetComponentInChildren<SpriteRenderer>().sprite = farC;
        nearBackgrounds.GetComponentInChildren<SpriteRenderer>().sprite = baseSprite;        
        SkyBackground.GetComponentInChildren<SpriteRenderer>().sprite = sky;
    }
}

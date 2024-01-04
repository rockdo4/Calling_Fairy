using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderManager : MonoBehaviour
{
    private AudioManager audioSound;
    
    private float[] slidersValue = new float[3];
    private float[] previousValue = new float[3];
    [SerializeField]
    private Slider[] sliders;
    private bool isFirst = false;
    private void Awake()
    {
        //LoadValue();
        audioSound = AudioManager.Instance;
        audioSound.masterSlider.onValueChanged.AddListener(SetMasterVolume);
        audioSound.bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        audioSound.seSlider.onValueChanged.AddListener(SetSEVolume);
        
    }
    private void OnEnable()
    {
        LoadValue();
        previousValue = slidersValue;
    }
    public void LoadValue()
    {
        if (!isFirst)
        {
            slidersValue = GameManager.Instance.Volume;
            isFirst = true;
        }
        else
        {
            slidersValue = SaveLoadSystem.SaveData.volumeValue;
        }
        for (int i = 0; i < slidersValue.Length; i++)
        {
            sliders[i].value = slidersValue[i];
        }
    }
    private void SetMasterVolume(float arg0)
    {
        slidersValue[0] = arg0;
        audioSound.MasterVolume(arg0);
        Debug.Log("Master조절중");
    }

    private void SetBGMVolume(float arg0)
    {
        slidersValue[1] = arg0;
        audioSound.BGMVolume(arg0);
        Debug.Log("BGM조절중");
    }

    private void SetSEVolume(float arg0)
    {
        slidersValue[2] = arg0;
        audioSound.SEVolume(arg0);
        Debug.Log("se조절중");
    }

    public void SaveVolume()
    {
        for (int i = 0; i < slidersValue.Length; i++)
        { 
            SaveLoadSystem.SaveData.volumeValue[i] = slidersValue[i]; 
        }
    }
    public void ReturnVolume()
    {
        SetMasterVolume(previousValue[0]);
        SetBGMVolume(previousValue[1]);
        SetSEVolume(previousValue[2]);
    }
}
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderManager : MonoBehaviour
{
    private AudioManager audioSound;
    
    private float[] slidersValue = new float[3];
    private float[] previousValue = new float[3];
    private float[] muteValue = new float[3];
    [SerializeField]
    private Slider[] sliders;
    private bool isFirst = false;
    private bool[] mute = new bool[3];
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
        Debug.Log($"Master조절중: {arg0}");
    }

    private void SetBGMVolume(float arg0)
    {
        slidersValue[1] = arg0;
        audioSound.BGMVolume(arg0);
        Debug.Log($"BGM조절중: {arg0}");
    }

    private void SetSEVolume(float arg0)
    {
        slidersValue[2] = arg0;
        audioSound.SEVolume(arg0);
        Debug.Log($"se조절중: {arg0}");
    }

    public void SaveVolume()
    {
        for (int i = 0; i < slidersValue.Length; i++)
        { 
            SaveLoadSystem.SaveData.volumeValue[i] = slidersValue[i]; 
            Debug.Log($"se조절중: {slidersValue[i]}");
        }
    }
    public void ReturnVolume()
    {
        SetMasterVolume(previousValue[0]);
        SetBGMVolume(previousValue[1]);
        SetSEVolume(previousValue[2]);
    }
    public void SwitchOff(int num)
    {
        var go = transform.GetChild(2).GetChild(num);
        Debug.Log(go.name);
        if (!mute[num])
        {
            //음소거가 된다.
            //Debug.Log(go.name);
            go.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
            go.transform.GetChild(4).GetChild(1).gameObject.SetActive(false);
            
            mute[num] = true;
            AudioManager.Instance.isMute[num] = mute[num];
            if(num ==0 || num == 1)
            {
                
            }
        }
        else if (mute[num])
        {
            //음소거가 아니다.
            go.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
            go.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
            mute[num] = false;
            AudioManager.Instance.isMute[num] = mute[num];
        }
        AudioManager.Instance.MuteBGM();
        muteCheck(num);
    }

    private void muteCheck(int num)
    {
        if (num == 0)
        {
            SetMasterVolume(slidersValue[0]);
        }
        
        if (num == 1)
        {
            SetBGMVolume(slidersValue[1]);
        }
        
        if (num ==2)
        {
            SetSEVolume(slidersValue[2]);
        }
        
    }
}
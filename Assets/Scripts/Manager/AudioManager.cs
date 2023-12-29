using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private static bool applicationIsQuitting = false;
    private static object _lock = new object();
    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    private AudioSource bgmSource;
    private List<AudioSource> seSources;
    [SerializeField]
    private AudioClip[] bgm;

    public static AudioManager Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(GameManager) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (AudioManager)FindFirstObjectByType(typeof(AudioManager));
                    var AudioManagers = FindObjectsOfType(typeof(AudioManager));
                    if (AudioManagers.Length > 1)
                    {
                        foreach (var audioManager in AudioManagers)
                        {
                            if (!ReferenceEquals(_instance, (AudioManager)audioManager))
                            {
                                Destroy(audioManager);
                            }
                        }
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<AudioManager>();
                        singleton.name = "(singleton) " + typeof(AudioManager).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(AudioManager) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
        set
        {
            if (_instance != null)
                Destroy(_instance);
            _instance = value;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        seSources = new List<AudioSource>();
        bgmSource = GetComponent<AudioSource>();

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        StopBGM();
        PlayBGM(bgm[arg1.buildIndex]);
    }

    private void Start()
    {
        // 게임 시작 시 저장된 볼륨값 불러오기 (기본값은 1)
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sevolume = PlayerPrefs.GetFloat("SEVolume", 1f);
        MasterVolume(masterVolume);
        BGMVolume(bgmVolume);
        SEVolume(sevolume);
    }

    public void UIUpdate()
    {
        audioMixer.GetFloat("Master", out float masterVolume);
        masterSlider.value = masterVolume;
        audioMixer.GetFloat("BGM", out float bgmVolume);
        bgmSlider.value = bgmVolume;
        audioMixer.GetFloat("SE", out float sevolume);
        seSlider.value = sevolume;
    }
    
    // BGM 재생
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // BGM 멈춤
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // SE 재생
    public void PlaySE(AudioClip clip)
    {
        if (audioMixer == null)
            return;
        AudioSource source = GetAvailableSESource();
        if (source != null)
        {
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SE")[0];
            source.clip = clip;
            source.Play();
        }
    }

    // 사용 가능한 SE 오디오 소스를 가져옴
    private AudioSource GetAvailableSESource()
    {
        foreach (AudioSource source in seSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // 사용 가능한 SE 오디오 소스가 없으면 새로 생성
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        seSources.Add(newSource);
        return newSource;
    }
    public void MasterVolume(float volume)
    {
        if (audioMixer == null)
            return;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }
    public void BGMVolume(float volume)
    {
        if (audioMixer == null)
            return;
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void SEVolume(float volume)
    {
        if (audioMixer == null)
            return;
        audioMixer.SetFloat("SE", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("SEVolume", volume);
        PlayerPrefs.Save();
    }
}
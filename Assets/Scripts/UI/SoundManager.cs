using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;
    #region singleton
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion singleton

    private SaveLoadManager SLManager;

    [Header("사운드 등록")]
    [SerializeField] Sound[] bgmSounds;
    [SerializeField] Sound[] effectSounds;

    [Header("브금 플레이어")]
    [SerializeField] AudioSource bgmPlayer;

    [Header("효과음 플레이어")]
    [SerializeField] AudioSource[] effectSoundPlayer;
    
    private Slider[] volumeSlider;

    private float MasterVolume = 0.5f;
    private float BgmVolume = 0.5f;
    private float SFXVolume = 0.5f;

    void Start()
    {
        volumeSlider = new Slider[3];
        PlayBgm("Title");
        SLManager = GameObject.FindWithTag("SLManager").GetComponent<SaveLoadManager>();
        MasterVolume = SLManager.LoadSound("Master");
        BgmVolume = SLManager.LoadSound("Bgm");
        SFXVolume = SLManager.LoadSound("SFX");
    }

    void Update()
    {
        if(volumeSlider[0] == null)
        {
            volumeSlider[0] = GameObject.FindWithTag("Master").GetComponent<Slider>();
            volumeSlider[1] = GameObject.FindWithTag("Bgm").GetComponent<Slider>();
            volumeSlider[2] = GameObject.FindWithTag("EffectSound").GetComponent<Slider>();

            volumeSlider[0].value = MasterVolume;
            volumeSlider[1].value = BgmVolume;
            volumeSlider[2].value = SFXVolume;

            volumeSlider[0].onValueChanged.AddListener(SetMasterVolume);
            volumeSlider[1].onValueChanged.AddListener(SetBgmVolume);
            volumeSlider[2].onValueChanged.AddListener(SetSFXVolume);

            SLManager = GameObject.FindWithTag("SLManager").GetComponent<SaveLoadManager>();
        }
        bgmPlayer.volume = MasterVolume * BgmVolume;
    }

    public void PlayBgm(string _soundName)
    {
        for(int soundCheck = 0; soundCheck < bgmSounds.Length; soundCheck++)
        {
            if(_soundName == bgmSounds[soundCheck].soundName)
            {
                bgmPlayer.clip = bgmSounds[soundCheck].clip;
                bgmPlayer.Play();
                return;
            }
        }
        Debug.Log("등록되지 않은 bgm입니다.");
        return;
    }

    public void PlaySFX(string _soundName)
    {
        for (int soundCheck = 0; soundCheck < effectSounds.Length; soundCheck++)
        {
            if(_soundName == effectSounds[soundCheck].soundName)
            {
                for (int playCheck = 0; playCheck < effectSoundPlayer.Length; playCheck++)
                {
                    if(!(effectSoundPlayer[playCheck].isPlaying))
                    {
                        effectSoundPlayer[playCheck].clip = effectSounds[soundCheck].clip;
                        effectSoundPlayer[playCheck].volume = MasterVolume * SFXVolume;
                        effectSoundPlayer[playCheck].PlayOneShot(effectSoundPlayer[playCheck].clip);
                        return;
                    }
                }
                Debug.Log("모든 오디오 소스가 사용 중 입니다.");
                return;
            }
        }
        Debug.Log("등록되지 않은 효과음 입니다.");
        return;
    }

    public void SetMasterVolume(float value)
    {
        MasterVolume = value;
        SLManager.SaveSound("Master", MasterVolume);
    }

    public void SetBgmVolume(float value)
    {
        BgmVolume = value;
        SLManager.SaveSound("Bgm", BgmVolume);
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = value;
        SLManager.SaveSound("SFX", SFXVolume);
    }
}

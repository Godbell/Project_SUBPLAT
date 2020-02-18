using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/*사운드의 이름을 설정하고 오디오 클립을 넣는 클래스*/
[System.Serializable]
public class Sound
{
    public string soundName;    //함수로 Sound를 찾을 때 이용
    public AudioClip clip;      //재생시킬 클립
}

public class SoundManager : MonoBehaviour
{
    //다른 스크립트에서 Sound 매니저를 이용할 때 사용
    static public SoundManager instance;

    //씬을 넘어갈 때 같이 넘어가도록 싱글톤한 것
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

    /*필요한 Sound를 list로 저장, Inspector창에서 추가*/
    [Header("사운드 등록")]
    [SerializeField] Sound[] bgmSounds;
    [SerializeField] Sound[] effectSounds;

    /*브금을 재생할 AudioSource를 등록*/
    [Header("브금 플레이어")]
    [SerializeField] AudioSource bgmPlayer;

    /*효과음을 재생할 AudioSource를 등록*/
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

    //bgmSounds에 들어있는 Sound중 이름이 같은 Sound의 클립을 재생
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

    //effectSounds에 들어있는 Sound중 이름이 같은 Sound의 클립을 재생
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

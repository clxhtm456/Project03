using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private List<AudioSource> effectSource = new List<AudioSource>();
    private AudioSource bgmSource;
    public AudioClip[] effectList;
    public AudioClip[] bgmList;
    private Coroutine bgmCorutine;
    // Use this for initialization

    public float BGMVolume
    {
        set
        {
            bgmSource.volume = value;
        }
        get
        {
            return bgmSource.volume;
        }
        
    }
    public float EFFECTVolume
    {
        set
        {
            for (int i = 0; i < effectSource.Count; i++)
            {
                effectSource[i].volume = value;
            }
        }
        get
        {
            if (effectSource.Count > 0)
                return effectSource[0].volume;
            else
                return 0.5f;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);
            AudioSource audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.playOnAwake = false;
            audiosource.loop = true;
            audiosource.volume = 0.5f;
            bgmSource = audiosource;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayEffect(string _name)
    {
        if (effectSource.Count > 5)
            return;
        if (_name == null)
            return;
        foreach (AudioSource temp in effectSource)
        {
            if (!temp.isPlaying)
            {
                for (int i = 0; i < effectList.Length; i++)
                {
                    if (effectList[i].name == _name)
                    {
                        temp.clip = effectList[i];

                        temp.Play();
                        return;
                    }
                }
            }
        }
        AudioSource audiosource = gameObject.AddComponent<AudioSource>();
        audiosource.playOnAwake = false;
        audiosource.Stop();
        effectSource.Add(audiosource);
        PlayEffect(_name);
    }
    public void PlayEffect(AudioClip _clip)
    {
        if (effectSource.Count > 5)
            return;
        if (_clip == null)
            return;
        foreach (AudioSource temp in effectSource)
        {
            if (!temp.isPlaying)
            {
                temp.clip = _clip;
                temp.Play();
                return;
            }
        }
        AudioSource audiosource = gameObject.AddComponent<AudioSource>();
        audiosource.playOnAwake = false;
        audiosource.Stop();
        audiosource.volume = EFFECTVolume;
        effectSource.Add(audiosource);
        PlayEffect(_clip);
    }

    public void PlayBGM(string _name,float _startTimer = 0.0f)
    {
        for (int i = 0; i < bgmList.Length; i++)
        {
            if (bgmList[i].name == _name)
            {
                //bgmCorutine = StartCoroutine(FadeOutBGMCor(bgmList[i], _startTimer));
                bgmSource.clip = bgmList[i];
                bgmSource.time = _startTimer;
                bgmSource.Play();
                return;
            }
        }
        Debug.Log("bgm사운드를 찾을수없음");
        return;
    }
    public void PlayBGM(int _count, float _startTimer = 0.0f)
    {
        bgmSource.clip = bgmList[_count];
        bgmSource.time = _startTimer;
        bgmSource.Play();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    Alert,
    BakeSound,
    BoilSound,
}

public enum SE
{
    CutSound,
    RifregeratorOpen,
    DoorOpen,
    DoorClose,
    PlayerFootstepOnFlooring,
    PlayerFootstepOnCarpet,
}

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    [Range(0.0f, 1.0f)]
    public float volume;
}

[DefaultExecutionOrder(-1)]
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public Sound[] bgmSound;
    public Sound[] seSound;

    public Dictionary<BGM, Sound> bgmdic = new Dictionary<BGM, Sound>();
    public Dictionary<SE, Sound> sedic = new Dictionary<SE, Sound>();

    [SerializeField]
    private AudioSource audioSource_BGM;
    [SerializeField]
    private AudioSource audioSource_SE;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void BgmRegister()
    {
        for (int i = 0; i < bgmSound.Length; i++)
        {
            bgmdic.Add((BGM)i, bgmSound[i]);
        }
    }

    private void SeRegister()
    {
        for (int i = 0; i < seSound.Length; i++)
        {
            sedic.Add((SE)i, seSound[i]);
        }
    }

    public void PlayBgm(BGM key, float volume = -1, float pitch = 1, bool isLoop = false)
    {
        Sound sound = bgmdic[key];
        AudioClip clip = sound.clip;
        //Debug.Log("<color=blue>" + clip.name + ":" + volume + "</color>");
        audioSource_BGM.clip   = clip;
        audioSource_BGM.volume = (volume == -1)
                                     ? sound.volume
                                     : Mathf.Clamp(volume, 0, 1);

        audioSource_BGM.loop  = isLoop;
        audioSource_BGM.pitch = Mathf.Clamp(pitch, -3, 3);

        audioSource_BGM.Play();
    }

    public void PlaySe(SE key, float volume = -1, float pitch = 1)
    {
        Sound sound = sedic[key];
        AudioClip clip = sound.clip;
        //Debug.Log("<color=blue>" + clip.name + ":" + volume + "</color>");
        audioSource_SE.volume = (volume == -1)
                                    ? sound.volume
                                    : Mathf.Clamp(volume, 0, 1);

        audioSource_SE.pitch = Mathf.Clamp(pitch, -3, 3);

        audioSource_SE.PlayOneShot(clip);
    }

    public void FadeOutBgm(float fadeTime)
    {
        StartCoroutine(FadeOut(fadeTime));
    }

    private IEnumerator FadeOut(float time)
    {
        float _time = time;
        float vol = audioSource_BGM.volume;
        while (_time > 0f)
        {
            _time -= Time.deltaTime;
            audioSource_BGM.volume = vol * _time / time;
            yield return null;
        }
        audioSource_BGM.Stop();
        audioSource_BGM.clip = null;
        yield break;
    }

    // Start is called before the first frame update
    void Start()
    {
        BgmRegister();
        SeRegister();

        Debug.Log("LoadBGMCount:" + bgmdic.Count);
        Debug.Log("LoadSECount:" + sedic.Count);
    }
}

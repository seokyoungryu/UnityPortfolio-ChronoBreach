using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class SoundManager : Singleton<SoundManager>
{
    private SoundData soundData = null;
    [SerializeField] private SoundList mainSceneBgm;
    [SerializeField] private SoundList practiceMode1SceneBgm;


    [Header("Audio Constants")]
    private const string MixerPath = "Sound/AudioMixer/AudioMixer";
    private const string AudioRootName = "AudioContainer";
    private const string BGMAudioSourceAName = "BGMAudioSourceA";
    private const string BGMAudioSourceBName = "BGMAudioSourceB";
    private const string UIAudioSoureceName = "UIAudioSource";
    private const string EffectAudioSoureceName = "EffectAudioSource";
    private const string EffectAudioSourceParentFolderName = "EffectAudioSources";
    private const string ExtraAudioSourceParentFolderName = "ExtraAudioSources";
    private const string MasterGroupName = "Master";
    private const string BGMGroupName = "BGM";
    private const string UIGroupName = "UI";
    private const string EffectGroupName = "Effect";

    [Header("Reference ")]
    public UISounds uiSounds = null;
    public Transform audioRoot = null;
    public AudioMixer mixer = null;
    public AudioSource bgmAudioSourceA = null;
    public AudioSource bgmAudioSourceB = null;
    public AudioSource[] uiAudioSource = null;
    public AudioSource oneShotAudioSource = null;
    public AudioSource[] effectAudioSource = null;
    public AudioSource[] extraAudioSource = null;
    public SoundClip currentBGMClip = null;
    public SoundClip lastBGMClip = null;
    public AudioClip currentClip = null;

    [Header("Values ")]
    public int uiAudioSourceCount = 5;
    public int effectAudioSourceCount = 5;
    public int extraAudioSourceCount = 5;

    private bool isFading = false;
    private float minVolume = -80.0f;
    private float maxVolume = 0.0f;

    public SoundList MainSceneBGM => mainSceneBgm;
    public SoundList PracticeMode1SceneBgm => practiceMode1SceneBgm;

    protected override void Awake()
    {
        base.Awake();
        Initialization();
    }

    /// <summary>
    /// 변수들 초기화 및 생성&할당
    /// </summary>
    private void Initialization()
    {
        if (uiSounds == null) uiSounds = GetComponent<UISounds>();

        if (soundData == null)
        {
            soundData = ScriptableObject.CreateInstance<SoundData>();
            soundData.LoadData();
        }
        if (audioRoot == null)
        {
            audioRoot = new GameObject(AudioRootName).transform;
            audioRoot.SetParent(transform);
            audioRoot.localPosition = Vector3.zero;
        }
        if (mixer == null)
        {
            mixer = Resources.Load(MixerPath) as AudioMixer;
        }
        if (bgmAudioSourceA == null)
        {
            GameObject container = new GameObject(BGMAudioSourceAName, typeof(AudioSource));
            container.transform.SetParent(audioRoot);
            bgmAudioSourceA = container.GetComponent<AudioSource>();
        }
        if (bgmAudioSourceB == null)
        {
            GameObject container = new GameObject(BGMAudioSourceBName, typeof(AudioSource));
            container.transform.SetParent(audioRoot);
            bgmAudioSourceB = container.GetComponent<AudioSource>();
        }
        if (uiAudioSource == null || effectAudioSource.Length == 0)
        {
            uiAudioSource = new AudioSource[uiAudioSourceCount];
            GameObject rootContainer = new GameObject(UIAudioSoureceName);
            rootContainer.transform.SetParent(audioRoot);

            for (int i = 0; i < uiAudioSourceCount; i++)
            {
                GameObject container = new GameObject(UIAudioSoureceName + i, typeof(AudioSource));
                container.transform.SetParent(rootContainer.transform);
                uiAudioSource[i] = container.GetComponent<AudioSource>();
            }
        }
        if (effectAudioSource == null || effectAudioSource.Length == 0)
        {
            effectAudioSource = new AudioSource[effectAudioSourceCount];
            GameObject rootContainer = new GameObject(EffectAudioSourceParentFolderName);
            rootContainer.transform.SetParent(audioRoot);
            for (int i = 0; i < effectAudioSourceCount; i++)
            {
                GameObject container = new GameObject(EffectAudioSoureceName + i, typeof(AudioSource));
                container.transform.SetParent(rootContainer.transform);
                effectAudioSource[i] = container.GetComponent<AudioSource>();
            }
        }
        if (extraAudioSource == null || extraAudioSource.Length == 0)
        {
            extraAudioSource = new AudioSource[extraAudioSourceCount];
            GameObject rootContainer = new GameObject(ExtraAudioSourceParentFolderName);
            rootContainer.transform.SetParent(audioRoot);
            for (int i = 0; i < extraAudioSourceCount; i++)
            {
                GameObject container = new GameObject(ExtraAudioSourceParentFolderName + i, typeof(AudioSource));
                container.transform.SetParent(rootContainer.transform);
                extraAudioSource[i] = container.GetComponent<AudioSource>();
            }
        }
        if (oneShotAudioSource == null)
        {
            GameObject container = new GameObject(EffectAudioSoureceName, typeof(AudioSource));
            container.transform.SetParent(audioRoot);
            oneShotAudioSource = container.GetComponent<AudioSource>();
        }
        if (mixer != null)
        {
            bgmAudioSourceA.outputAudioMixerGroup = mixer.FindMatchingGroups(BGMGroupName)[0];
            bgmAudioSourceB.outputAudioMixerGroup = mixer.FindMatchingGroups(BGMGroupName)[0];

            for (int i = 0; i < uiAudioSource.Length; i++)
                uiAudioSource[i].outputAudioMixerGroup = mixer.FindMatchingGroups(UIGroupName)[0];

            for (int i = 0; i < effectAudioSource.Length; i++)
                effectAudioSource[i].outputAudioMixerGroup = mixer.FindMatchingGroups(EffectGroupName)[0];
        }

        AudioSource[] audios = GetComponentsInChildren<AudioSource>();
        for (int i = 0; i < audios.Length; i++)
        {
            audios[i].playOnAwake = false;
        }

        MixerVolumInit();
    }

    private void MixerVolumInit()
    {
        if (mixer != null)
        {
            mixer.SetFloat(MasterGroupName, GetMasterVolume());
            mixer.SetFloat(BGMGroupName, GetBGMVolume());
            mixer.SetFloat(UIGroupName, GetUIVolume());
            mixer.SetFloat(EffectGroupName, GetEffectVolume());
        }
    }

    public SoundData GetSoundData() => soundData;

    #region Get Set Mixer (Save Load)

    //Master
    public void SetMasterVolume(float currentVolume)
    {
        currentVolume = Mathf.Clamp01(currentVolume);
        float volum = Mathf.Lerp(minVolume, maxVolume, currentVolume);
        mixer.SetFloat(MasterGroupName, volum);
        PlayerPrefs.SetFloat(MasterGroupName, volum);
    }

    public float GetMasterVolume()
    {
        if (PlayerPrefs.HasKey(MasterGroupName))
            return Mathf.Lerp(minVolume, maxVolume, PlayerPrefs.GetFloat(MasterGroupName));
        else
            return maxVolume;
    }

    //BGM
    public void SetBGMVolume(float currentVolume)
    {
        currentVolume = Mathf.Clamp01(currentVolume);
        float volum = Mathf.Lerp(minVolume, maxVolume, currentVolume);
        mixer.SetFloat(BGMGroupName, volum);
        PlayerPrefs.SetFloat(BGMGroupName, volum);
    }

    public float GetBGMVolume()
    {
        if (PlayerPrefs.HasKey(BGMGroupName))
            return Mathf.Lerp(minVolume, maxVolume, PlayerPrefs.GetFloat(BGMGroupName));
        else
            return maxVolume;
    }

    //UI
    public void SetUIVolume(float currentVolume)
    {
        currentVolume = Mathf.Clamp01(currentVolume);
        float volum = Mathf.Lerp(minVolume, maxVolume, currentVolume);
        mixer.SetFloat(UIGroupName, volum);
        PlayerPrefs.SetFloat(UIGroupName, volum);
    }

    public float GetUIVolume()
    {
        if (PlayerPrefs.HasKey(UIGroupName))
            return Mathf.Lerp(minVolume, maxVolume, PlayerPrefs.GetFloat(UIGroupName));
        else
            return maxVolume;
    }

    //Effect
    public void SetEffectVolume(float currentVolume)
    {
        currentVolume = Mathf.Clamp01(currentVolume);
        float volum = Mathf.Lerp(minVolume, maxVolume, currentVolume);
        mixer.SetFloat(EffectGroupName, volum);
        PlayerPrefs.SetFloat(EffectGroupName, volum);
    }

    public float GetEffectVolume()
    {
        if (PlayerPrefs.HasKey(EffectGroupName))
            return Mathf.Lerp(minVolume, maxVolume, PlayerPrefs.GetFloat(EffectGroupName));
        else
            return maxVolume;
    }

    #endregion


    #region Sound Mute, Stop, Resume, Start 

    public void AllSoundDoMute()
    {
        DoMuteBGM();
        DoMuteEffect();
        DoMuteUI();
    }

    public void AllSoundDontMute()
    {
        DontMuteBGM();
        DontMuteEffect();
        DontMuteUI();
    }

    public void StopBGM()
    {
        bgmAudioSourceA.Stop();
        bgmAudioSourceB.Stop();
    }

    public void ResumeBMG()
    {
        bgmAudioSourceA.Play();
        bgmAudioSourceB.Play();
    }

    public void DoMuteBGM()
    {
        bgmAudioSourceA.mute = true;
        bgmAudioSourceB.mute = true;
    }

    public void DontMuteBGM()
    {
        bgmAudioSourceA.mute = false;
        bgmAudioSourceB.mute = false;
    }

    public void StopEffect()
    {
        for (int i = 0; i < effectAudioSource.Length; i++)
        {
            effectAudioSource[i].Stop();
        }
    }

    public void ResumeEffect()
    {
        for (int i = 0; i < effectAudioSource.Length; i++)
        {
            effectAudioSource[i].Play();
        }
    }

    public void DoMuteEffect()
    {
        for (int i = 0; i < effectAudioSource.Length; i++)
        {
            effectAudioSource[i].mute = true;
        }
    }

    public void DontMuteEffect()
    {
        for (int i = 0; i < effectAudioSource.Length; i++)
        {
            effectAudioSource[i].mute = false;
        }
    }

    public void DoMuteUI()
    {
        for (int i = 0; i < uiAudioSource.Length; i++)
            uiAudioSource[i].mute = true;

    }

    public void DontMuteUI()
    {
        for (int i = 0; i < uiAudioSource.Length; i++)
            uiAudioSource[i].mute = false;
    }
    #endregion


    #region BGM Play Options
    public void PlayBGM_CrossFade(SoundList list, float fadeTime)
    {
        Debug.Log("BGM : " + fadeTime);
        CheckAndSetClip(list);

        if (bgmAudioSourceA.isPlaying)
        {
            StartCoroutine(FadeOut_Decrease(bgmAudioSourceA, fadeTime));
            StartCoroutine(FadeIn_Increase(currentBGMClip, bgmAudioSourceB, fadeTime));
        }
        else if (bgmAudioSourceB.isPlaying)
        {
            StartCoroutine(FadeOut_Decrease(bgmAudioSourceB, fadeTime));
            StartCoroutine(FadeIn_Increase(currentBGMClip,bgmAudioSourceA, fadeTime));
        }
        else
        {
            StartCoroutine(FadeIn_Increase(currentBGMClip,bgmAudioSourceA, fadeTime));
        }
    }


    public void PlayBGM_FadeInAndFadeOut(SoundList list, float fadeOutTime, float fadeInTime)
    {
        CheckAndSetClip(list);

        if (bgmAudioSourceA.isPlaying)
        {
            //페이드 B로
            bgmAudioSourceB.clip = currentBGMClip.clip;
            bgmAudioSourceB.loop = currentBGMClip.isLoop;
            bgmAudioSourceB.Play();
            StartCoroutine(FadeInAndOut(currentBGMClip, bgmAudioSourceA, bgmAudioSourceB, fadeOutTime, fadeInTime));

        }
        else if (bgmAudioSourceB.isPlaying)
        {
            //페이드 A로 
            bgmAudioSourceA.clip = currentBGMClip.clip;
            bgmAudioSourceA.loop = currentBGMClip.isLoop;
            bgmAudioSourceA.Play();
            StartCoroutine(FadeInAndOut(currentBGMClip, bgmAudioSourceB, bgmAudioSourceA, fadeOutTime, fadeInTime));

        }
    }

    public void PlayBGM_FadeIn_Increase(float fadeTime)
    {
        if (bgmAudioSourceA.isPlaying)
        {
            StartCoroutine(FadeIn_Increase(currentBGMClip,bgmAudioSourceA, fadeTime));
        }
        else if (bgmAudioSourceB.isPlaying)
        {
            StartCoroutine(FadeIn_Increase(currentBGMClip,bgmAudioSourceB, fadeTime));
        }
    }

    public void PlayBGM_FadeOut_Decrease(float fadeTime)
    {
        if (bgmAudioSourceA.isPlaying)
        {
            StartCoroutine(FadeOut_Decrease(bgmAudioSourceA, fadeTime));
        }
        else if (bgmAudioSourceB.isPlaying)
        {
            StartCoroutine(FadeOut_Decrease(bgmAudioSourceB, fadeTime));
        }
    }

    public void PlayBGM_Immediately(SoundList list)
    {
        CheckAndSetClip(list);

        if (bgmAudioSourceA.isPlaying)
        {
            //페이드 B로
            bgmAudioSourceA.clip = null;
            bgmAudioSourceA.clip = currentBGMClip.clip;
            bgmAudioSourceA.Play();
        }
        else if (bgmAudioSourceB.isPlaying)
        {
            //페이드 A로 
            bgmAudioSourceB.clip = null;
            bgmAudioSourceB.clip = currentBGMClip.clip;
            bgmAudioSourceB.Play();
        }
    }

    private void CheckAndSetClip(SoundList list)
    {
        if (currentBGMClip != null)
            lastBGMClip = currentBGMClip;

        currentBGMClip = soundData.GetSoundClip((int)list);
        currentClip = currentBGMClip.clip;
        Debug.Log("current : " + currentBGMClip?.clipName);
    }

    public void PlaySoundAtPosition(AudioClip clip, Transform position)
    {
        AudioSource.PlayClipAtPoint(clip, position.position);
    }
    public void PlaySoundAtPosition(SoundList soundList, Transform position)
    {
        AudioSource.PlayClipAtPoint(soundData.GetSoundClip((int)soundList).clip, position.position);
    }
    public void PlaySoundAtPosition(UISoundType type, Transform position)
    {
        UISoundInfo info = uiSounds.GetInfo(type);
        AudioSource.PlayClipAtPoint(soundData.GetSoundClip((int)info.sound).clip, position.position);
    }

    IEnumerator FadeInAndOut(SoundClip clip, AudioSource fadeOut, AudioSource fadeIn, float fadeOutTime, float fadeInTime)
    {
        fadeIn.volume = 0;
        yield return StartCoroutine(FadeOut_Decrease(fadeOut, fadeOutTime));
        yield return StartCoroutine(FadeIn_Increase(clip,fadeIn, fadeInTime));
    }

    IEnumerator FadeIn_Increase(SoundClip clip,AudioSource audio, float fadeTime)
    {
        audio.clip = currentClip;
        audio.loop = clip.isLoop;
        audio.Play();
        Debug.Log(audio.name + " - " + "FadeIn_Increase _" + currentBGMClip.clip);
        bool isReached = false;
        float currentTime = 0f;
        float volume = clip.volume;

        while (!isReached)
        {
            currentTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(0, volume, currentTime / fadeTime);

            if (audio.volume >= volume)
                isReached = true;

            yield return null;
        }
    }

    IEnumerator FadeOut_Decrease(AudioSource audio, float fadeTime)
    {
        Debug.Log(audio.name + " - " + "FadeOut_Decrease");

        bool isReached = false;
        float currentTime = 0f;

        while (!isReached)
        {
            currentTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(1, 0, currentTime / fadeTime);

            if (audio.volume <= 0)
                isReached = true;

            yield return null;
        }

        yield return null;
    }

    #endregion


    #region Effect & UI Play
    public void PlayExtraSound(UISoundType uiSoundtype)
    {
        UISoundInfo info = uiSounds.GetInfo(uiSoundtype);
        PlaySoundProcess(soundData.GetSoundClip((int)info.sound), extraAudioSource);
    }
    public void PlayExtraSound(SoundList soundList)
    {
        PlaySoundProcess(soundData.GetSoundClip((int)soundList), extraAudioSource);
    }
    public void PlayExtraSound(SoundClip clip)
    {
        PlaySoundProcess(clip, extraAudioSource);
    }

    public void PlayUISound(UISoundType uiSoundtype)
    {
        UISoundInfo info = uiSounds.GetInfo(uiSoundtype);
        PlaySoundProcess(soundData.GetSoundClip((int)info.sound), uiAudioSource);
    }

    public void PlayUISound(SoundList list)
    {
        if (list == SoundList.None) return;
        PlaySoundProcess(soundData.GetSoundClip((int)list), uiAudioSource);
    }

    private void PlaySoundProcess(SoundClip clip, AudioSource[] sources)
    {
        if (clip == null || sources == null || sources.Length <= 0)
            return;

        bool isPlay = false;

        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].isPlaying)
            {
                sources[i].volume = clip.volume;
                sources[i].clip = clip.clip;
                sources[i].Play();
                isPlay = true;
               // Debug.Log("재생 0 : " + clip.clipName + " _ " + clip.volume);
                break ;

            }
        }

        if (!isPlay)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].clip == clip.clip)
                {
                    sources[i].Stop();
                    sources[i].volume = clip.volume;
                    sources[i].clip = clip.clip;
                    sources[i].Play();
                    isPlay = true;
                 //   Debug.Log("재생 1 : " + clip.clipName + " _ " + clip.volume);
                    break;
                }
            }
        }

        if (!isPlay)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if (!sources[i].isPlaying || (sources[i].isPlaying && sources[i].time >= sources[i].clip.length * 0.7f))
                {
                    sources[i].Stop();
                    sources[i].clip = clip.clip;
                    sources[i].volume = clip.volume;
                    sources[i].Play();
                   // Debug.Log("재생 2 : " + clip.clipName + " _ " + clip.volume);
                    isPlay = true;
                    break;
                }
            }
        }

        if (!isPlay)
        {
            AudioSource source = GetLongestPlayedAudioSource(sources);
            if(source == null)
            {
                Debug.Log("재생 3 : NULL ");
                return;
            }
            source.Stop();
            source.clip = clip.clip;
            source.Play();
          //  Debug.Log("재생 3 : " + clip.clipName + " _ " + clip.volume);
        }

    }

    public AudioSource GetLongestPlayedAudioSource(AudioSource[] audioSources)
    {
        if (audioSources == null || audioSources.Length == 0)
        {
            return null;
        }

        AudioSource longestPlayedSource = null;
        float longestPlayTime = 0f;

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                // 현재 오디오의 재생 시간
                float currentPlayTime = audioSource.time;
                if (currentPlayTime > longestPlayTime)
                {
                    longestPlayTime = currentPlayTime;
                    longestPlayedSource = audioSource;
                }
            }
        }

        return longestPlayedSource;
    }

    public void PlayOneShot(SoundList list)
    {
        if (list == SoundList.None) return;

        AudioClip uiSound = soundData.GetSoundClip((int)list).clip;
        oneShotAudioSource.PlayOneShot(uiSound);
    }

    public void PlayEffect(SoundList[] randomList)
    {
        if (randomList == null || randomList.Length <= 0) return;

        int random = Random.Range(0, randomList.Length);
        PlayEffect(randomList[random]);
    }

    public void PlayEffect(SoundList list)
    {
        if (list == SoundList.None) return;

        SoundClip soundClip = soundData.GetSoundClip((int)list);
        PlaySoundProcess(soundClip, effectAudioSource);
    }



    private bool CheckIsPlayingEffectSound(AudioClip clip)
    {
        bool isPlaying = false;
        for (int i = 0; i < effectAudioSource.Length; i++)
            if (effectAudioSource[i].isPlaying && effectAudioSource[i].clip == clip)
                isPlaying = true;

        return isPlaying;
    }

    #endregion
}


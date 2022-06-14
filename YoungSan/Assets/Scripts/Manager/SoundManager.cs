using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager
{
    public AudioMixer mixer;
    Hashtable SoundTable { get; set; }

    Hashtable InstanceSounds { get; set; }

    Hashtable MusicTable { get; set; }
    AudioSource bgm;

    // Start is called before the first frame update
    void Awake()
    {
        SoundTable = new Hashtable();
        InstanceSounds = new Hashtable();
        MusicTable = new Hashtable();
        bgm = GetComponent<AudioSource>();
        LoadMusics();
        LoadSounds();
    }

    public void SetBgm(string name)
    {
        if (!MusicTable.Contains(name) || name == string.Empty)
        {
            bgm.clip = null;
        }
        else
        {
            bgm.clip = MusicTable[name] as AudioClip;
            bgm.Play();
        }
    }
    private void LoadMusics()
    {
        AudioClip[] prefabs = Resources.LoadAll<AudioClip>("Music");
        foreach (AudioClip item in prefabs)
        {
            //print(item + item.name);
            MusicTable.Add(item.name, item);
        }
    }

    private void LoadSounds()
    {
        AudioClip[] prefabs = Resources.LoadAll<AudioClip>("Sounds");
        foreach (AudioClip item in prefabs)
        {
            //print(item + item.name);
            SoundTable.Add(item.name, item);
        }
    }
    private AudioClip GetSound(string name)
    {
        if (SoundTable.ContainsKey(name))
        {
            return (AudioClip)SoundTable[name];
        }
        else
        {
            return null;
        }
    }

    public object SoundStart(string soundName, Transform soundPos, bool is3DSound = true)
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        AudioSource audioSource = poolManager.GetObject("SoundPrefab").GetComponent<AudioSource>();
        return (StartCoroutine(SoundPlayCoroutine(soundName, soundPos, is3DSound, audioSource)), audioSource);
    }

    private IEnumerator SoundPlayCoroutine(string soundName, Transform soundPos, bool is3DSound, AudioSource audioSource)
    {
        audioSource.gameObject.transform.position = soundPos.position;
        AudioClip clip = GetSound(soundName);

        audioSource.spatialBlend = is3DSound ? 1 : 0;

        //audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        //audioSource.volume = 0.5f;
        audioSource.Play();

        float timeStack = clip.length;
        while (timeStack >= 0)
        {
            timeStack -= Time.deltaTime;
            audioSource.gameObject.transform.position = soundPos.position;
            yield return null;
        }
        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
    }

    public void SoundStop(object soundData)
    {
        var coroutine = ((Coroutine, AudioSource))soundData;
        if (coroutine.Item1 != null)
        {
            StopCoroutine(coroutine.Item1);
            coroutine.Item2.Stop();
            coroutine.Item2.gameObject.SetActive(false);
        }
    }


}

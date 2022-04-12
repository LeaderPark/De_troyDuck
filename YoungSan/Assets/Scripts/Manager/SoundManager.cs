using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager
{
    public AudioMixer mixer;
    Hashtable SoundTable { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SoundTable = new Hashtable();
        LoadSounds();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SoundStart(string soundName, Transform soundPos,bool is3DSound = true)
    {
        StartCoroutine(SoundPlayCoroutine(soundName,soundPos, is3DSound));
    }
    private IEnumerator SoundPlayCoroutine(string soundName, Transform soundPos, bool is3DSound)
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        AudioSource audioSource = poolManager.GetObject("SoundPrefab").GetComponent<AudioSource>();
        audioSource.gameObject.transform.position = soundPos.position;
        audioSource.gameObject.transform.SetParent(soundPos);
        AudioClip clip = GetSound(soundName);

        audioSource.spatialBlend = is3DSound ? 1 : 0;

        //audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        //audioSource.volume = 0.5f;
        audioSource.Play();

        yield return new WaitForSeconds(clip.length);
        audioSource.Stop();
        audioSource.gameObject.transform.SetParent(poolManager.gameObject.transform);
        audioSource.gameObject.SetActive(false);
    }
}

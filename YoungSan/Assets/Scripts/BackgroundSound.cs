using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
	public void BGSoundStart(AudioClip BGM)
	{
		SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
		soundManager.SoundStart(BGM.name, transform, false);
	}
}

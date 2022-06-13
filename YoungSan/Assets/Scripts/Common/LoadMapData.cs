using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapData : MonoBehaviour
{
	public Quest quest;
	public bool active;

	private void Awake()
	{
		if (quest.clear)
		{
			SceneManager sceneManager = ManagerObject.Instance.GetManager(ManagerType.SceneManager) as SceneManager;
			sceneManager.afterSceneLoadAction += () => gameObject.SetActive(active);
		}
	}
}

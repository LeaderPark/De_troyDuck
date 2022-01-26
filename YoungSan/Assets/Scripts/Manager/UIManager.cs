using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	Entity playerEntity;
	public Text hpText;
	public GameObject gmaeOverPanel;

	private void Update()
	{
		playerEntity = FindObjectOfType<Player>()?.gameObject.GetComponent<Entity>();
		if (playerEntity != null)
		{
			hpText.text = "Hp : " + playerEntity.clone.GetStat(StatCategory.Health).ToString();
		}
		if (FindObjectOfType<Player>() == null)
		{
			GameOver();
		}
	}
	private void GameOver()
	{
		gmaeOverPanel.SetActive(true);
	}
	public void GotoMainTitle()
	{
		SceneManager.LoadScene("Title");
	}
}

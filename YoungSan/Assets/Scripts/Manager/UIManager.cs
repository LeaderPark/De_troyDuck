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
	public void FindPlayer()
	{
		playerEntity = FindObjectOfType<Player>().gameObject.GetComponent<Entity>();
	}
	private void Update()
	{
		if (playerEntity != null)
		{
			hpText.text = "Hp : "+playerEntity.clone.GetStat(StatCategory.Health).ToString();
			if (playerEntity.clone.GetStat(StatCategory.Health) <= 0)
			{
				GameOver();
			}
		}
		else
		{
			FindPlayer();
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

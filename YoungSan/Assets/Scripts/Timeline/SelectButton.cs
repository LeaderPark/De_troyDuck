using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
	[SerializeField]
	private GameObject buttonsParent;
	public List<Button> buttons = new List<Button>();
	public Image selectImage;
	private int selectIdx = 0;

	private void Awake()
	{
		for (int i = 0; i < buttonsParent.transform.childCount; i++)
		{
			buttons.Add(buttonsParent.transform.GetChild(i).GetComponent<Button>());
		}
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			selectIdx = (selectIdx + 1) % buttons.Count;
			MoveSelect();
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			selectIdx = (selectIdx - 1) < 0 ? buttons.Count - 1 : selectIdx - 1% buttons.Count;
			MoveSelect();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			buttons[selectIdx].onClick.Invoke();
		}
	}
	public void MoveSelect()
	{
		selectImage.transform.position = buttons[selectIdx].transform.position;
	}
	//public void ButtonCountSetting()
	//{
		
	//}
	public void ButtonsSetting(int buttonIdx,string text,Action onclickFunc)
	{
		Text btnText = buttons[buttonIdx].GetComponentInChildren<Text>();
		btnText.text = text;
		buttons[buttonIdx].onClick.AddListener(() => onclickFunc()); ;
		buttons[buttonIdx].onClick.AddListener(()=>gameObject.SetActive(false)); ;

	}
	private void ButtonsInit()
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			buttons[i].onClick.RemoveAllListeners();
		}
	}
}

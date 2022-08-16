using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator animator;
	private Collider doorCol;
	private void Awake()
	{
		animator = GetComponent<Animator>();
		doorCol = GetComponent<Collider>();
	}
	public void OpenDoor()
	{
		animator.Play("DoorOpen");
		doorCol.enabled = false;
	}
	public void CloseDoor()
	{
		animator.Play("CloseDoor");
		doorCol.enabled = true;
	}
}

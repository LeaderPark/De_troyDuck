using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamIn : MonoBehaviour
{
	public CinemachineVirtualCamera changeCam;
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
			changeCam.Priority = 20;
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			changeCam.Priority = 0;
	}
}

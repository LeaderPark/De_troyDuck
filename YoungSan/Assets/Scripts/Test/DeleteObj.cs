using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObj : MonoBehaviour
{
	private void Awake()
	{
		Destroy(gameObject);
	}
}

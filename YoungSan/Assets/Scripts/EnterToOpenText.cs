using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterToOpenText : MonoBehaviour
{
	public TextMesh mesh;
	float time = 0;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StopAllCoroutines();
			StartCoroutine(OpenText(true));
		}

	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StopAllCoroutines();
			StartCoroutine(OpenText(false));
		}
	}
	IEnumerator OpenText(bool on)
	{
		Debug.Log("A");
		float val = on ? 1 : -1;
		//time = Mathf.Clamp(-val, 0, 1);
		time = mesh.color.a;
		while (true)
		{
			time += Time.deltaTime * val;
			mesh.color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, Mathf.Lerp(0, 1, time));
			if (!on)
			{
				if (time <= 0)
				{
					time = 0;
					yield break;
				}
			}
			else
			{
				if (time >= 1)
				{
					time = 1;
					yield break;
				}

			}
			yield return null;

		}
	}
}

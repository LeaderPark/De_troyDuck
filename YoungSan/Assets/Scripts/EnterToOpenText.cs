using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterToOpenText : MonoBehaviour
{
	public TextMesh mesh;
	float time = 0;
	public List<EntityData> entityDatas = new List<EntityData>();
	bool notEntityData;
	private void Awake()
	{
	}
	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Entity playerEntity = other.gameObject.GetComponent<Entity>();
			if (entityDatas.Count == 0)
			{
				StopAllCoroutines();
				StartCoroutine(OpenText(true));
			}
			else
			{
				foreach (var item in entityDatas)
				{
					if (playerEntity.entityData == item)
					{
						StopAllCoroutines();
						StartCoroutine(OpenText(true));
					}
				}
			}
			
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
	public bool entityCheck(EntityData playerEntity, EntityData checkEntity)
	{
		bool val;
		val = playerEntity == checkEntity;
		if (notEntityData)
			val = !val;
		return val;
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

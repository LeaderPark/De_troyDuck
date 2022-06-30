using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPTEST2 : MonoBehaviour
{
    public TextMeshProUGUI tmp;
	Vector3[] vertice;
	public int asd = 0;
	public string txt;

    // Start is called before the first frame update
    void Start()
    {
		//vertice = tmp.mesh.vertices;
		//asd = 0;
		//StartCoroutine(test());

	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Y))
		{
			Debug.Log(tmp.text.Length);
			Debug.Log(tmp.textInfo.characterCount);
			Debug.Log(tmp.mesh.vertices.Length/4);
			//Color32[] vertexColors = tmp.textInfo.meshInfo[0].colors32;

			//for (int i = 0; i < vertexColors.Length; i++)
			//{
			//	vertexColors[i].a = 0;
			//}
			//tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			tmp.text = txt;
			vertice = tmp.mesh.vertices;
			asd = 0;
			StartCoroutine(test());
		}
		if (Input.GetKeyDown(KeyCode.Space))
        {
			StartCoroutine(TextAnimation(asd, tmp, vertice));
			asd++;
			if (asd >= tmp.textInfo.characterCount-1)
				asd = 0;
        }
    }
	IEnumerator test()
	{
		yield return null;

		while (true)
		{
			yield return null;
			vertice = tmp.mesh.vertices;
			Debug.Log(tmp.mesh.vertexCount/4);
			StartCoroutine(TextAnimation(asd, tmp, vertice));
			asd++;
			//Debug.Log(asd);
			if (asd >= (vertice.Length / 4) - 1)
			{
				Debug.Log(asd + " , " + (vertice.Length / 4));
				yield break;
			}
			yield return new WaitForSeconds(0.01f);

		}

	}
	public IEnumerator TextAnimation(int idx, TextMeshProUGUI talkBox,Vector3[] origineVertice)
	{
		Mesh mesh = talkBox.mesh;
		//Debug.Log(mesh.vertices.Length / 4);
		Color32[] vertexColors = talkBox.textInfo.meshInfo[0].colors32;
		Vector3[] vertice = talkBox.mesh.vertices;

		float time = 0;

		if (vertice.Length < idx * 4)
			yield break;
		//talkBox.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		//mesh.vertices = vertice;
		//talkBox.canvasRenderer.SetMesh(mesh);
		while (true)
		{
			yield return null;
			vertice = talkBox.mesh.vertices;

			time += Time.deltaTime;
			for (int i = 0; i < 4; i++)
			{
				vertice[idx * 4 + i] = Vector3.Lerp(origineVertice[idx * 4 + i] - new Vector3(0, 20, 0), origineVertice[idx * 4 + i], time/0.1f);
				vertexColors[idx * 4 + i].a = (byte)Mathf.Lerp(0, 255, time / 0.1f);
			}

			talkBox.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
			mesh.vertices = vertice;
			talkBox.canvasRenderer.SetMesh(mesh);

			if (time > 0.1f)
			{
				time = 0.1f;
				for (int i = 0; i < 4; i++)
				{
					vertice[idx * 4 + i] = Vector3.Lerp(origineVertice[idx * 4 + i] - new Vector3(0, 20, 0), origineVertice[idx * 4 + i], time/0.1f);
					vertexColors[idx * 4 + i].a = (byte)Mathf.Lerp(0, 255, time / 0.1f);
				}
				talkBox.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
				mesh.vertices = vertice;
				talkBox.canvasRenderer.SetMesh(mesh);
				yield break;
			}
		}
	}
}

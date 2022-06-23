using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPTEST : MonoBehaviour
{
	public TMP_Text asdasd;
	TMP_CharacterInfo[] info;
	Mesh mesh;
	Vector3[] vertice;
	bool a = false;
	float time = 0;

	private void Start()
	{

	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			a = true;
		}
		if (a)
		{
			time += Time.deltaTime;

			//asdasd.ForceMeshUpdate();
			////asdasd.canvasRenderer.SetColor(Random.ColorHSV());
			////Debug.Log(asdasd.mesh.vertices.Length);
			////Debug.Log(asdasd.textInfo.characterInfo.Length);
			////for (int i = 0; i < asdasd.mesh.vertices.Length/4; i++)
			////{
			////	int meshIndex = asdasd.textInfo.characterInfo[i].materialReferenceIndex;
			////	Color32[] vertexColors = asdasd.textInfo.meshInfo[meshIndex].colors32;
			////	for (int j = 0; j < 4; j++)
			////	{
			////		vertexColors[i * 4 + j] = Color32.Lerp(new Color32(255, 255, 255, 100), new Color32(255, 255, 255, 0), time);
			////		//vertexColors[i * j + 2] = Random.ColorHSV();
			////		//vertexColors[i * j + 3] = Random.ColorHSV();
			////		//vertexColors[i * j + 4] = Random.ColorHSV();
			////	}

			////}
			//Vector3[] vertice = asdasd.mesh.vertices;
			//vertice[0] += new Vector3(0, Mathf.Sin(Time.deltaTime), 0);
			//vertice[1] -= new Vector3(0, Mathf.Sin(Time.deltaTime), 0);
			//vertice[2] += new Vector3(0, Mathf.Sin(Time.deltaTime), 0);
			//vertice[3] -= new Vector3(0, Mathf.Sin(Time.deltaTime), 0);
			//asdasd.mesh.SetVertices(vertice);
			//asdasd.ForceMeshUpdate();


			////	for (int i = 0; i < asdasd.textInfo.characterInfo.Length; i++)
			////{
			////	int meshIndex = asdasd.textInfo.characterInfo[i].materialReferenceIndex;
			////	Vector3[] vertice = asdasd.mesh.vertices;
			////	for (int j = 0; j < 4; j++)
			////	{
			////		vertice[i * j + 1] += new Vector3(1,0,0);
			////		vertice[i * j + 2] += new Vector3(0, 1, 0);
			////		vertice[i * j + 3] += new Vector3(0, 0, 1);
			////		vertice[i * j + 4] += Vector3.zero;
			////	}
			////	asdasd.mesh.SetVertices(vertice);

			////}
			//asdasd.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

		}
		Color32[] vertexColors = asdasd.textInfo.meshInfo[0].colors32;

		mesh = asdasd.mesh;
		vertice = asdasd.mesh.vertices;
		vertice[0] = new Vector3(-97.6f, 10, 0.0f);
		vertice[1] = new Vector3(-97.6f, 30, 0.0f);
		vertice[2] = new Vector3(-97.6f+20, 30, 0.0f);
		vertice[3] = new Vector3(-97.6f+20, 10, 0.0f);
		//vertice[1] += new Vector3(0, , 0);
		//vertice[2] += new Vector3(0, 15, 0);
		//vertice[3] += new Vector3(0, 10, 0);
		vertexColors[0] = Color32.Lerp(new Color32(255, 255, 255, 100), new Color32(255, 255, 255, 0),time);
		vertexColors[1] = Color32.Lerp(new Color32(255, 255, 255, 100), new Color32(255, 255, 255, 0), time);
		vertexColors[2] = Color32.Lerp(new Color32(255, 255, 255, 100), new Color32(255, 255, 255, 0), time);
		vertexColors[3] = Color32.Lerp(new Color32(255, 255, 255, 100), new Color32(255, 255, 255, 0), time);
		asdasd.UpdateVertexData(TMP_VertexDataUpdateFlags.All);


		//for (int i = 0; i < asdasd.mesh.vertices.Length/4; i++)
		//{
		//	for (int j = 0; j < 4; j++)
		//	{
		//		vertice[i * 4 + j] += new Vector3(0, i*2, 0);
		//	}
		//}
		mesh.vertices = vertice;
		asdasd.canvasRenderer.SetMesh(mesh);

	}
}
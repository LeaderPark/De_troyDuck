using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransperent : MonoBehaviour
{
    public Vector3 offset = Vector3.zero; 
    public float obstructionFadingSpeed = 2;
    public float transperentValue = 0.5f;

    [SerializeField] private HashSet<Transform> ObjectToHide = new HashSet<Transform>();
    [SerializeField] private HashSet<Transform> ObjectToShow = new HashSet<Transform>();

    List<Transform> Remove = new List<Transform>();
    public LayerMask layerMask;

    void Update()
    {
        BlockingCheck();
 
        foreach (var obstruction in ObjectToHide)
        {
            HideObstruction(obstruction);
        }
 
        foreach (var obstruction in ObjectToShow)
        {
            ShowObstruction(obstruction);
        }
        
        foreach (var obstruction in Remove)
        {
            ObjectToShow.Remove(obstruction);
        }
        Remove.Clear();
    }

    void BlockingCheck()
    {
        GameManager gm = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Vector3 playerPosition = gm.Player.transform.position + offset;
        float characterDistance = Vector3.Distance(transform.position, playerPosition);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, playerPosition - transform.position, characterDistance,layerMask);

        if (hits.Length > 0)
        {
            foreach (var obstruction in ObjectToHide)
            {
                ObjectToShow.Add(obstruction);
            }
 
            ObjectToHide.Clear();
 
            foreach (var hit in hits)
            {
                Transform obstruction = hit.transform;
                ObjectToHide.Add(obstruction);
                ObjectToShow.Remove(obstruction);
            }
        }
        else
        {
            foreach (var obstruction in ObjectToHide)
            {
                ObjectToShow.Add(obstruction);
            }
 
            ObjectToHide.Clear();
        }
    }

    private void HideObstruction(Transform obj)
    {
        SpriteRenderer[] renders = obj.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < renders.Length; i++)
		{
            Color color = renders[i].color;
            color.a = Mathf.Max(transperentValue, color.a - obstructionFadingSpeed * Time.deltaTime);
            renders[i].color = color;

        }
    }

    private void ShowObstruction(Transform obj)
    {
        SpriteRenderer[] renders = obj.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            Color color = renders[i].color;
            color.a = Mathf.Min(1, color.a + obstructionFadingSpeed * Time.deltaTime);
            renders[i].color = color;
            if (renders[i].color.a == 1)
                Remove.Add(obj);

        }
    }
}

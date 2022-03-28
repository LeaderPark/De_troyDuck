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
            // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
            foreach (var obstruction in ObjectToHide)
            {
                ObjectToShow.Add(obstruction);
            }
 
            ObjectToHide.Clear();
 
            // Hide the current obstructions
            foreach (var hit in hits)
            {
                Transform obstruction = hit.transform;
                ObjectToHide.Add(obstruction);
                ObjectToShow.Remove(obstruction);
            }
        }
        else
        {
            // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
            foreach (var obstruction in ObjectToHide)
            {
                ObjectToShow.Add(obstruction);
            }
 
            ObjectToHide.Clear();
        }
    }

    private void HideObstruction(Transform obj)
    {
        // obj.GetComponent<SpriteRenderer>().re
        Color color = obj.GetComponent<SpriteRenderer>().color;
        color.a = Mathf.Max(transperentValue, color.a - obstructionFadingSpeed * Time.deltaTime);
        obj.GetComponent<SpriteRenderer>().color = color;
 
    }

    private void ShowObstruction(Transform obj)
    {
        var color = obj.GetComponent<SpriteRenderer>().color;
        color.a = Mathf.Min(1, color.a + obstructionFadingSpeed * Time.deltaTime);
        obj.GetComponent<SpriteRenderer>().color = color;

        if (obj.GetComponent<SpriteRenderer>().color.a == 1f)
        {
            Remove.Add(obj);
        }
    }
}

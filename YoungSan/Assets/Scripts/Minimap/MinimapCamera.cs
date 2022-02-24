using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform target; //카메라가 따라갈 대상
    public Transform indicator;
    public float offesetRatio = 0; //target이 미니맵에 있는 위치 % (-1 ~ 1)

    Camera cam;
    Vector2 size;
    

    void Start()
    {
        cam = GetComponent<Camera>();
        size = new Vector2(cam.orthographicSize, cam.orthographicSize * cam.aspect);
    }

    void Update()
    {
        Vector3 tragetForwardVector = target.forward;
        tragetForwardVector.y = 0;
        tragetForwardVector.Normalize();

        Vector3 position = new Vector3(target.transform.position.x, 10, target.transform.position.z) + tragetForwardVector * offesetRatio * cam.orthographicSize;
        transform.position = position;
        transform.eulerAngles = new Vector3(90, 0, -target.eulerAngles.y);
    }

    public void ShowBorderIndicator(Vector3 position)
    {
        float reciprocal;
        float rotation;
        Vector2 distance = new Vector3(transform.position.x - position.x, transform.position.z - position.z);

        distance = Quaternion.Euler(0, 0, target.eulerAngles.y) * distance;

        // X axis
        if(Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            reciprocal = Mathf.Abs(size.x / distance.x);
            rotation = (distance.x > 0) ? 90 : -90;
        }
        // Y axis
        else
        {
            reciprocal = Mathf.Abs(size.y / distance.y);
            rotation = (distance.y > 0) ? 180 : 0;
        }

        indicator.localPosition = new Vector3(distance.x * -reciprocal, distance.y * -reciprocal, 1);
        indicator.localEulerAngles = new Vector3(0, 0, rotation);
    }

    public void HideBorderIncitator()
    {
       indicator.gameObject.SetActive(false);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LookAtMouse : MonoBehaviour
{
    private float speed = 10.0f;
    private Vector2 target;
    private Vector2 position;
    private Camera cam;
    public Transform Objet;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    

    void Start()
    {
        target = new Vector2(0.0f, 0.0f);
        position = gameObject.transform.position;
        cam = Camera.main;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, target, step);
    }
    void OnGUI()
    {
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();
        Vector2 point = new Vector2();

        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;
        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0.0f));          
        target = point;
    }
    public void SetEyeLimit(Vector2 minPos, Vector2 maxPos)
    {
        minPosition = minPos;
        maxPosition = maxPos;
    }
}
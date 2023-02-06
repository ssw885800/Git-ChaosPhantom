using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCast : MonoBehaviour
{
    GameManager gameManager;

    public Transform firePoint;
    public Transform player;
    public LineRenderer ReadyLine;
    public LineRenderer AtkLine;

    int layerMask = 1 << 10;
    Vector2 traget;
    Vector2 direction;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ReadyLine.enabled = false;
        AtkLine.enabled = false;
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        StartCoroutine(shoot());
    }
    public IEnumerator shoot()
    {
        direction = player.position - firePoint.position;
        traget = player.position;
        ReadyLine.enabled = true;
        ReadyLine.SetPosition(0, firePoint.position);
        ReadyLine.SetPosition(1, traget);
        RaycastHit2D Readyhit = Physics2D.Raycast(firePoint.position, direction.normalized);
        yield return new WaitForSeconds(1.0f);
        ReadyLine.enabled = false;
        AtkLine.enabled=true;
        AtkLine.SetPosition(0, firePoint.position);
        AtkLine.SetPosition(1, traget);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction.normalized,1000F,layerMask) ;
        Debug.Log(1);
        if (hit)
        {
            AtkLine.SetPosition(1, hit.point);
            Debug.Log(hit.collider.name);
            if(hit.collider.name == "Pc_Shadow")
            {
                gameManager.NowHp--;
            }
        }
        yield return new WaitForSeconds(0.5f);
        AtkLine.enabled = false;
    }
}

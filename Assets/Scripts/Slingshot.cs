using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    [Header("Set Dinamically")]
    public GameObject launchPoint;
    public Vector2 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody2D projectileRigidbody;

    static public Vector2 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector2.zero;
            return S.launchPos;
        }
    }

    private void Awake()
    {
        S = this;
        Transform launchpointTrans = transform.Find("LaunchPoint");
        launchPoint = launchpointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchpointTrans.position;
    }
    private void OnMouseEnter()
    {
        //print("Mouse enter");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("Mouse exit");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile);
        projectile.transform.position = launchPos;
        //projectile.GetComponent<Rigidbody2D>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDelta = mousePos - launchPos;
        float maxMagnitude = this.GetComponent<CircleCollider2D>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector2 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            GameController.ShotFired();
            ProjectileLine.S.poi = projectile;
        } 
    }
}

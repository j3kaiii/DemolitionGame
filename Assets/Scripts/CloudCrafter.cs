using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-30, 2, 30);
    public Vector3 cloudPosMax = new Vector3(150, 80, 30);
    public float cloudScaleMin = 0.2f;
    public float clodScaleMax = 3f;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

    private void Awake()
    {
        //массив дл€ всех облаков
        cloudInstances = new GameObject[numClouds];
        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;

        for (int i=0; i<numClouds; i++)
        {
            //создаем экземпл€р облака
            cloud = Instantiate<GameObject>(cloudPrefab);
            //выбираем место
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //масштабируем
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, clodScaleMax, scaleU);
            //маленькие облака ниже и дальше
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(anchor.transform);
            cloudInstances[i] = cloud;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            if (cPos.x <= cloudPosMin.x)
            {
                cPos.x = cloudPosMax.x;
            }

            cloud.transform.position = cPos;
        }
    }
}

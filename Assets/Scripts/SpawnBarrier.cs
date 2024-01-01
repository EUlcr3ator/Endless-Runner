using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SpawnBarrier : MonoBehaviour
{
    public GameObject gameObject;
    private List<float> randomRange = new List<float> { -3f, 0.001f, 3f };
    int index;
    // Start is called before the first frame update
    void Start()
    {
        index = UnityEngine.Random.Range(0, randomRange.Count);
        //Vector3 target = new Vector3((float) Convert.ToDouble(randomRange[index]), 0f, 0f);
        //gameObject.transform.position += target;
        SetTransformX(randomRange[index]);
    }

    // Update is called once per frame
    void Update()
    {
        //SetTransformX(randomRange[index]);
    }

    void SetTransformX(float n)
    {
        gameObject.transform.position = new Vector3(n, transform.position.y, transform.position.z);
        Debug.Log(gameObject.transform.position.x);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    float paralaxPercent = 0.9f;
    float startY;
    float startCamY;

    void Start()
    {
        startY = transform.position.y;
        startCamY = Camera.main.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float diff = Camera.main.transform.position.y - startCamY;
        pos.y = startY + diff * paralaxPercent;
        transform.position = pos;
    }
}

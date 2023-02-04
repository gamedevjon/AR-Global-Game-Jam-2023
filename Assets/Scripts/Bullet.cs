using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Vector3 dir = Vector3.forward;
    public float speed = 1f;
    public float dist = 2f;
    public float distTraveled = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaDist = speed * Time.deltaTime;
        transform.position += dir * deltaDist;
        distTraveled += deltaDist;
        if(distTraveled > dist)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Vector3 dir = Vector3.forward;
    public float speed = 1f;
    public float dist = 2f;
    public float distTraveled = 0f;
    
    // Update is called once per frame
    void Update()
    {
        float deltaDist = speed * Time.deltaTime;
        //transform.position += dir * deltaDist;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        distTraveled += deltaDist;
        if(distTraveled > dist)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
            other.GetComponent<IDamagable>()?.Damage(5);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtNearestByTag : MonoBehaviour
{

    public GameObject target;
    public float targetDist = 0f;
    public string lookTag = "Enemy";
    public float lookSpeed = 5f;
    public float lookDist = 2f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        // Find nearest target
        GameObject[] candidates = GameObject.FindGameObjectsWithTag(lookTag);
        float distNearest = float.PositiveInfinity;
        target = null;
        foreach (GameObject candidate in candidates)
        {
            Vector3 pos = candidate.transform.position - transform.position;
            float dist = pos.magnitude;
            if(dist < lookDist)
            {
                if (dist < distNearest)
                {
                    distNearest = dist;
                    target = candidate;
                    targetDist = dist;
                }
            }
        }

        // Look at target
        if(target)
        {

            // Look direction
            Vector3 lookPos = target.transform.position - transform.position;
            lookPos.y = 0f;

            // Smoothly rotate towards the target point
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);

        }

    }

}

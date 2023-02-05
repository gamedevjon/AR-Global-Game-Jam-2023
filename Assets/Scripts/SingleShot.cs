using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float fireDist = 1.5f;

    public float T = 2f;  // inter-burst interval
    public int B = 5;  // number of burst-instances per burst
    public float BT = 0.1f;  // inter-burst-instance period

    private bool shouldFireFlag = false;
    private bool shouldBurstFlag = false;

    private bool fireFlag = true;
    private float t = 0;  // inter-burst
    private int b = 0;  // bursts
    private float bt = 0f;  // per-burst-instance timer

    private LookAtNearestByTag lookAtNearestByTag;

    // Start is called before the first frame update
    void Start()
    {
        lookAtNearestByTag = GetComponent<LookAtNearestByTag>();
    }

    // Update is called once per frame
    void Update()
    {

        // Facing vector
        Vector3 forward = transform.rotation * Vector3.forward;

        // Should fire?
        if(lookAtNearestByTag)
        {
            if(lookAtNearestByTag.target && lookAtNearestByTag.targetDist <= fireDist)
            {
                shouldFireFlag = true;
            }
            else
            {
                shouldFireFlag = false;
            }
        }
        
        // Burst fire manager
        if(shouldFireFlag)
        {
            shouldBurstFlag = true;
        }

        if(shouldBurstFlag)
        {
            if (b < B && bt < 0)
            {
                fireFlag = true;
                b++;
                bt = BT;
            }
            if(b >= B)
            {
                shouldBurstFlag = false;
            }
        }

        // Inter-burst reset
        if(t < 0)
        {
            b = 0;
            bt = 0;
            t = T;
        }

        // Fire
        if(fireFlag)
        {

            fireFlag = false;

            if(bulletPrefab)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Bullet bulletBullet = bullet.GetComponent<Bullet>();
                bulletBullet.dir = forward;
            }

        }

        // Timers
        bt -= Time.deltaTime;
        if(shouldFireFlag || t < T)
        {
            t -= Time.deltaTime;
        }

    }

}

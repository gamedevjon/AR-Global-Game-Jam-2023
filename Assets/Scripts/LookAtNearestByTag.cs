using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtNearestByTag : MonoBehaviour, IDamagable
{

    public GameObject target;
    public string lookTag = "Enemy";
    public float lookSpeed = 5f;
    public float lookDist = 2f;
    [SerializeField] private int _maxHealth;
    private Slider _healthSlider;
    [SerializeField] private GameObject _healthBarCanvas;
    [SerializeField] float _attackTime = .5f;
    [SerializeField] int _burstAmount = 5;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] float _attackRadius = 1f;
    bool _isFiring = false;
    Quaternion targetRotation;
    float _canFire = -1;

    public int Health { get; set; }

    public void Damage(int DamageAmount)
    {
        if (!_healthBarCanvas.activeInHierarchy)
            _healthBarCanvas.SetActive(true);

        Health -= DamageAmount;
        _healthSlider.SetValueWithoutNotify(Health);

        if (Health <= 0)
            OnDefeat();
    }

    public void OnDefeat()
    {
        Destroy(this.gameObject);
    }

    [ContextMenu("HitTest")]
    public void HitTest()
    {
        Damage(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        _healthBarCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _healthSlider = _healthBarCanvas.GetComponentInChildren<Slider>();
        Health = _maxHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _maxHealth;
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
                }
            }
        }

        // Look at target
        if(target)
        {
            // Look direction
            Vector3 lookPos = Vector3.Normalize(target.transform.position - transform.position);
            //lookPos.y = 0f;
            
            // Smoothly rotate towards the target point
            targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);

        }

        if (target!= null)
        {
            float distance = Vector3.Magnitude(target.transform.position - transform.position);
            if  (distance <= _attackRadius && !_isFiring && Time.time > _canFire)
            {
                StartCoroutine(BurstFire());
            }

        }
    }

    IEnumerator BurstFire()
    {
        int burstCount = 0;
        _isFiring= true;
        while (burstCount < _burstAmount)
        {
            yield return new WaitForSeconds(.1f);
            burstCount++;

            Vector3 direction = target.transform.position - transform.position;
           

            Instantiate(_bulletPrefab, transform.position, Quaternion.LookRotation(direction));
        }
        _canFire = Time.time + _attackTime;


        _isFiring= false;
    }

}

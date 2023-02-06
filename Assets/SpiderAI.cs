using System;
using UnityEngine;
using UnityEngine.UI;

public class SpiderAI : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _attackDistance = 1.5f;
    [SerializeField]
    private float _searchRadius = 5f;
    private Transform _defaultTarget;
    private Collider[] _hits = new Collider[10];
    [SerializeField]
    private LayerMask _detectionLayers;
    [SerializeField]
    private int _health = 100;
    [SerializeField]
    private GameObject _healthBarCanvas;
    private Slider _healthSlider;

    private enum SpiderState
    {
        Idle,
        Walk,
        Jump,
        Attack,
        Die
    }

    [SerializeField]
    private SpiderState _currentState;
    private float _canAttack = -1;
    [SerializeField]
    private float _attackDelay = 3.0f;
    private Animator _anim;
    private bool _isDead = false;

    public int Health { get=>_health; set => _health = value; }

    private void Start()
    {
        _healthBarCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _healthSlider = _healthBarCanvas.GetComponentInChildren<Slider>();
        Health = _health;
        _healthSlider.maxValue = _health;
        _healthSlider.value = _health;
        _anim = GetComponentInChildren<Animator>();
        _currentState = SpiderState.Walk;
        _anim.Play("Walk");
        _defaultTarget = GameObject.Find("Stash").GetComponent<Transform>();
        SearchForTarget();
    }

    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case SpiderState.Idle:
                break;
            case SpiderState.Walk:
                if (_target == null)
                    return;

                transform.LookAt(_target.position);
                transform.Translate(Vector3.forward * _speed * Time.deltaTime);

                var distance = Vector3.Distance(transform.position, _target.position);

                if (distance < _attackDistance)
                {
                    _currentState = SpiderState.Attack;
                }

                break;
            case SpiderState.Jump:
                break;
            case SpiderState.Attack:
                
                if (Time.time > _canAttack)
                {//check if target is actually alive still 
                    var validTarget = _target.GetComponent<IDamagable>()?.Health > 0;

                    if (validTarget)
                    {
                        _target.GetComponent<IDamagable>().Damage(1);
                        _canAttack = Time.time + _attackDelay;
                        _anim.SetTrigger("Attack");
                    }
                    else
                        SearchForTarget();
                }

                break;
            case SpiderState.Die:
                
                if (_isDead == false)
                {
                    _isDead = true;
                    _anim.SetTrigger("Dead");
                    Destroy(this.gameObject, 5.0f);
                }



                break;
        }
    }

    private void SearchForTarget()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, _hits, _detectionLayers, QueryTriggerInteraction.Collide);

        _target = FindClosestTarget();
    }

    private Transform FindClosestTarget()
    {
        float closestDistance = 1000;
        Transform closestTarget = null;
        foreach(Collider hit in _hits)
        {
            
            if (hit == null || !hit.transform.CompareTag("Plant")) continue;
            if (hit.transform == this) continue;
            float distance = Vector3.Magnitude(hit.transform.position - transform.position);
            if (distance < closestDistance) 
            {
                closestDistance = distance;
                closestTarget = hit.transform;
            }
        }
        if (closestTarget != null)
            return closestTarget;
        else return _defaultTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _searchRadius);
    }

    public void Damage(int DamageAmount)
    {
        Debug.Log("Hit");

        if (!_healthBarCanvas.activeInHierarchy)
            _healthBarCanvas.SetActive(true);

        Health -= DamageAmount;
        _healthSlider.SetValueWithoutNotify(Health);

        Health -= DamageAmount;

        if (Health < 1)
        {
            _currentState = SpiderState.Die;
            GameManager.Instance.AddScore(1);
            SpawnManager.Instance.KillEnemy();
        }

        SearchForTarget();
    }

    public void OnDefeat()
    {
        throw new NotImplementedException();
    }
}

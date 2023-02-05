using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAI : MonoBehaviour
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
    private void Start()
    {
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
                {
                    _canAttack = Time.time + _attackDelay;
                    _anim.SetTrigger("Attack");
                }

                break;
            case SpiderState.Die:
                break;
        }
    }

    private void SearchForTarget()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, _hits, _detectionLayers, QueryTriggerInteraction.Collide);

        _target =FindClosestTarget();
    }

    private Transform FindClosestTarget()
    {
        float closestDistance = 1000;
        Transform closestTarget = null;
        foreach(Collider hit in _hits)
        {
            if (hit == null) continue;

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
}

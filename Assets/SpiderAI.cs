using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAI : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private Transform _target;
   
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

                if (distance < 1.5f)
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
}

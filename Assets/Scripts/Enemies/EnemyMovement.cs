using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Pathfinding _pathfinding;
    private Vector3 _target;
    private Transform _player;

    [Header("Movement-----------")]
    [Range(0, 5)]
    [SerializeField] private float _movementSpeed;
    [Range(0, 1)]
    [SerializeField] private float _startupTime = 0.5f;
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed, _maxAcceleration;

    private float _movementInput;
    private Vector3 _movementVelocity;

    [Header("Rotation-----------")]
    [Range(0, 10)]
    [SerializeField] private float _rotationSpeed;

    [Header("Follow Player-----------")]
    [Range(0, 10)]
    [SerializeField] private float _stopDistance = 1;

    void Awake()
    {
        _pathfinding = gameObject.GetComponent<Pathfinding>();
    }

    void Start()
    {   
        _player = _pathfinding.end;
    }

    void Update()
    {
        if(_pathfinding.pathList.Count == 0) return;
        
        _target = _pathfinding.pathList[0].vPosition;
        transform.up = Vector3.Slerp(transform.up, _target - transform.position, Time.deltaTime * _rotationSpeed);

        if(Vector3.Distance(transform.position, _player.position) > _stopDistance) ChangeMovementInputValue(true);
        else ChangeMovementInputValue(false);

        Move();
    }

    void Move()
    {
        Vector3 desiredVelocity = new Vector3(0, _movementInput, 0) * _maxSpeed;
        float maxSpeedChange = _maxAcceleration * Time.deltaTime;

        if(_movementVelocity.y < desiredVelocity.y) _movementVelocity.y = Mathf.Min(_movementVelocity.y + maxSpeedChange, desiredVelocity.y);
        else if(_movementVelocity.y > desiredVelocity.y) _movementVelocity.y = Mathf.Max(_movementVelocity.y - maxSpeedChange, desiredVelocity.y);
        
        Vector3 displacement = _movementVelocity.y * Time.deltaTime * transform.up;
        transform.localPosition += displacement;
    }

    void ChangeMovementInputValue(bool increaseValue)
    {
        if(increaseValue) {if(_movementInput == 1) return;}
        else {if(_movementInput == 0) return;}

        float elapsedTime = 0;

        while(elapsedTime < _startupTime)
        {
            if(increaseValue) _movementInput = Mathf.Lerp(0, 1, elapsedTime/_startupTime);
            else _movementInput = Mathf.Lerp(1, 0, elapsedTime/_startupTime);
            elapsedTime += Time.deltaTime;
        }

        if(increaseValue) _movementInput = 1;
        else _movementInput = 0;
    }
}

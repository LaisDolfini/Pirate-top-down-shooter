using System.Collections;
using System.Collections.Generic;
using PirateTopDown.Enemies;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class ShotAttack : MonoBehaviour
{
    private BulletsManager _bulletsManager;
    private EnemyMovement _enemyMovement;
    private Life _enemyLife;
    private Transform _player;

    [SerializeField] [Range(0, 10)] private float _cooldown;
    private float _cooldownTimer;
    private float _characterHeight;

    void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyLife = GetComponent<Life>();
    }

    void OnEnable()
    {
        _cooldownTimer = 0;
    }

    void Update()
    {
        if(_enemyLife.died) return;
        if(!_enemyMovement.nearPlayer) return;

        _cooldownTimer += Time.deltaTime;

        if(_cooldownTimer < _cooldown) return;

        Vector3 playerDirection = _player.position - transform.position;
        _bulletsManager.InstantiateBullet(transform.position + (_characterHeight * transform.up), playerDirection);
        _cooldownTimer = 0;      
    }

    public void ShotAttackSetup(BulletsManager bulletsManager, float charactersHeight, Transform player)
    {
        _bulletsManager = bulletsManager;
        _characterHeight = charactersHeight;
        _player = player;
    }
}

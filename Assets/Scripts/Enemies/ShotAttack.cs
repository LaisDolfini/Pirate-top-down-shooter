using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class ShotAttack : MonoBehaviour
{
    [SerializeField] private BulletsManager _bulletsManager;
    private EnemyMovement _enemyMovement;
    [SerializeField] [Range(0, 10)] private float _cooldown;
    private float _cooldownTimer;
    private float _characterHeight;

    void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        if(!_enemyMovement.nearPlayer) return;

        _cooldownTimer += Time.deltaTime;

        if(_cooldownTimer < _cooldown) return;

        _bulletsManager.InstantiateBullet(transform.position + (_characterHeight * transform.up), transform.up);
        _cooldownTimer = 0;      
    }

    public void ShotAttackSetup(BulletsManager bulletsManager, float charactersHeight)
    {
        _bulletsManager = bulletsManager;
        _characterHeight = charactersHeight;
    }
}

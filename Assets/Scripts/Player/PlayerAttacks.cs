using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [Header("Required Components---------")]
    [SerializeField] private BulletsManager _playerBulletsManager;
    
    [Header("FrontalSingleShot Setup---------")]
    [SerializeField] [Range(0, 10)] private float _singleShotCooldown;
    private float _singleShotCooldownTimer;

    [Header("SideTripleShot Setup---------")]
    [SerializeField] [Range(0, 10)] private float _tripleShotCooldown;
    private float _tripleShotCooldownTimer;
    private float _playerHeight;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _playerHeight = spriteRenderer.bounds.extents.y;
    }
    
    void Update()
    {
        FrontalSingleShot();
        SideTripleShot();
    }

    void FrontalSingleShot()
    {
        if(Input.GetKey(KeyCode.E))
        {
            if(_singleShotCooldownTimer < _singleShotCooldown) return;

            _playerBulletsManager.InstantiateBullet(transform.position + (_playerHeight * transform.up), transform.up);
            _singleShotCooldownTimer = 0;
        }

        _singleShotCooldownTimer += Time.deltaTime;
    }

    void SideTripleShot()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            if(_tripleShotCooldownTimer < _tripleShotCooldown) return;

            _playerBulletsManager.InstantiateBullet(transform.position, transform.right);
            _playerBulletsManager.InstantiateBullet(transform.position + (_playerHeight * transform.up) , transform.right);
            _playerBulletsManager.InstantiateBullet(transform.position + (_playerHeight * -transform.up) , transform.right);

            _playerBulletsManager.InstantiateBullet(transform.position, -transform.right);
            _playerBulletsManager.InstantiateBullet(transform.position + (_playerHeight * transform.up) , -transform.right);
            _playerBulletsManager.InstantiateBullet(transform.position + (_playerHeight * -transform.up) , -transform.right);

            _tripleShotCooldownTimer = 0;
        }

        _tripleShotCooldownTimer += Time.deltaTime;
    }
}

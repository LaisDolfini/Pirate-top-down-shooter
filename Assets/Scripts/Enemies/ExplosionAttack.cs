using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAttack : MonoBehaviour
{
    [SerializeField] [Range(0,100)] private int _damage;
    private Life _life, _playerLife;

    void Awake()
    {
        _life = GetComponent<Life>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(_playerLife == null) _playerLife = other.gameObject.GetComponent<Life>();

            _playerLife.TookDamage(_damage);
            _life.Kill(false);
        }
    }
}

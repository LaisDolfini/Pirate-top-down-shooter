using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private int _bulletDamage;
    [SerializeField] [Range(0, 5)] private float _bulletLifeTime;
    private float _lifetimeTimer;
    [SerializeField] [Range(0, 10)] private int _bulletSpeed;
    [SerializeField] private string _bulletTargetTag;

    void OnEnable()
    {
        _lifetimeTimer = 0;
    }

    void Update()
    {
        _lifetimeTimer += Time.deltaTime;

        if(_lifetimeTimer >= _bulletLifeTime)
        {
            gameObject.SetActive(false);
            return;
        } 

        transform.localPosition += transform.up * _bulletSpeed * Time.deltaTime;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(_bulletTargetTag))
        {
            other.GetComponent<Life>().TookDamage(_bulletDamage);
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] [Range(0, 100)] private int _bulletDamage;
    [SerializeField] [Range(0, 5)] private float _bulletLifeTime;
    private float _lifetimeTimer;
    [SerializeField] [Range(0, 10)] private int _bulletSpeed;
    [SerializeField] private string _bulletTargetTag;
    private bool _hitted;

    void OnEnable()
    {
        _lifetimeTimer = 0;
    }

    void Update()
    {
        if(_hitted) return;

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
            if(_hitted) return;
            _hitted = true;
            other.GetComponent<Life>().TookDamage(_bulletDamage);
            StartCoroutine(DisableDelay());
        }
    }

    IEnumerator DisableDelay()
    {
        _explosion.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        _explosion.SetActive(false);
        _hitted = false;
        gameObject.SetActive(false);
    }
}

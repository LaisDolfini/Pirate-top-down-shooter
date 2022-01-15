using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;

    public enum TargetType
    {
        Player,
        Enemy
    }
    [SerializeField] private TargetType _targetType; 
    private Life _lastLifeGot;
    private int _lastId;
    [SerializeField] [Range(0, 100)] private int _bulletDamage;
    [SerializeField] [Range(0, 5)] private float _bulletLifeTime;
    private float _lifetimeTimer;
    [SerializeField] [Range(0, 10)] private int _bulletSpeed;
    private string _bulletTargetTag;
    private bool _hitted;

    void Start()
    {
        if(_targetType == TargetType.Player) _bulletTargetTag = "Player";
        else _bulletTargetTag = "Enemy";
    }

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

            int gameObjectId = other.gameObject.GetInstanceID();
            if(_lastId != gameObjectId) _lastLifeGot = null;

            if(_lastLifeGot == null) _lastLifeGot = other.GetComponent<Life>();
            _lastLifeGot.TookDamage(_bulletDamage);

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

using System.Collections;
using System.Collections.Generic;
using PirateTopDown.Enemies;
using PirateTopDown.Player;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{   
    [Header("Required Components----------")]
    [SerializeField] private Slider _lifeBar;
    [SerializeField] private GameObject _deathAnimation, _explosionAnimation;
    [SerializeField] private Sprite[] _shipFeedbackSprites;
    private SpriteRenderer _spriteRenderer;

    public enum CharacterType
    {
        Player,
        Enemy
    }
    [Header("Setup-------------")]
    [SerializeField] private CharacterType _characterType;

    [Range(0, 100)]
    [SerializeField] private int _maxLife;
    [Range(0, 5)]
    [SerializeField] private int _deathDelay;
    private int _currentLife;
    public bool died {get {return _died;}}
    private bool _died;

    private PlayerMovement _playerMovement;
    private EnemyMovement _enemyMovement;

    public delegate void OnCharacterDied();
    public OnCharacterDied onDied;

    void Awake()
    {
        _lifeBar.maxValue = _maxLife;
        _lifeBar.value = _maxLife;
        _currentLife = _maxLife;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        if(_characterType == CharacterType.Player) _playerMovement = GetComponent<PlayerMovement>();
        else _enemyMovement = GetComponent<EnemyMovement>();
    }

    void OnEnable()
    {
        if(_characterType == CharacterType.Enemy) Revive();
    }

    public void TookDamage(int damage, bool increaseScore = true)
    { 
        _currentLife -= damage;
        _lifeBar.value = _currentLife;
        ChangeShipSprite();

        if(_died) return;
        if(_currentLife <= 0) StartCoroutine(Died(increaseScore));
    }

    void ChangeShipSprite()
    {
        if(_currentLife <= 0) _spriteRenderer.sprite = _shipFeedbackSprites[3];
        else if(_currentLife <= _maxLife/3) _spriteRenderer.sprite = _shipFeedbackSprites[2];
        else if(_currentLife <= _maxLife/1.5f) _spriteRenderer.sprite = _shipFeedbackSprites[1];
    }

    IEnumerator Died(bool increaseScore)
    {
        _died = true;
        if(_characterType == CharacterType.Player) _playerMovement.enabled = false;
        else _enemyMovement.enabled = false;
        _deathAnimation.SetActive(true);
        _explosionAnimation.SetActive(true);

        yield return new WaitForSeconds(_deathDelay);

        _deathAnimation.SetActive(false);
        _explosionAnimation.SetActive(false);
        if(_characterType == CharacterType.Enemy) gameObject.SetActive(false);
        if(increaseScore) onDied?.Invoke();
    }

    public void Revive()
    {
        _spriteRenderer.sprite = _shipFeedbackSprites[0];
        _lifeBar.maxValue = _maxLife;
        _lifeBar.value = _maxLife;
        _currentLife = _maxLife;
        _enemyMovement.enabled = true;
        _died = false;
    }

    public void Kill(bool increaseScore = true)
    {
        TookDamage(_currentLife, increaseScore);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PirateTopDown.Player
{
    public class PlayerAttacks : MonoBehaviour
    {
        [Header("Required Components---------")]
        [SerializeField] private BulletsManager _playerBulletsManager;
        [SerializeField] private Image _frontalAttackUIFeedback, _sideAttackUIFeedback;
        [SerializeField] private AudioSource _gameplayAudioSource;
        [SerializeField] private AudioClip _shotSound;
        
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
                _gameplayAudioSource.PlayOneShot(_shotSound);
                _singleShotCooldownTimer = 0;
            }

            _singleShotCooldownTimer += Time.deltaTime;
            FillImage(_frontalAttackUIFeedback, _singleShotCooldownTimer, _singleShotCooldown);
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

                _gameplayAudioSource.PlayOneShot(_shotSound);
                _tripleShotCooldownTimer = 0;
            }

            _tripleShotCooldownTimer += Time.deltaTime;
            FillImage(_sideAttackUIFeedback, _tripleShotCooldownTimer, _tripleShotCooldown);
        }

        void FillImage(Image image, float currentValue, float maxValue)
        {
            image.fillAmount = currentValue/maxValue;

            if(image.fillAmount == 1) image.color = Color.white;
            else image.color = Color.gray; 
        }
    }
}

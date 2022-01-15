using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PirateTopDown.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement-----------")]
        [Range(0, 5)]
        [SerializeField] private float _movementSpeed;
        [Range(0, 10)]
        [SerializeField] private float _maxSpeed, _maxAcceleration;

        private Vector3 _movementVelocity;
        private Vector2 _screenBounds;
        private float _playerWidth, _playerHeight;

        [Header("Rotation-----------")]
        [Range(0, 5)]
        [SerializeField] float _rotationSpeed;

        void Awake()
        {
            _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            _playerWidth = spriteRenderer.bounds.extents.x;
            _playerHeight = spriteRenderer.bounds.extents.y;
        }

        void Update()
        {
            Move();
            RotateLeft();
            RotateRight();
        }

        void Move()
        {
            float movementInput = Input.GetAxis("Vertical");
            Vector3 desiredVelocity = new Vector3(0, movementInput, 0) * _maxSpeed;
            float maxSpeedChange = _maxAcceleration * Time.deltaTime;

            if(_movementVelocity.y < desiredVelocity.y) _movementVelocity.y = Mathf.Min(_movementVelocity.y + maxSpeedChange, desiredVelocity.y);
            else if(_movementVelocity.y > desiredVelocity.y) _movementVelocity.y = Mathf.Max(_movementVelocity.y - maxSpeedChange, desiredVelocity.y);
            
            Vector3 displacement = _movementVelocity.y * Time.deltaTime * transform.up;
            Vector3 newPosition = transform.localPosition + displacement;
            ConstraintMovementToScreenSize(newPosition);
        }

        void ConstraintMovementToScreenSize(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, _screenBounds.x * -1 + _playerWidth, _screenBounds.x - _playerWidth);
            position.y = Mathf.Clamp(position.y, _screenBounds.y * -1 + _playerHeight, _screenBounds.y - _playerHeight);

            transform.localPosition = position;
        }

        public void RotateLeft()
        {
            if(Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * _rotationSpeed);
            }
        }

        public void RotateRight()
        {
            if(Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.back * _rotationSpeed);
            }
        }
    }
}

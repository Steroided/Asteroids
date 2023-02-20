using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Tech;
using Asteroids.Core;

namespace Asteroids.Objects
{
    public class Player : ISpaceObject
    {
        private const int _lazerMaxAmmo = 10;
        private const float _lazerCooldownTime = 4;
        private const float MaxSpeed = 30;
        private const float Drag = 1.5f;
        private const float SteerAngle = 220;

        public int _lazerAmmo { get; set; } = 10;
        public float _lazerCooldown { get; set; }
        public bool _isActive { get; set; } = true;
        public float _turnInput { get; set; } = 0f;
        public float _moveInput { get; set; } = 0f;
        public float _flySpeed { get; } = 16;
        public float _lifeTime { get; }
        public float _currentSteer { get; set; }
        public float _size { get; }
        public Vector3 _flyDir { get; set; }
        public static List<Bullet> _bulletList { get; private set; } = new List<Bullet>();
        public Animator _anim { get; }
        public SpriteRenderer _spriteRenderer { get; }
        public Transform _transform { get; private set; }
        public WrapObject _boundariesW { get; }
        public GameObject _object { get; }


        private GameObject[] _bulletPrefab { get; }

        public Player(GameObject prefab, GameObject[] bulletPrefab)
        {
            _object = Object.Instantiate(prefab, Vector2.zero, Quaternion.identity);
            _anim = _object.GetComponent<Animator>();
            _spriteRenderer = _object.GetComponent<SpriteRenderer>();
            _transform = _object.transform;
            _bulletPrefab = bulletPrefab;
            _boundariesW = new WrapObject(9, 5.18f, _transform);
            _isActive = true;
        }
        #region MAIN
        public void Update()
        {
            _boundariesW.Boundaries();

            foreach (Bullet b in _bulletList.ToList())
            {

                if (b._transform != null)
                {
                    b.Update();
                }
                else
                {
                    _bulletList.Remove(b);
                }
                _bulletList.RemoveAll(s => s == null);

            }

        }
        private void LazerCoolDown()
        {
            if (_lazerAmmo < _lazerMaxAmmo)
            {
                _lazerCooldown += Time.deltaTime;
                if (_lazerCooldown >= _lazerCooldownTime)
                {
                    _lazerAmmo++;
                    _lazerCooldown = 0;
                }
            }
        }
        public void FixedUpdate()
        {
            LazerCoolDown();

            if (_isActive)
            {
                _currentSteer = Mathf.Lerp(_currentSteer, _turnInput, Drag * 1.5f * Time.deltaTime);
                _flyDir += _transform.up * _flySpeed * _moveInput * Time.deltaTime;
                _transform.position += _flyDir * Time.deltaTime;
                _transform.Rotate(Vector3.forward * _currentSteer * SteerAngle * Time.deltaTime);
                _flyDir = Vector3.Slerp(_flyDir, Vector3.zero, Drag * Time.deltaTime);
                _flyDir = Vector3.ClampMagnitude(_flyDir, MaxSpeed);
            }
            foreach (Bullet b in _bulletList)
            {
                if (b._transform != null)
                {
                    b.FixedUpdate();
                }
                else
                {
                    _bulletList.Remove(b);
                }
                _bulletList.RemoveAll(s => s == null);
            }
        }
        public void Shoot(int _choosedWeapon)
        {
            if (!_isActive)
                return;
            if (_choosedWeapon == 1)
            {
                if (_lazerAmmo <= 0)
                {
                    return;
                }
                else
                {
                    _lazerAmmo--;
                }
            }
            new Bullet(_bulletPrefab[_choosedWeapon], _transform, 10);
        }
        #endregion
    }
}
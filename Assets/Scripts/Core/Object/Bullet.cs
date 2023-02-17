using Asteroids.Core;
using Asteroids.Tech;
using UnityEngine;

namespace Asteroids.Objects
{
    public class Bullet : ISpaceObject
    {
        public float _flySpeed { get; } = 10;
        public float _lifeTime { get; } = 10;
        public float _size { get; } = 0.06f;
        public Vector3 _flyDir { get; }

        public WrapObject _boundariesW { get; }
        public SpriteRenderer _spriteRenderer { get; }
        public GameObject _object { get; }
        public Transform _transform { get; }
        private Transform _pool { get; }

        public Bullet(GameObject prefab, Transform playerTransform, float lifetime)
        {
            _object = Object.Instantiate(prefab, playerTransform.position, playerTransform.rotation);
            _pool = GameObject.FindGameObjectWithTag("Pool").transform;
            _transform = _object.transform;
            _transform.parent = _pool;
            _boundariesW = new WrapObject(9, 5.18f, _transform);
            _flyDir = _transform.up;
            Player._bulletList.Add(this);
            Object.Destroy(_object, lifetime);
        }
        #region MAIN
        public void ObjectFly(Vector3 direction)
        {

            _transform.position += _transform.up * _flySpeed * Time.deltaTime;
        }
        public void Update()
        {
            _boundariesW.Boundaries();
        }
        public void FixedUpdate()
        {
            ObjectFly(_flyDir);
        }
        #endregion
    }
}
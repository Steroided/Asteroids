using Asteroids.Core;
using Asteroids.Tech;
using UnityEngine;

namespace Asteroids.Objects
{
    public class AsteroidOrUfo : ISpaceObject
    {
        public float _flySpeed { get; }
        public float _lifeTime { get; } = 20;
        public float _size { get; } = 1;
        public Vector3 _flyDir { get; }
        public SpriteRenderer _spriteRenderer { get; }
        public WrapObject _boundariesW { get; }
        public GameObject _object { get; }
        public Transform _transform { get; }
        public ColliderOverlap _overLap { get; }

        private float _minSize { get; } = 0.35f;
        private bool _insideBoundaries { get; set; }
        private bool _fragment { get; set; }
        private bool _UFO { get; }
        private Transform _playerT { get; }
        private Transform _pool { get; }
        private BoxCollider2D _collider { get; }

        public AsteroidOrUfo(float size, float minsize, float flySpeed, bool UFO, GameObject prefab, Transform playerT, Vector3 spawnPoint, Vector3 flyDir , Sprite sprite)
        {
            _object = Object.Instantiate(prefab, spawnPoint, Quaternion.identity);
            _pool = GameObject.FindGameObjectWithTag("Pool").transform;
            _spriteRenderer = _object.GetComponent<SpriteRenderer>();
            _collider = _object.GetComponent<BoxCollider2D>();
            _transform = _object.transform;
            _transform.parent = _pool;
            _size = size;
            _minSize = minsize;
            _flyDir = flyDir;
            _playerT = playerT;
            _flySpeed = flySpeed;
            _UFO = UFO;
            _transform.localScale = Vector3.one * _size;
            _overLap = new ColliderOverlap(new BoxC2DCorners(_transform, _collider));
            _boundariesW = new WrapObject(15, 11.18f, _transform);
            if (!UFO)
            {
                _spriteRenderer.sprite = sprite;
                _transform.eulerAngles = Vector3.forward * Random.value * 360f;
            }

          
          
        }


        #region MAIN
        public void ObjectFly(Vector3 direction)
        {
            if (!_UFO)
                _transform.position += direction * _flySpeed * Time.deltaTime;
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, _playerT.position, _flySpeed * Time.deltaTime);
            }
        }
        public void Update()
        {

            OverlapArea();


            if (!_insideBoundaries)
            {
                if (_transform.position.x <= 10 && _transform.position.x >= -10 && _transform.position.y <= 6.18f && _transform.position.y >= -6.18f)
                {
                    _boundariesW.BoundariesX = 10;
                    _boundariesW.BoundariesY = 6.18f;
                    _insideBoundaries = true;
                }

            }


            _boundariesW.Boundaries();
        }
        public void FixedUpdate()
        {
            ObjectFly(_flyDir);
        }

        public void OverlapArea()
        {
            Collider2D col = _overLap.hit();
            if (col)
            {
                switch (col.gameObject.tag)
                {
                    case "Bullet":
                        {
                            if (col.gameObject.layer != 11)
                                Object.Destroy(col.transform.gameObject);
                            if (_size * 0.5f >= _minSize && !_fragment && !_UFO)
                            {
                                CreateSplit(2);
                            }
                            Object.FindObjectOfType<GameManager>()._gamePlayManager.AsteroidDestroyed(this);
                            Object.Destroy(_transform.gameObject);
                            break;
                        }
                    case "Player":
                        {
                            Object.FindObjectOfType<GameManager>()._gamePlayManager.PlayerDeath();
                            break;
                        }

                }
            }
        }

        private void CreateSplit(int Splits)
        {
            for (int i = 0; i < Splits; i++)
            {
                Vector2 position = _transform.position;
                position += Random.insideUnitCircle * 0.5f;
                AsteroidOrUfo half = new AsteroidOrUfo(_size * 0.5f, _minSize, _flySpeed + 0.5f, _UFO, _transform.gameObject, _playerT, position, _flyDir, _spriteRenderer.sprite);
                half._fragment = true;
                SpaceObjectSpawner.asteroidList.Add(half);
            }
        }

        #endregion
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Objects;

namespace Asteroids.Core
{
    public class SpaceObjectSpawner
    {
        private const float _minSize = 0.35f;
        private const float _maxSize = 1.65f;
        private const int _maxAsteroids = 50;

        public float _timeNow { get; private set; }
        public AsteroidOrUfo _asteroidOrUfo { get; private set; }
        public static List<AsteroidOrUfo> asteroidList { get; private set; } = new List<AsteroidOrUfo>();

        private float  _spawnRate { get; }
        private float _spawnDistance { get; } = 12f;
        [Range(0f, 45f)]
        private float _trajectoryVariance = 15f;
        private Transform _playerT { get; }
        private GameObject[] _prefab { get; }
        private Sprite[] _sprites { get; }
        public SpaceObjectSpawner(float spawnRate, GameObject[] prefab, Sprite[] sprites, Transform playerT)
        {
            _spawnRate = spawnRate;
            _prefab = prefab;
            _sprites = sprites;
            _playerT = playerT;
        }
        #region MAIN
        public void Update()
        {

            foreach (AsteroidOrUfo a in asteroidList.ToList())
            {
                if (a._transform != null)
                {
                    a.Update();
                }
                else
                {
                    asteroidList.Remove(a);
                }

                asteroidList.RemoveAll(s => s == null);

            }


            if (_timeNow > _spawnRate)
            {
                if (asteroidList.Count < _maxAsteroids)
                {
                    int i = Random.Range(0, 2);
                    bool UFO = i == 1 ? true : false;
                    float SizeTemp;
                    SizeTemp = UFO ? _maxSize / 3 : _maxSize;
                    Vector2 spawnDirection = Random.insideUnitCircle.normalized;
                    Vector3 spawnPoint = spawnDirection * _spawnDistance;
                    float variance = Random.Range(-_trajectoryVariance, _trajectoryVariance);
                    Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
                    Vector2 flyDir = rotation * -spawnDirection;
                    _asteroidOrUfo = new AsteroidOrUfo(Random.Range(_minSize, SizeTemp), _minSize, Random.Range(0.5f, 2), UFO, _prefab[i], _playerT, spawnPoint, flyDir, _sprites[Random.Range(0, _sprites.Length)]);
                    asteroidList.Add(_asteroidOrUfo);
                    _timeNow = 0;
                }
            }
            else
            {
                _timeNow += Time.deltaTime;
            }



        }




        public void FixedUpdate()
        {

            foreach (AsteroidOrUfo a in asteroidList)
            {
                if (a._transform != null)
                {
                    a.FixedUpdate();
                }
                else
                {
                    asteroidList.Remove(a);
                }

                asteroidList.RemoveAll(s => s == null);
            }
        }


        #endregion
    }
}
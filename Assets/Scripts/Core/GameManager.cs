using Asteroids.Objects;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _explosionEffect;

        [SerializeField]
        private GameObject _gameOverUI;

        private InputManager _inputManager { get; set; }
        public GamePlayManager _gamePlayManager { get; set; }

        private SpaceObjectSpawner _objectSpawner { get; set; }
        [SerializeField]
        private Sprite[] _asteroidSprites;
        [SerializeField]
        private GameObject[] _spaceObjectsPrefab;

        [SerializeField]
        private float _spawnRate;

        private Player _player { get; set; }
        [SerializeField]
        private GameObject _playerPrefab;
        [SerializeField]
        private GameObject[] _bulletPrefab;


        [SerializeField]
        private Text[] _UiTexts;
        // _scoreText=0;
        // _livesText=1;
        //  _coordXText=2;
        //  _coordYText=3;
        //  _rotationAngleText=4;
        // _speedText=5;
        //  _lazerAmmoText=6;
        //  _lazerCooldownText=7;

        #region MAIN

        private void Start()
        {
            _player = new Player(_playerPrefab, _bulletPrefab);
            _gamePlayManager = new GamePlayManager(_gameOverUI, _explosionEffect, _UiTexts, _player);
            _inputManager = new InputManager(_player);
            _objectSpawner = new SpaceObjectSpawner(_spawnRate, _spaceObjectsPrefab, _asteroidSprites, _player._transform);


        }



        private void Update()
        {
            _inputManager.Update();
            _gamePlayManager.Update();
            _objectSpawner.Update();
            _player.Update();

        }
        private void FixedUpdate()
        {
            _objectSpawner.FixedUpdate();
            _player.FixedUpdate();
        }


        #endregion


    }
}
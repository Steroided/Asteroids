using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using Asteroids.Objects;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace Asteroids.Core
{
    public class GamePlayManager
    {
        
        public static bool _newGameKey { get; set; }
        public Player _player { get; }
        public static class InterFaceValues
        {
            public static int _scoreText;
            public static int _lives;
            public static float _coordinatesX;
            public static float _coordinatesY;
            public static float _rotationAngle;
            public static float _speed;
            public static int _lazerAmmo;
            public static float _lazerCooldown;
        }

        private int _lives { get; set; }
        private int _scoreText { get; set; }
        private float _respawnTimeNow { get; set; }
        private float respawnDelay { get; } = 3f;
        private ParticleSystem _explosionEffect { get; }
        private GameObject _gameOverUI { get; }
        private Text[] _UITexts { get; }

        public GamePlayManager(GameObject gameOverUI, ParticleSystem explosionEffect, Text[] UiTexts, Player player)
        {
            _gameOverUI = gameOverUI;
            _explosionEffect = explosionEffect;
            _UITexts = UiTexts;
            _player = player;
            NewGame();

        }

        #region MAIN
        public void Update()
        {
            Respawn();
            SetInterfaceValues();

            SetText();

            if (_lives <= 0 && _newGameKey)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                NewGame();
            }
        }
        private void SetInterfaceValues()
        {
            InterFaceValues._lives = _lives;
            InterFaceValues._scoreText = _scoreText;
            InterFaceValues._speed = Round(Vector3.Magnitude(_player._flyDir)) * 10;
            InterFaceValues._coordinatesX = Round(_player._transform.position.x);
            InterFaceValues._coordinatesY = Round(_player._transform.position.y);
            InterFaceValues._rotationAngle = Round(_player._transform.eulerAngles.z);
            InterFaceValues._lazerAmmo = _player._lazerAmmo;
            InterFaceValues._lazerCooldown = Round(_player._lazerCooldown);
        }
        public float Round(float number)
        {

            return Mathf.Round(number * 10) * 0.1f;
        }

        private void Respawn()
        {
            if (_player._transform.gameObject.tag == "Player")
            {
                _respawnTimeNow += Time.deltaTime;
            }
            else
            {
                _respawnTimeNow -= Time.deltaTime;





                if (_respawnTimeNow <= -respawnDelay)
                {
                    _player._transform.gameObject.tag = "Player";
                    _player._anim.enabled = false;

                    _player._spriteRenderer.color = new Color(1, 1, 1, 1);
                }
            }

            if (_lives > 0 && !_player._isActive && _respawnTimeNow >= respawnDelay)
            {
                _player._anim.enabled = true;
                _respawnTimeNow = 0;
                _player._transform.gameObject.tag = "Respawn";
                _player._transform.position = Vector3.zero;
                _player._flyDir = Vector3.zero;
                _player._currentSteer = 0;
                _player._transform.rotation = Quaternion.identity;
                _player._isActive = true;
                _player._transform.gameObject.SetActive(true);

            }
        }

        public void NewGame()
        {




            _lives = 1;

            _scoreText = 0;

            _gameOverUI.SetActive(false);



        }
        
        public void AsteroidDestroyed(AsteroidOrUfo asteroid)
        {
            _explosionEffect.transform.position = asteroid._transform.position;
            _explosionEffect.Play();

            if (asteroid._size < 0.7f)
            {
                _scoreText += 100;
                // small asteroid
            }
            else if (asteroid._size < 1.4f)
            {
                _scoreText += 50; // medium asteroid
            }
            else
            {
                _scoreText += 25; // large asteroid
            }


        }

        public void PlayerDeath()
        {
            _explosionEffect.transform.position = _player._transform.position;
            _explosionEffect.Play();
            _player._transform.gameObject.SetActive(false);
            _respawnTimeNow = 0;
            _lives--;
            _player._isActive = false;
            if (_lives <= 0)
            {
                GameOver();
            }
        }

        public void GameOver()
        {

            _gameOverUI.SetActive(true);
        }
        private void SetText()
        {
            for (int i = 0; i < typeof(InterFaceValues).GetFields().Length; i++)
            {
                FieldInfo p = typeof(InterFaceValues).GetFields()[i];
                _UITexts[i].text = p.GetValue(null).ToString();
            }

        }
    }
    #endregion
}
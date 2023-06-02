using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace RovioAsteroids {
    public class AsteroidsManager : MonoBehaviour {
        private IPool<IAsteroid> _bigAsteroidPool;
        private IPool<IAsteroid> _mediumAsteroidPool;
        private IPool<IAsteroid> _smallAsteroidPool;

        [Inject] private readonly IMainCamera _mainCamera;
        [Inject] private readonly IGameSettings _gameSettings;

        private void Awake() {
            _bigAsteroidPool = GameObject.FindGameObjectWithTag("BigAsteroidPool")
                .GetComponent<IPool<IAsteroid>>();
            _mediumAsteroidPool = GameObject.FindGameObjectWithTag("MediumAsteroidPool")
                .GetComponent<IPool<IAsteroid>>();
            _smallAsteroidPool = GameObject.FindGameObjectWithTag("SmallAsteroidPool")
                .GetComponent<IPool<IAsteroid>>();
        }

        private async void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                var asteroid = await _bigAsteroidPool.SpawnAsync();
                var position = CreateRandomPosition();
                var velocity = GetVelocityForBigAsteroid();
                var angularVelocity = Random.Range(_gameSettings.BigAsteroidAngularVelocityRange.x,
                    _gameSettings.BigAsteroidAngularVelocityRange.y);
                asteroid.SetPosition(position);
                asteroid.SetVelocity(velocity);
                asteroid.SetAngularVelocity(angularVelocity);
            }
            else if (Input.GetKeyDown(KeyCode.S)) {
                var asteroid = await _smallAsteroidPool.SpawnAsync();

                var position = CreateRandomPosition();
                var velocity = GetVelocityForSmallAsteroid();
                var angularVelocity = Random.Range(_gameSettings.SmallAsteroidAngularVelocityRange.x,
                    _gameSettings.SmallAsteroidAngularVelocityRange.y);
                asteroid.SetPosition(position);
                asteroid.SetVelocity(velocity);
                asteroid.SetAngularVelocity(angularVelocity);
            }
            else if (Input.GetKeyDown(KeyCode.M)) {
                var asteroid = await _mediumAsteroidPool.SpawnAsync();

                var position = CreateRandomPosition();
                var velocity = GetVelocityForMediumAsteroid();
                var angularVelocity = Random.Range(_gameSettings.MediumAsteroidAngularVelocityRange.x,
                    _gameSettings.MediumAsteroidAngularVelocityRange.y);
                asteroid.SetPosition(position);
                asteroid.SetVelocity(velocity);
                asteroid.SetAngularVelocity(angularVelocity);
            }
        }

        private Vector3 CreateRandomPosition() {
            var x = Random.Range(0, Screen.width);
            var y = 0; //on bottom
            if (Random.Range(0f, 1f) > .5f) {
                //generate on top
                y = Screen.height;
            }

            var screenPosition = new Vector3(x, y, _mainCamera.NearClipPlane());
            return _mainCamera.ScreenToWorldPoint(screenPosition);
        }

        private Vector2 GetVelocityForBigAsteroid() {
            var xVelocity = Random.Range(_gameSettings.BigAsteroidSpeedRange.x, _gameSettings.BigAsteroidSpeedRange.y);
            var yVelocity = Random.Range(_gameSettings.BigAsteroidSpeedRange.x, _gameSettings.BigAsteroidSpeedRange.y);

            return new Vector2(xVelocity, yVelocity);
        }

        private Vector2 GetVelocityForMediumAsteroid() {
            var xVelocity = Random.Range(_gameSettings.MediumAsteroidSpeedRange.x,
                _gameSettings.MediumAsteroidSpeedRange.y);
            var yVelocity = Random.Range(_gameSettings.MediumAsteroidSpeedRange.x,
                _gameSettings.MediumAsteroidSpeedRange.y);

            return new Vector2(xVelocity, yVelocity);
        }

        private Vector2 GetVelocityForSmallAsteroid() {
            var xVelocity = Random.Range(_gameSettings.SmallAsteroidSpeedRange.x,
                _gameSettings.SmallAsteroidSpeedRange.y);
            var yVelocity = Random.Range(_gameSettings.SmallAsteroidSpeedRange.x,
                _gameSettings.SmallAsteroidSpeedRange.y);

            return new Vector2(xVelocity, yVelocity);
        }
    }
}
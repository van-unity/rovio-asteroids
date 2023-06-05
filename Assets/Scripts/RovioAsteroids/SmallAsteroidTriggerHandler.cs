using UnityEngine;
using Zenject;

namespace RovioAsteroids {
    public class SmallAsteroidTriggerHandler : MonoBehaviour, IAsteroidTriggerHandler {
        private IPool<IAsteroid> _pool;

        [Inject] private readonly IGameplayModel _gameplayModel;

        private void Start() {
            _pool = GameObject.FindGameObjectWithTag("BigAsteroidPool").GetComponent<IPool<IAsteroid>>();
        }

        public void HandleTriggerEnter(IAsteroid asteroid, Collider2D other) {
            if (other.CompareTag("PlayerBullet")) {
                _pool.Despawn(asteroid);
                _gameplayModel.OnAsteroidDestroyed(AsteroidType.Big);
            }
        }
    }
}
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace RovioAsteroids {
    [RequireComponent(typeof(IBullet))]
    public class BulletOutOfBoundsHandler : MonoBehaviour, IOutOfBoundsHandler {
        private Transform _transform;
        private IBullet _bullet;
        private IPool<IBullet> _pool;

        [Inject] private readonly IOutOfBoundsChecker _outOfBoundsChecker;
        private CancellationTokenSource _registerCancellation;

        private void Awake() {
            _transform = transform;
            _bullet = GetComponent<Bullet>();
        }

        private async void OnEnable() {
            _registerCancellation = new CancellationTokenSource();
            while (_outOfBoundsChecker == null) {
                await Task.Delay((int)(Time.deltaTime * 1000), _registerCancellation.Token);
            }

            _outOfBoundsChecker.Register(this);
        }

        private void Start() {
            _pool = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<IPool<IBullet>>();
        }

        public Vector3 GetPosition() => _transform.position;

        public void OnOutOfBounds() {
            _pool.Despawn(_bullet);
        }

        private void OnDisable() {
            _outOfBoundsChecker.UnRegister(this);
        }

        private void OnDestroy() {
            _registerCancellation.Cancel();
        }
    }
}
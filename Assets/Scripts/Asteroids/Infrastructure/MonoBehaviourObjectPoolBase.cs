using System;
using System.Threading.Tasks;
using Asteroids.Domain;
using UnityEngine;

namespace Asteroids.Infrastructure {
    public abstract class MonoBehaviourObjectPoolBase<T> : MonoBehaviour, IPool<T> where T : ISpawnable {
        [SerializeField] private Transform _container;
        [SerializeField] private int _size;
        private T[] _pool;

        private void Start() {
            _pool = new T[_size];
            for (int i = 0; i < _size; i++) {
                CreateAsync(i);
            }
        }

        private async void CreateAsync(int index) {
            _pool[index] = await CreateTask();
            _pool[index].SetParent(_container);
            _pool[index].SetActive(false);
        }

        protected abstract Task<T> CreateTask();

        public async Task<T> SpawnAsync() {
            foreach (var spawnable in _pool) {
                if (spawnable.IsActive) {
                    continue;
                }

                spawnable.SetActive(true);
                spawnable.OnSpawn();
                return spawnable;
            }

            Array.Resize(ref _pool, ++_size);
            var newItem = await CreateTask();
            _pool[_size - 1] = newItem;
            newItem.SetActive(true);
            newItem.OnSpawn();
            return newItem;
        }

        public void Despawn(T objectToDespawn) {
            objectToDespawn.OnDespawn();
            objectToDespawn.SetActive(false);
        }

        public void Clear() {
            foreach (var spawnable in _pool) {
                Despawn(spawnable);
            }
        }
    }
}
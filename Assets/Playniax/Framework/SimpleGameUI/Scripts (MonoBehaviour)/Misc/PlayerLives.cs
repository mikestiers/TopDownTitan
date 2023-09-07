using UnityEngine;
using UnityEngine.Events;
using Playniax.Ignition;

namespace Playniax.UI.SimpleGameUI
{
    public class PlayerLives : MonoBehaviour
    {
        public GameObject prefab;
        public int testLives = 9;
        public int playerIndex;
        public float timeout = 1.5f;
        public bool resetCamera;
        public float cameraSteps = 10;
        public float cameraPosition = .01f;
        public UnityEvent onGameOver;
        void Start()
        {
            if (prefab == null) return;

            if (PlayerData.Get(playerIndex).lives <= 0) return;

            if (SimpleGameUI.instance == null) PlayerData.Get(playerIndex).lives = testLives;

            _parent = prefab.transform.parent;

            _prefab = Instantiate(prefab);
            _prefab.name = prefab.name;
            _prefab.hideFlags = HideFlags.HideInHierarchy;
            _prefab.SetActive(false);

            _player = prefab;

            _timer = timeout;
        }
        void LateUpdate()
        {
            if (_prefab == null) return;

            if (SimpleGameUI.instance && SimpleGameUI.instance.isBusy) return;

            if (PlayerData.Get(playerIndex).lives <= 0) return;

            if (_player) return;

            if (resetCamera && _ResetCamera() == false) return;

            if (_timer > 0)
            {
                _timer -= 1 * Time.deltaTime;

                return;
            }

            if (PlayerData.Get(playerIndex).lives > 1) _NewPlayer();

            PlayerData.Get(playerIndex).lives -= 1;

            if (PlayerData.Get(playerIndex).lives <= 0) onGameOver.Invoke();
        }

        void _NewPlayer()
        {
            _player = Instantiate(_prefab, _parent);

            _player.name = _prefab.name;

            _player.SetActive(true);

            _timer = timeout;
        }

        bool _ResetCamera()
        {
            var camera = Camera.main.transform.position;

            camera.x += (_prefab.transform.position.x - camera.x) / cameraSteps;
            camera.y += (_prefab.transform.position.y - camera.y) / cameraSteps;

            Camera.main.transform.position = camera;

            camera.z = _prefab.transform.position.z;

            if (Vector3.Distance(_prefab.transform.position, camera) < cameraPosition) return true;

            return false;
        }

        Transform _parent;
        GameObject _prefab;
        GameObject _player;
        float _timer;
    }
}
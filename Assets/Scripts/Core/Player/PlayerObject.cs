using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player
{

    public class PlayerObject : MonoBehaviour
    {
        private bool _spawned = false;
        private PlayerMode _activeMode;
        [SerializeField] private Camera PlayerCamera;

        public void SetPlayerMode(PlayerMode playerMode)
        {
            _activeMode = playerMode;
        }

        public void SpawnPlayer()
        {
            _spawned = true;
        }

        private void Update()
        {
            if (_spawned)
            {

            }
        }

        public Camera GetPlayerCamera() => PlayerCamera;
    }

    public enum PlayerMode
    {

    }
}

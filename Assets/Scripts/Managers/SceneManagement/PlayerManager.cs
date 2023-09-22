using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public void CreatePlayerInstance()
    {
        GameObject p_Instance = Instantiate(playerPrefab);
        SpawnPlayer(p_Instance.GetComponent<PlayerObject>());
    }

    private void SpawnPlayer(PlayerObject instance)
    {
        instance.SpawnPlayer();
        instance.SetPlayerMode(PlayerMode.active);
    }
}

public enum PlayerMode { loading, active, dead }

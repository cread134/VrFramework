using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private PlayerObject playerObject;

    
    public void CreatePlayerInstance()
    {
        GameObject p_Instance = Instantiate(playerPrefab);
        PlayerObject p_Obj = p_Instance.GetComponent<PlayerObject>();
        SpawnPlayer(p_Obj);
        playerObject = p_Obj;
    }

    private void SpawnPlayer(PlayerObject instance)
    {
        instance.SpawnPlayer();
        instance.SetPlayerMode(PlayerMode.gameplay);
    }

    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            CreatePlayerInstance();
        }
    }

    private void Start()
    {
        GameManager.Instance.levelManager.OnSceneChange += OnSceneLoaded;
    }

    public void OnSceneLoaded(object sender, SceneChangeEvent sceneChange)
    {
        switch (sceneChange._loadedScene.sceneType)
        {
            case GameScene.SceneType.menu:
                playerObject?.SetPlayerMode(PlayerMode.active);
                break;
            case GameScene.SceneType.gameplay:
                playerObject?.SetPlayerMode(PlayerMode.gameplay);
                break;
            case GameScene.SceneType.admininstrive:
                playerObject?.SetPlayerMode(PlayerMode.active);
                break;
        }
    }
}

public enum PlayerMode { loading, active, gameplay, dead }

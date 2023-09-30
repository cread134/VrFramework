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

        SetupGameCanvases();
    }

    private void Awake()
    {
        var p = GameObject.FindObjectOfType<PlayerObject>();
        if (p == null)
        {
            CreatePlayerInstance();
        } else
        {
            playerObject = p;
        }
    }

    private void SetupGameCanvases()
    {
        var canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var item in canvases)
        {
            item.worldCamera = playerObject.GetPlayerCamera();
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
        SetupGameCanvases();
    }
}

public enum PlayerMode { loading, active, gameplay, dead }

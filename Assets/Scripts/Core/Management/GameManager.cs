using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DebugController debugController { get; private set; }
    public LevelLoadManager levelManager { get; private set; }
    public VfxManager vfxManager { get; private set; }
    public AudioManager audioManager { get; private set; }
    public PlayerManager playerManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        debugController = GetComponentInChildren<DebugController>();
        levelManager = GetComponentInChildren<LevelLoadManager>();
        vfxManager = GetComponentInChildren<VfxManager>();
        audioManager = GetComponentInChildren<AudioManager>();
        playerManager = GetComponentInChildren<PlayerManager>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ManagerExtensions
{
    public static void LoadScene(this GameScene gameScene)
    {
        GameManager.Instance.levelManager.LoadScene(gameScene);
    }
}

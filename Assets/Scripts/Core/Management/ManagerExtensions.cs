using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Management
{
    public static class ManagerExtensions
    {
        public static void LoadScene(this GameScene gameScene)
        {
            GameManager.Instance.levelManager.LoadScene(gameScene);
        }
    }
}
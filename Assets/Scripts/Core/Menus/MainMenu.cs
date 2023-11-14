using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Management;

namespace Core.Menu
{
    public class MainMenu : MonoBehaviour
    {

        public GameScene gameScene;
        void Start()
        {
            LoadGame();
        }

        public void LoadGame()
        {
            GameManager.Instance.levelManager.LoadScene(gameScene);
        }
    }
}
using System;

public class SceneChangeEvent : EventArgs
{
    public SceneChangeEvent(GameScene loadedScene)
    {
        _loadedScene = loadedScene;
    }
    public GameScene _loadedScene;
}

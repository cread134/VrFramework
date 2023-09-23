using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAssurance
{
    [RuntimeInitializeOnLoadMethod]
    public static void AssureGameManager()
    {
        var objs = GameObject.FindObjectOfType<GameManager>();
        if(objs == null )
        {
            var managerPre = Resources.Load("GameManager") as GameObject;
            GameObject.Instantiate(managerPre);
        }
    }
}
  
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageController : MonoBehaviour
{ 
    private PlayerController[] playerList;
    private TargetController[] targetList;
    private ObstacleManager[] obstacleManagerList;

    public void Init()
    {
        gameObject.SetActive(true);

        targetList = transform.GetComponentsInChildren<TargetController>();
        foreach (TargetController t in targetList )
        {
            t.Show(false);
        }

        playerList = transform.GetComponentsInChildren<PlayerController>(true);
        

        obstacleManagerList= transform.GetComponentsInChildren<ObstacleManager>(); 
        foreach (ObstacleManager obsManager in obstacleManagerList) {
            obsManager.Reset();
        }
    }


    public void Deactivate()
    {
        if (playerList != null)
        {
            foreach (PlayerController p in playerList)
            { 
                p.ClearAllData();
            }
        }
        gameObject.SetActive(false);

    }

    public PlayerController[] GetPlayerList()
    {
        return playerList;
    }

    
}
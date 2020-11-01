using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleController[] obstacleList;

    public void Start()
    {
        obstacleList = GetComponentsInChildren<ObstacleController>();
    }

    public void Reset() {
        foreach (ObstacleController o in obstacleList)
        {
            o.Reset();
        }
    }

    public void CollidedSomeone(Vector3 collidePoint)
    {
        Vector3 forceDirection = transform.position - collidePoint;
        AfterCollision(forceDirection, collidePoint);
    }

    public void AfterCollision(Vector3 forceDirection, Vector3 hitPosition)
    {
        foreach (ObstacleController o in obstacleList) {
            o.Hit(forceDirection.normalized, hitPosition);
        }
    }
}

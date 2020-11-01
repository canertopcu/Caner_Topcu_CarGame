using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    public Transform followingPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followingPlayer != null)
        {
            Vector3 dummyPlace = followingPlayer.position;
            dummyPlace.y += 3f;
            transform.position = dummyPlace;
        }
    }

    public void SetFollowing(Transform player) {

        followingPlayer = player;
    }
}

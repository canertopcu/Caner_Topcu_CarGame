using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Player
{
    public bool isStopped = false;
    public List<LiveData> dataList;
      
    public LiveData[] savedData;
    private float speed = 1;
    private float rotationSpeed = 1; 

    public Transform startTransform;
    public TargetController target;
    public bool isReplay = false;
    public int playIndex = 0;
    bool endMove = false;

    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        dataList = new List<LiveData>();
        
    }
     

    public override void Init(bool isReplaying)
    {
        speed = gameManager.speed;
        rotationSpeed = gameManager.rotSpeed;

        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
        isReplay = isReplaying;
        playIndex = 0;
        endMove = false;
        target.Show(!isReplaying);
        if (!isReplaying) {
            ClearAllData(); 
        }

    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (gameManager.gameState == GameState.Play)
        {
            if (!isReplay)
            {
                MovePlayer(); 
            }
            else
            {
                MoveNPC();
            }

        }
    }

    private void MovePlayer()
    {
        if (gameManager.isLeftPressed && !gameManager.isRightPressed)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
        else if (!gameManager.isLeftPressed && gameManager.isRightPressed)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else { }


        transform.position += transform.forward * speed * Time.deltaTime;
        dataList.Add(new LiveData(transform));
    }

    private void MoveNPC() {
        if (!endMove)
        {
            if (savedData.Length > playIndex)
            {
                transform.position = savedData[playIndex].position;
                transform.rotation = savedData[playIndex].rotation;

                playIndex++;
            }
        }
    }



    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (gameManager.gameState == GameState.Play)
        {
            if (collision.collider.CompareTag("EndPos"))
            {
                if (collision.collider.transform == target.transform)
                {
                    if (!isReplay)
                    {
                        savedData = dataList.ToArray();
                        dataList.Clear();
                        gameManager.PassNextPlayer();
                    }
                    else
                    {
                        endMove = true;
                        isStopped = true;
                    }
                }
            }
            else//Obstacle or other player hitted
            { 
                if (!isReplay)
                {

                    ObstacleController obst = collision.collider.GetComponent<ObstacleController>();
                    if (obst != null)
                    {
                        obst.transform.parent.GetComponent<ObstacleManager>().CollidedSomeone(obst.firstPos);
                    }

                }
                else
                {
                    endMove = true;
                }
                gameManager.ShowGameOver();

            }
        }
    }

    public void ClearAllData() {
        if (dataList!= null)
        {
            dataList.Clear();
            savedData = dataList.ToArray();
        }
         
    }
}




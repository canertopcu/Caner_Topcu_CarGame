using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isStopped = false;
    public List<LiveData> dataList;
      
    public LiveData[] savedData;
    private float speed = 1;
    private float rotationSpeed = 1;
    GameManager gameManager;

    public Transform startTransform;
    public TargetController target;
    public bool isReplay = false;
    public int playIndex = 0;
    bool endMove = false;

    
    // Start is called before the first frame update
    void Awake()
    {
        dataList = new List<LiveData>();
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.OnStateChange += GameManager_OnStateChange;
    }

    private void OnDisable()
    {
        gameManager.OnStateChange -= GameManager_OnStateChange;
    }

    private void GameManager_OnStateChange()
    { 
    }

    public void Init(bool isReplaying)
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
    void Update()
    {

        if (gameManager.gameState == GameState.Play)
        {
            if (!isReplay)
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
            else
            {
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

        }
    }

    

    private void OnCollisionEnter(Collision collision)
    {
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

[System.Serializable]
public struct LiveData
{
    public Vector3 position;
    public Quaternion rotation;

    public LiveData(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
    }
}


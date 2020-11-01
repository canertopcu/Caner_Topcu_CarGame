using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState { Pause, Play, Stop }

public class GameManager : Singleton<GameManager>
{
    public float speed = 3f;
    public float rotSpeed = 180f;
    public GameState gameState = GameState.Pause;

    public Transform stageParent;
    private StageController[] stages;
    public StageController activeStage;
    public int activeStageIndex = 0;

    public int playerCount = 0;
    private PlayerController[] playerList;
    public int activePlayerIndex = 0;
    public PlayerController activePlayer;

    internal bool isLeftPressed = false;
    internal bool isRightPressed = false;
    public event Action OnStateChange;

    UIManager uiManager;
    public IndicatorController indicator;

    public void SetState(GameState state)
    {
        gameState = state;
        OnStateChange?.Invoke();

    }

    // Start is called before the first frame update
    private void Start()
    {
        uiManager = UIManager.Instance;
        uiManager.Init();
        stages = stageParent.GetComponentsInChildren<StageController>(true);

        ActivateStage(activeStageIndex);
    }

    private void ActivateStage(int index)
    {
        if (index >= stages.Length)
        {
            Debug.LogError("GameFinished");
            index = UnityEngine.Random.Range(0, stages.Length);
        }

        foreach (var stage in stages)
        {
            stage.Deactivate();
        }
        activeStage = stages[index];

        activeStage.Init();


        playerList = activeStage.GetPlayerList();
        foreach (PlayerController p in playerList)
        {
            p.ClearAllData();
        }

        playerCount = playerList.Length;
        activePlayerIndex = 0;
        ActivatePlayer(activePlayerIndex);
    }


    private void ResetStageWithLastData() {
        activeStage.Init();
        playerList = activeStage.GetPlayerList();
        ActivatePlayer(activePlayerIndex);
    }
    public void RestartStage() {
        ActivateStage(activeStageIndex);
    }



    private void ActivatePlayer(int index)
    {
        SetState(GameState.Pause);

        activePlayerIndex = index;
        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i <= activePlayerIndex; i++)
        {
            playerList[i].gameObject.SetActive(true);
            playerList[i].Init(i != activePlayerIndex);
            if (activePlayerIndex == i)
            {
                activePlayer = playerList[i];
                indicator.SetFollowing(activePlayer.transform);
            }
        }
    }

    public void PassNextPlayer()
    {
        if (activePlayerIndex < playerCount - 1)
        {
            activePlayerIndex++;
            ActivatePlayer(activePlayerIndex);
        }
        else
        {
            ShowGoNextStage();
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Pause)
        {
            if (Input.anyKey)
            {
                SetState(GameState.Play);
            }
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            isLeftPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            isLeftPressed = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            isRightPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            isRightPressed = false;
        }
    }

    public void ShowGameOver() //Show UI
    {
        SetState(GameState.Stop);

        //run this after 1 min
        StartCoroutine(RunAfterTime(() =>
        {
            uiManager.ShowGameOverPanel(() => { ResetStageWithLastData(); });// ActivateStage(activeStageIndex); });
        }, 1f));

    }
    public void ShowGoNextStage()//Show UI
    {
        SetState(GameState.Stop);

        //run this after 1 min
        StartCoroutine(RunAfterTime(() =>
        {
            uiManager.ShowPassPanel(() =>
            {
                if (activeStageIndex < stages.Length)
                {
                    activeStageIndex++;
                    ActivateStage(activeStageIndex);
                }
            });
        }, 1f));
    }

    IEnumerator RunAfterTime(Action action, float time) //Run code after a duration
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

}

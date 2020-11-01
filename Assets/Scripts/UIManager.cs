using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private Action PassLevelAction;
    private Action ReplayAction;
    public GameObject passLevelPanel;
    public GameObject gameOverPanel;
    public Button LeftButton, RightButton;

    GameManager gameManager;

    public void Init()
    {
        gameManager = GameManager.Instance;
        passLevelPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void RestartStage()
    { 
        GameManager.Instance.RestartStage();
    }

    public void ShowPassPanel(Action passLevelAction)
    {
        PassLevelAction = passLevelAction;
        passLevelPanel.SetActive(true);
    }

    public void PassLevelButtonPressed()
    {
        PassLevelAction?.Invoke();
        passLevelPanel.SetActive(false);
    }

    internal void ShowGameOverPanel(Action replayAction)
    {
        ReplayAction = replayAction;
        gameOverPanel.SetActive(true);
    }

    public void ReplayButtonPressed()
    {
        ReplayAction?.Invoke();
        gameOverPanel.SetActive(false);
    }

    public void LeftButtonPressed()
    {
        gameManager.isLeftPressed = true;
    }
    public void LeftButtonReleased()
    {
        gameManager.isLeftPressed = false;
    }
    public void RighButtonPressed()
    { 
        gameManager.isRightPressed = true;
    }
    public void RightButtonReleased()
    {
        gameManager.isRightPressed = false;
    }
}

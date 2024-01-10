using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;

    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    private bool isGamePause = false;
    private int moveLeft;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        GameObject map = Instantiate(currentLevelData.map);
        moveLeft = currentLevelData.moveLimit;
        gameScene.UpdateMoveLeft(moveLeft);
        Time.timeScale = 1;
    }

    public void Win()
    {
        if (isGameWin || isGameLose)
        {
            return;
        }
        LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex, true, true);
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        isGameWin = true;

        gameScene.ShowWinPanel();
        LevelManager.instance.levelData.SaveDataJSON();
    }

    public void DecreaseMove()
    {
        moveLeft--;
        gameScene.UpdateMoveLeft(moveLeft);
    }

    public void CheckMove()
    {
        if (moveLeft <= 0)
        {
            if (isGameWin || isGameLose)
            {
                return;
            }
            Lose();
        }
    }

    public void Lose()
    {
        isGameLose = true;
        gameScene.ShowLosePanel();
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isGameLose;
    }

    public void PauseGame()
    {
        isGamePause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGamePause = false;
        Time.timeScale = 1;
    }

    public bool IsGamePause()
    {
        return isGamePause;
    }
}


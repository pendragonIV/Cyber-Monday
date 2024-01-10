using DG.Tweening;
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
    private int moveLeft;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetTheLevelAtGivenIndex(LevelManager.instance.currentLevelIndex);
        GameObject map = Instantiate(currentLevelData.map);
        moveLeft = currentLevelData.moveLimit;
        gameScene.UpdateMoveLeft(moveLeft);
        Time.timeScale = 1;
    }

    public void PlayerWinThisLevel()
    {
        if (isGameWin || isGameLose)
        {
            return;
        }
        LevelManager.instance.levelData.ReSetGivenLevelData(LevelManager.instance.currentLevelIndex, true, true);
        if (LevelManager.instance.levelData.GiveAllLevelAssigned().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetTheLevelAtGivenIndex(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.ReSetGivenLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        isGameWin = true;

        gameScene.PopupWinPanelGameScene();
        LevelManager.instance.levelData.SaveThisDataToJsonFile();
    }

    public void DecreaseMovementAndShowUI()
    {
        moveLeft--;
        gameScene.UpdateMoveLeft(moveLeft);
    }

    public void CheckMoveLeftToCheckLose()
    {
        if (moveLeft <= 0)
        {
            if (isGameWin || isGameLose)
            {
                return;
            }
            PlayerLoseThisLevelAndShowUI();
        }
    }

    public void PlayerLoseThisLevelAndShowUI()
    {
        isGameLose = true;
        gameScene.PopupLosePanelGameScene();
    }

    public bool IsThisGameFinalOrWin()
    {
        return isGameWin;
    }

    public bool IsThisGameFinalOrLose()
    {
        return isGameLose;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHolder : MonoBehaviour, IPointerClickHandler
{
    private const string GAME = "GameScene";
    [SerializeField]
    private Text levelIndexText;
    [SerializeField]
    private Image holderFilter;
    [SerializeField]
    private Sprite enabledLevel;
    [SerializeField]
    private CanvasGroup holderCG;

    private int levelIndex;

    private void Start()
    {
        levelIndexText.text = "Level " + (levelIndex + 1).ToString();
    }

    public void SetLevelIndex(int index)
    {
        levelIndex = index;
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LevelManager.instance.currentLevelIndex = levelIndex;
        ChangeToGameScene();
    }

    public void DisableHolder()
    {
        holderFilter.gameObject.SetActive(true);
        holderCG.interactable = false;
        holderCG.blocksRaycasts = false;
    }

    public void EnableHolder()
    {
        holderFilter.gameObject.SetActive(false);
        holderCG.interactable = true;
        holderCG.blocksRaycasts = true;
    }

    public void CompletedLevel()
    {
        Image enabled = this.transform.GetChild(0).GetComponent<Image>();
        enabled.sprite = enabledLevel;
        enabled.SetNativeSize();

    }

    public void ChangeToGameScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(GAME));
    }

    private IEnumerator ChangeScene(string sceneName)
    {

        //Optional: Add animation here
        LevelScene.instance.PlayChangeScene();
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadSceneAsync(sceneName);

    }
}
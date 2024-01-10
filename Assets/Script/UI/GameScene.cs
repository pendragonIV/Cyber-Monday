using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Transform bg;

    [SerializeField]
    private Transform navWin;
    [SerializeField]
    private Transform winScreen;
    [SerializeField]
    private Text moveLeft;
    [SerializeField]
    private Text levelName;

    private void Start()
    {
        levelName.text = "LEVEL " + (LevelManager.instance.currentLevelIndex + 1).ToString();
        bg.DOShakePosition(10f, 2f, 0, 0, false, true).SetEase(Ease.InSine).SetLoops(-1);
    }

    public void UpdateMoveLeft(int moveLeft)
    {
        this.moveLeft.text = moveLeft.ToString();
    }

    public void PopupWinPanelGameScene()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        winScreen.DOScaleY(0, 1f).SetUpdate(true).SetDelay(.3f).OnComplete(() =>
        {
            navWin.gameObject.SetActive(true);
        });
        FadePanelInScene(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
    }

    public void PopupLosePanelGameScene()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadePanelInScene(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
    }

    private void FadePanelInScene(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(1, .3f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}

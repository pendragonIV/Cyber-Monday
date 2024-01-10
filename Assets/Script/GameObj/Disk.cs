using DG.Tweening;
using UnityEngine;

public class Disk : MonoBehaviour
{
    private void Start()
    {
        this.transform.DOLocalMoveY(.2f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}

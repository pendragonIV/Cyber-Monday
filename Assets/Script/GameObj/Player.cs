using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private const float MOVE_DELAY = 0.2f;
    #region Movement variables
    [SerializeField]
    private Vector2 movementDirection;
    private bool isMoving = false;
    #endregion

    #region Animation
    [SerializeField]
    private Animator animator;
    #endregion

    private void Update()
    {
        if (GameManager.instance.IsThisGameFinalOrLose() || GameManager.instance.IsThisGameFinalOrWin() || isMoving)
        {
            return;
        }
        PlayerMovementControl();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            Debug.Log("Win");
            GameManager.instance.PlayerWinThisLevel();
            Destroy(collision.gameObject);
        }
    }

    #region Movement

    private void PlayerMovementControl()
    {
        if (movementDirection != Vector2.zero
            && !GameManager.instance.IsThisGameFinalOrLose()
            && !GameManager.instance.IsThisGameFinalOrWin()
            && !isMoving)
        {
            Vector3Int cellPos = GridCellManager.instance.GetCellPositionOfGivenPosition(transform.position);
            Vector3Int dir = Vector3Int.FloorToInt(movementDirection);
            Vector3Int nextCellPos = cellPos + dir;

            List<GameObject> nexts = CheckAllTheNextPuzzleBlock(cellPos, dir);

            if (nexts != null && nexts.Count > 1)
            {
                return;
            }
            else if (nexts != null)
            {
                if (GridCellManager.instance.IsThisAreaCanMoveTo(nextCellPos))
                {
                    foreach (GameObject next in nexts)
                    {
                        PushingThePuzzleBlockToSpecificPosition(next, dir);
                    }
                }
            }

            MovePlayerToSpecificPosition(nextCellPos);
        }
    }

    private void MovePlayerToSpecificPosition(Vector3Int nextCellPos)
    {
        if (GridCellManager.instance.IsThisAreaCanMoveTo(nextCellPos))
        {
            PlayPlayerJumpAndMoveAnimation();
            isMoving = true;
            GameManager.instance.DecreaseMovementAndShowUI();
            Vector3 moveto = GridCellManager.instance.GetWordPositionOfGivenCellPosition(nextCellPos);
            this.transform.DOMove(moveto, 0.5f).SetEase(Ease.Linear).SetDelay(MOVE_DELAY).OnComplete(() =>
            {
                isMoving = false;
                GameManager.instance.CheckMoveLeftToCheckLose();
            });
        }
    }

    private void PushingThePuzzleBlockToSpecificPosition(GameObject pushingBlock, Vector3Int dir)
    {
        Vector3Int cellPos = GridCellManager.instance.GetCellPositionOfGivenPosition(pushingBlock.transform.position);
        Vector3Int nextCell = cellPos + dir;

        pushingBlock.transform.DOMove(GridCellManager.instance.GetWordPositionOfGivenCellPosition(nextCell), 0.5f).SetEase(Ease.Linear).SetDelay(MOVE_DELAY).OnComplete(() =>
        {
            Block block = pushingBlock.GetComponent<Block>();
            block.CenterAllChildInThisBlock();
            if (block.CheckIfThisBLockCanFullyPlaced(dir))
            {
                pushingBlock.GetComponent<Block>().SetThisBlockToGround();
            }
        });
    }

    #endregion

    #region Checking

    private List<GameObject> CheckAllTheNextPuzzleBlock(Vector3Int start, Vector3Int dir)
    {
        List<GameObject> nexts = new List<GameObject>();
        Vector3Int checkCell = start + dir;
        Vector3 checkPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(checkCell);
        Collider2D next = Physics2D.OverlapPoint(checkPos, LayerMask.GetMask("Block"));
        if (next)
        {
            nexts = next.GetComponent<Block>().CheckIfThereIsBlockNextToThisBlock(next.gameObject, dir);
        }

        if (nexts.Count > 0)
        {
            return nexts;
        }
        return null;
    }


    #endregion

    private void PlayPlayerJumpAndMoveAnimation()
    {
        if (animator != null)
        {
            animator.Play("Computer", -1, 0f);
        }
    }

    #region Input System
    private void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
    }
    #endregion
}

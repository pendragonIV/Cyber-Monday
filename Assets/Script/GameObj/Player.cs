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
        if (GameManager.instance.IsGameLose() || GameManager.instance.IsGameWin() || isMoving)
        {
            return;
        }
        MovementManager();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            Debug.Log("Win");   
            GameManager.instance.Win();
            Destroy(collision.gameObject);
        }
    }

    #region Movement

    private void MovementManager()
    {
        if (movementDirection != Vector2.zero
            && !GameManager.instance.IsGameLose()
            && !GameManager.instance.IsGameWin()
            && !isMoving)
        {
            Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
            Vector3Int dir = Vector3Int.FloorToInt(movementDirection);
            Vector3Int nextCellPos = cellPos + dir;

            List<GameObject> nexts = CheckNextBlock(cellPos,dir);

            if (nexts != null && nexts.Count > 1)
            {
                return;
            }
            else if(nexts != null)
            {
                if (GridCellManager.instance.IsPlaceableArea(nextCellPos))
                {
                    foreach (GameObject next in nexts)
                    {
                        Push(next, dir);
                    }
                }
            }

            Move(nextCellPos);
        }
    }

    private void Move(Vector3Int nextCellPos)
    {
        if (GridCellManager.instance.IsPlaceableArea(nextCellPos))
        {
            PlayAnimation();
            isMoving = true;
            GameManager.instance.DecreaseMove();
            Vector3 moveto = GridCellManager.instance.PositonToMove(nextCellPos);
            this.transform.DOMove(moveto, 0.5f).SetEase(Ease.Linear).SetDelay(MOVE_DELAY).OnComplete(() =>
            {
                isMoving = false;
                GameManager.instance.CheckMove();
            });
        }
    }

    private void Push(GameObject pushingBlock, Vector3Int dir)
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(pushingBlock.transform.position);
        Vector3Int nextCell = cellPos + dir;

        pushingBlock.transform.DOMove(GridCellManager.instance.PositonToMove(nextCell), 0.5f).SetEase(Ease.Linear).SetDelay(MOVE_DELAY).OnComplete(() =>
        {
            Block block = pushingBlock.GetComponent<Block>();
            block.CenterBlock();
            if (block.IsBlockCanPlaced(dir))
            {
                pushingBlock.GetComponent<Block>().SetStaticBlock();
            }
        }); 
    }

    #endregion

    #region Checking

    private List<GameObject> CheckNextBlock(Vector3Int start ,Vector3Int dir)
    {
        List<GameObject> nexts = new List<GameObject>();
        Vector3Int checkCell = start + dir;
        Vector3 checkPos = GridCellManager.instance.PositonToMove(checkCell);
        Collider2D next = Physics2D.OverlapPoint(checkPos, LayerMask.GetMask("Block"));
        if (next)
        {
            nexts = next.GetComponent<Block>().CheckNeigorBlock(next.gameObject, dir);
        }
        
        if(nexts.Count > 0)
        {
            return nexts;
        }
        return null;
    }


    #endregion

    private void PlayAnimation()
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

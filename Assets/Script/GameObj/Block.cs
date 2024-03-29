
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Block : MonoBehaviour
{
    [SerializeField]
    private Color disabledColor;
    [SerializeField]
    private Sprite[] defaultColors;
    [SerializeField]
    private Collider2D[] colliders;

    private void Start()
    {
        SetRamdomColorToThisPuzzleBlock();
    }

    private void SetRamdomColorToThisPuzzleBlock()
    {
        int random = Random.Range(0, defaultColors.Length);
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = defaultColors[random];
        }
    }

    public void SetThisBlockToGround()
    {
        this.GetComponent<SortingGroup>().sortingOrder = 0;
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (Transform child in transform)
        {
            child.GetComponent<CenterCell>().PutObjectToCenterCell();
            Vector3Int cellPos = GridCellManager.instance.GetCellPositionOfGivenPosition(child.position);
            GridCellManager.instance.SetPlacedBlockSoPlayerCanMove(cellPos);
            child.GetComponent<SpriteRenderer>().color = disabledColor;
        }
    }

    public void CenterAllChildInThisBlock()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<CenterCell>().PutObjectToCenterCell();
        }
    }

    public bool CheckIfThisBLockCanFullyPlaced(Vector3Int dir)
    {
        foreach (Transform child in transform)
        {
            Vector3Int cellPos = GridCellManager.instance.GetCellPositionOfGivenPosition(child.position);
            if (GridCellManager.instance.IsThisAreaCanMoveTo(cellPos))
            {
                return false;
            }
        }
        return true;
    }


    public List<GameObject> CheckIfThereIsBlockNextToThisBlock(GameObject objToCheck, Vector3Int dir)
    {
        List<GameObject> nexts = new List<GameObject>();
        nexts.Add(objToCheck);
        foreach (Transform child in objToCheck.transform)
        {
            Vector3Int cellPos = GridCellManager.instance.GetCellPositionOfGivenPosition(child.position);
            Vector3Int nextCell = cellPos + dir;
            Vector3 nextPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(nextCell);
            Collider2D next = Physics2D.OverlapPoint(nextPos, LayerMask.GetMask("Block"));
            if (next)
            {
                if (!nexts.Contains(next.gameObject))
                {
                    nexts.Add(next.gameObject);
                }
            }
        }

        return nexts;
    }
}


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

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
        SetDefaultColor();
    }

    private void SetDefaultColor()
    {
        int random = Random.Range(0, defaultColors.Length);
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = defaultColors[random];
        }
    }

    public void SetStaticBlock()
    {
        this.GetComponent<SortingGroup>().sortingOrder = 0;
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (Transform child in transform)
        {
            child.GetComponent<CenterCell>().SetCenterCell();
            Vector3Int cellPos = GridCellManager.instance.GetObjCell(child.position);
            GridCellManager.instance.SetPlacedBlock(cellPos);
            child.GetComponent<SpriteRenderer>().color = disabledColor;
        }
    }

    public void CenterBlock()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<CenterCell>().SetCenterCell();
        }
    }

    public bool IsBlockCanPlaced(Vector3Int dir)
    {
        foreach (Transform child in transform)
        {
            Vector3Int cellPos = GridCellManager.instance.GetObjCell(child.position);
            if (GridCellManager.instance.IsPlaceableArea(cellPos))
            {
                return false;
            }
        }
        return true;
    }


    public List<GameObject> CheckNeigorBlock(GameObject objToCheck, Vector3Int dir)
    {
        List<GameObject> nexts = new List<GameObject>();
        nexts.Add(objToCheck);
        foreach(Transform child in objToCheck.transform)
        {
            Vector3Int cellPos = GridCellManager.instance.GetObjCell(child.position);
            Vector3Int nextCell = cellPos + dir;
            Vector3 nextPos = GridCellManager.instance.PositonToMove(nextCell);
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

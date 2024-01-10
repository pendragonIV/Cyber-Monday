using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private List<Vector3> locations = new List<Vector3>();
    [SerializeField]
    private List<Vector3Int> placedBlock = new List<Vector3Int>();

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void SetMapForThisManager(Tilemap tilemap)
    {
        if (tilemap != null)
        {
            this.tileMap = tilemap;
        }
    }

    public void SetPlacedBlockSoPlayerCanMove(Vector3Int cellPos)
    {
        placedBlock.Add(cellPos);
    }

    public bool IsThisAreaCanMoveTo(Vector3Int cellPos)
    {
        if (tileMap.GetTile(cellPos) == null && !placedBlock.Contains(cellPos))
        {
            return false;
        }
        return true;
    }

    public Vector3Int GetCellPositionOfGivenPosition(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 GetWordPositionOfGivenCellPosition(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }
}

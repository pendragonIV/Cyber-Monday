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
    public void SetTileMap(Tilemap tilemap)
    {
        if(tilemap != null)
        {
            this.tileMap = tilemap;
        }
    }

    private void GetTiles()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);

                Vector3 location = tileMap.GetCellCenterWorld(localLocation);
                if (tileMap.HasTile(localLocation))
                {
                    locations.Add(location);
                }
            }
        }
    }

    public void SetPlacedBlock(Vector3Int cellPos)
    {
        placedBlock.Add(cellPos);
    }

    public bool IsPlaceableArea(Vector3Int cellPos)
    {
        if (tileMap.GetTile(cellPos) == null && !placedBlock.Contains(cellPos))
        {
            return false;
        }
        return true;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SlimeTiles
{
    public Dictionary<Vector3Int, bool> slimeTilesGridDict = new Dictionary<Vector3Int, bool>();
    public bool isActive;
    public Tilemap slimeTileMap;
    public TileBase slimeTileBase;
    public TileBase originalTileBase;
    public Coroutine durationRoutine;

    public SlimeTiles(bool isActive, Tilemap slimeTileMap, TileBase slimeTileBase, Coroutine durationRoutine)
    {
        this.isActive = isActive;
        this.slimeTileMap = slimeTileMap;
        this.slimeTileBase = slimeTileBase;
        this.durationRoutine = durationRoutine;
    }
}

public class MapManager : MonoBehaviour
{

    public Tilemap[] slimeAffectedTilemaps;
    public Tilemap slimeTileMap;
    public TileBase slimeTileBase;

    private Dictionary<Tilemap, SlimeTiles> slimeTileMapDict = new Dictionary<Tilemap, SlimeTiles>();


    private void Awake()
    {
        foreach (Tilemap tilemap in slimeAffectedTilemaps)
        {
            SlimeTiles slimeTiles = new SlimeTiles(false, slimeTileMap, slimeTileBase, null);
            slimeTileMapDict.Add(tilemap, slimeTiles);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (Tilemap tilemap in slimeAffectedTilemaps)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = tilemap.WorldToCell(mousePos);

                TileBase clickedTile = tilemap.GetTile(gridPos);

                print("At position " + gridPos + " there's a " + clickedTile);
            }
        }
    }

    public void ChangeTileToSlimeTile(Vector2 worldPosition)
    {
        foreach (Tilemap slimeAffectedTilemap in slimeAffectedTilemaps)
        {
            SlimeTiles slimeTiles = slimeTileMapDict[slimeAffectedTilemap];
            Vector3Int gridPosition = slimeAffectedTilemap.WorldToCell(worldPosition);

            if (slimeTiles != null && slimeAffectedTilemap.GetTile(gridPosition))
            {
                if (!slimeTiles.slimeTilesGridDict.ContainsKey(gridPosition))
                {
                    slimeTiles.slimeTilesGridDict.Add(gridPosition, slimeTiles.isActive);

                    slimeTiles.originalTileBase = slimeAffectedTilemap.GetTile(gridPosition);
                    slimeTileMap.SetTile(gridPosition, slimeTiles.slimeTileBase);
                    slimeAffectedTilemap.SetTile(gridPosition, null);
                    slimeTiles.slimeTilesGridDict[gridPosition] = true;
                }

                if (slimeTiles.slimeTilesGridDict[gridPosition] == false)
                {
                    slimeTiles.originalTileBase = slimeAffectedTilemap.GetTile(gridPosition);
                    slimeTileMap.SetTile(gridPosition, slimeTiles.slimeTileBase);
                    slimeAffectedTilemap.SetTile(gridPosition, null);
                    slimeTiles.slimeTilesGridDict[gridPosition] = true;
                }
            }
        }
    }

    private IEnumerator RevertTileAfterDuration(Tilemap originalTilemap, Vector3Int gridPosition, float duration)
    {
        yield return new WaitForSeconds(duration);

    }
}

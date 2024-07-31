using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SlimeTiles
{
    public Dictionary<Vector3Int, bool> slimeTilesGridDict = new Dictionary<Vector3Int, bool>();
    public Dictionary<Vector3Int, Coroutine> slimeTilesRoutineDict = new Dictionary<Vector3Int, Coroutine>();

    public bool isActive;
    public Tilemap slimeTileMap;
    public TileBase slimeTileBase;
    public TileBase originalTileBase;

    public SlimeTiles(bool isActive, Tilemap slimeTileMap, TileBase slimeTileBase)
    {
        this.isActive = isActive;
        this.slimeTileMap = slimeTileMap;
        this.slimeTileBase = slimeTileBase;
    }
}

public class MapManager : MonoBehaviour
{

    public Tilemap[] slimeAffectedTilemaps;
    public Tilemap slimeTileMap;
    public TileBase slimeTileBase;
    public float slimeTileDuration;

    private Dictionary<Tilemap, SlimeTiles> slimeTileMapDict = new Dictionary<Tilemap, SlimeTiles>();



    private void Awake()
    {
        foreach (Tilemap tilemap in slimeAffectedTilemaps)
        {
            SlimeTiles slimeTiles = new SlimeTiles(false, slimeTileMap, slimeTileBase);
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

                //print("At position " + gridPos + " there's a " + clickedTile);
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
                    slimeTiles.slimeTilesRoutineDict.Add(gridPosition, null);

                }

                slimeTiles.originalTileBase = slimeAffectedTilemap.GetTile(gridPosition);
                slimeTileMap.SetTile(gridPosition, slimeTiles.slimeTileBase);
                //slimeAffectedTilemap.SetTile(gridPosition, null);
                slimeTiles.slimeTilesGridDict[gridPosition] = true;

                if (slimeTiles.slimeTilesRoutineDict[gridPosition] == null)
                {
                    slimeTiles.slimeTilesRoutineDict[gridPosition] =
                        StartCoroutine(RevertTileAfterDuration(slimeTiles, slimeAffectedTilemap, gridPosition, slimeTileDuration));
                }
                else
                {
                    StopCoroutine(slimeTiles.slimeTilesRoutineDict[gridPosition]);
                    slimeTiles.slimeTilesRoutineDict[gridPosition] =
                        StartCoroutine(RevertTileAfterDuration(slimeTiles, slimeAffectedTilemap, gridPosition, slimeTileDuration));
                }
                
            }
        }
    }

    private IEnumerator RevertTileAfterDuration(SlimeTiles slimeTiles, Tilemap tilemap, Vector3Int gridPosition, float duration)
    {
        yield return new WaitForSeconds(duration);

        slimeTileMap.SetTile(gridPosition, null);
        slimeTiles.slimeTilesGridDict[gridPosition] = false;
        //tilemap.SetTile(gridPosition, slimeTiles.originalTileBase);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WallType
{
    None,
    N,
    E,
    S,
    W,
    NW,
    NE,
    SE,
    SW,
    NDoor,
    EDoor,
    SDoor,
    WDoor
}

public class WallInfo
{
    public int rotate { get; set; }
    public GameObject wall { get; set; }
}

public class ManorBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject floor;
    [SerializeField]
    private GameObject floorParent;

    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject corner;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject wallsParent;

    // This reads bottom to top (as in South appears first in the array)
    private WallType[,] walls = new WallType[,] {
        { WallType.SW, WallType.S, WallType.S, WallType.S, WallType.SE },
        { WallType.W, WallType.None, WallType.None, WallType.None, WallType.E },
        { WallType.W, WallType.None, WallType.None, WallType.None, WallType.E },
        { WallType.W, WallType.None, WallType.None, WallType.None, WallType.E },
        { WallType.NW, WallType.N, WallType.NDoor, WallType.N, WallType.NE },
        { WallType.None, WallType.None, WallType.None, WallType.None, WallType.None }
    };

    Dictionary<WallType, WallInfo> wallDefinitions;

    void Awake()
    {
        PopulateWallDefinitions();

        Vector3 offsetToCentre = new Vector3(walls.GetLength(1) / 2.0f, 0f, walls.GetLength(0) / 2.0f);
        Vector3 offsetToTile = new Vector3(0.5f, 0f, 0.5f);

        for (int x = 0; x < walls.GetLength(1); x++)
        {
            for (int y = 0; y < walls.GetLength(0); y++)
            {
                Instantiate(
                    floor,
                    new Vector3(x, 0f, y) + transform.position - offsetToCentre,
                    Quaternion.identity,
                    floorParent.transform
                );

                GameObject wallToInstantiate = wallDefinitions[walls[y, x]].wall;
                int rotate = wallDefinitions[walls[y, x]].rotate;

                if (wallToInstantiate != null)
                {
                    Instantiate(
                        wallToInstantiate,
                        new Vector3(x, 0f, y) + transform.position - offsetToCentre + offsetToTile,
                        Quaternion.AngleAxis(rotate, Vector3.up),
                        wallsParent.transform
                    );
                }
            }
        }

    }

    void PopulateWallDefinitions()
    {
        wallDefinitions = new Dictionary<WallType, WallInfo>();
        wallDefinitions[WallType.None] = new WallInfo() { rotate = 0, wall = null };

        wallDefinitions[WallType.N] = new WallInfo() { rotate = 180, wall = wall };
        wallDefinitions[WallType.E] = new WallInfo() { rotate = -90, wall = wall };
        wallDefinitions[WallType.S] = new WallInfo() { rotate = 0, wall = wall };
        wallDefinitions[WallType.W] = new WallInfo() { rotate = 90, wall = wall };

        wallDefinitions[WallType.NW] = new WallInfo() { rotate = 180, wall = corner };
        wallDefinitions[WallType.NE] = new WallInfo() { rotate = -90, wall = corner };
        wallDefinitions[WallType.SE] = new WallInfo() { rotate = 0, wall = corner };
        wallDefinitions[WallType.SW] = new WallInfo() { rotate = 90, wall = corner };

        wallDefinitions[WallType.NDoor] = new WallInfo() { rotate = 180, wall = door };
        wallDefinitions[WallType.EDoor] = new WallInfo() { rotate = -90, wall = door };
        wallDefinitions[WallType.SDoor] = new WallInfo() { rotate = 0, wall = door };
        wallDefinitions[WallType.WDoor] = new WallInfo() { rotate = 90, wall = door };
    }
}

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
    void Awake()
    {
        Vector3 offsetToCentre = new Vector3(walls.GetLength(1) / 2.0f, 0f, walls.GetLength(0) / 2.0f);
        Vector3 offsetToTile = new Vector3(0.5f, 0f, 0.5f);
        Debug.Log(offsetToCentre);
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

                // yes this is really smelly game-jam code!
                GameObject wallToInstantiate = null;
                int rotate = 0;
                if (walls[y, x] == WallType.N)
                {
                    wallToInstantiate = wall;
                    rotate = 180;
                }
                else if (walls[y, x] == WallType.E)
                {
                    wallToInstantiate = wall;
                    rotate = -90;
                }
                else if (walls[y, x] == WallType.S)
                {
                    wallToInstantiate = wall;
                    rotate = 0;
                }
                else if (walls[y, x] == WallType.W)
                {
                    wallToInstantiate = wall;
                    rotate = 90;
                }
                else if (walls[y, x] == WallType.NDoor)
                {
                    wallToInstantiate = door;
                    rotate = 180;
                }
                else if (walls[y, x] == WallType.EDoor)
                {
                    wallToInstantiate = door;
                    rotate = -90;
                }
                else if (walls[y, x] == WallType.SDoor)
                {
                    wallToInstantiate = door;
                    rotate = 0;
                }
                else if (walls[y, x] == WallType.WDoor)
                {
                    wallToInstantiate = door;
                    rotate = 90;
                }
                else if (walls[y, x] == WallType.NE)
                {
                    wallToInstantiate = corner;
                    rotate = -90;
                }
                else if (walls[y, x] == WallType.NW)
                {
                    wallToInstantiate = corner;
                    rotate = 180;
                }
                else if (walls[y, x] == WallType.SW)
                {
                    wallToInstantiate = corner;
                    rotate = 90;
                }
                else if (walls[y, x] == WallType.SE)
                {
                    wallToInstantiate = corner;
                    rotate = 0;
                }

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
}

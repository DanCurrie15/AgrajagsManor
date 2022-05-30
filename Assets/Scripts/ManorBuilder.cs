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
    WDoor,
    NWindow,
    EWindow,
    SWindow,
    WWindow,
    NWindowSlide,
    EWindowSlide,
    SWindowSlide,
    WWindowSlide
}

public class WallInfo
{
    public int rotate { get; set; }
    public GameObject wall { get; set; }
}

[System.Serializable]
public struct Furniture
{
    public GameObject prefab;
    public Vector3 position;
    public float rotation;  // around y
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
    private GameObject window;
    [SerializeField]
    private GameObject windowSlide;
    [SerializeField]
    private GameObject wallsParent;

    [SerializeField]
    private List<Furniture> furniture;
    private List<GameObject> furnitureInstances;
    [SerializeField]
    private GameObject furnitureParent;
    private bool initialFurniture = true;

    // This reads bottom to top (as in South appears first in the array)
    private WallType[,] originalWalls = new WallType[,] {
        { WallType.SW, WallType.S, WallType.SWindowSlide, WallType.S, WallType.SE },
        { WallType.W, WallType.None, WallType.None, WallType.None, WallType.EWindow },
        { WallType.W, WallType.None, WallType.None, WallType.None, WallType.E },
        { WallType.W, WallType.None, WallType.None, WallType.None, WallType.EWindow },
        { WallType.NW, WallType.N, WallType.NDoor, WallType.N, WallType.NE },
        { WallType.None, WallType.None, WallType.None, WallType.None, WallType.None }
    };

    private WallType[,] walls = new WallType[,] {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, WallType.SW, WallType.SWindow, WallType.SE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.SW, WallType.SWindow, WallType.SE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, WallType.SW, 0, 0, 0, WallType.SE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.SW, 0, 0, 0, WallType.SE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, WallType.WWindow, 0, 0, 0, WallType.EWindow, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.WWindow, 0, 0, 0, WallType.E, WallType.SW, WallType.SWindow, WallType.SDoor, WallType.SWindow, WallType.SE, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.EDoor, WallType.WDoor, 0, 0, 0, WallType.EWindow, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.E, WallType.NW, WallType.N, WallType.NDoor, WallType.N, WallType.NE, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.E, WallType.SW, WallType.SDoor, WallType.SWindow, WallType.S, WallType.SWindow, WallType.SDoor, WallType.SDoor, WallType.SDoor, WallType.SWindow, WallType.S, WallType.SWindow, WallType.SDoor, WallType.SE, WallType.W, 0, 0, 0, WallType.E, WallType.SW, WallType.S, WallType.SDoor, WallType.S, WallType.S, WallType.SE, 0, 0, 0, 0},
        {0, WallType.SW, WallType.SWindow, WallType.SWindow, WallType.SE, WallType.NW, WallType.N, WallType.NDoor, WallType.N, WallType.NE, WallType.W, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.E, WallType.NW, WallType.N, WallType.NDoor, WallType.N, WallType.NE, WallType.W, 0, 0, 0, 0, WallType.EWindowSlide, 0, 0, 0, 0},
        {0, WallType.WWindow, 0, 0, WallType.E, WallType.SW, WallType.S, WallType.SDoor, WallType.S, WallType.SE, WallType.W, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.E, WallType.SW, WallType.S, WallType.SDoor, WallType.S, WallType.SE, WallType.NW, WallType.NDoor, WallType.N, WallType.N, WallType.N, WallType.NE, 0, 0, 0, 0},
        {0, WallType.WDoor, 0, 0, WallType.EDoor, WallType.WDoor, 0, 0, 0, WallType.EDoor, WallType.WDoor, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.EDoor, WallType.WDoor, 0, 0, 0, WallType.EDoor, 0, 0, 0, 0, WallType.SW, WallType.S, WallType.S, WallType.SWindowSlide, WallType.SE, 0},
        {0, WallType.WWindow, 0, 0, WallType.E, WallType.NW, WallType.N, WallType.NDoor, WallType.N, WallType.NE, WallType.W, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.E, WallType.NW, WallType.N, WallType.NDoor,WallType.N, WallType.NE, 0, 0, 0, 0, WallType.WDoor, 0, 0, 0, WallType.EDoor, 0},
        {0, WallType.NW, WallType.NWindow, WallType.NWindow, WallType.NE, WallType.SW, WallType.S, WallType.SDoor, WallType.S, WallType.SE, WallType.NW, WallType.N, WallType.N, WallType.N, WallType.N, WallType.N, 0, WallType.N, WallType.N, WallType.N, WallType.N, WallType.N, WallType.NE, WallType.SW, WallType.S, WallType.SDoor, WallType.S, WallType.SE, WallType.SW, WallType.SDoor, WallType.SDoor, WallType.SE, WallType.W, 0, 0, 0, WallType.EWindowSlide, 0},
        {0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.E, WallType.SW, WallType.SE, WallType.SW, WallType.S, WallType.SE, WallType.SW, 0, WallType.SE, 0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.E, WallType.NW, WallType.NDoor, WallType.NDoor, WallType.NE, WallType.W, 0, 0, 0, WallType.EWindowSlide, 0},
        {0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.E, WallType.W, WallType.EDoor, WallType.WDoor, 0, WallType.EDoor, WallType.WDoor, 0, 0, WallType.S, WallType.S, WallType.SDoor, WallType.S, WallType.SE, WallType.W, 0, 0, 0, WallType.E, 0, 0, 0, 0, WallType.NW, WallType.NDoor, WallType.N, WallType.NWindowSlide, WallType.NE, 0},
        {0, 0, 0, 0, 0, WallType.W, 0, 0, 0, WallType.E, WallType.NW, WallType.NE, WallType.NW, WallType.NWindowSlide, WallType.NE, WallType.NW, 0, WallType.NE, WallType.NW, WallType.NWindowSlide, WallType.NWindowSlide, WallType.NWindowSlide, WallType.NE, WallType.W, 0, 0, 0, WallType.E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, WallType.WWindow, 0, 0, 0, WallType.EWindow, 0, 0, 0, 0, 0, WallType.WWindow, 0, WallType.EWindow, 0, 0, 0, 0, 0, WallType.WWindow, 0, 0, 0, WallType.EWindow, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, WallType.NW, 0, 0, 0, WallType.NE, 0, 0, 0, 0, 0, WallType.WWindow, 0, WallType.EWindow, 0, 0, 0, 0, 0, WallType.NW, 0, 0, 0, WallType.NE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, WallType.NW, WallType.NWindow, WallType.NE, 0, 0, 0, 0, 0, 0, WallType.WWindow, 0, WallType.EWindow, 0, 0, 0, 0, 0, 0, WallType.NW, WallType.NWindow, WallType.NE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.SW, WallType.S, 0, WallType.S, WallType.SE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.WWindow, 0, 0, 0, WallType.EWindow, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.WWindow, 0, 0, 0, WallType.EWindow, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.WWindow, 0, 0, 0, WallType.EWindow, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, WallType.NW, WallType.NWindow, WallType.NWindow, WallType.NWindow, WallType.NE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    };

    Dictionary<WallType, WallInfo> wallDefinitions;

    void Awake()
    {
        furnitureInstances = new List<GameObject>();
        PopulateWallDefinitions();

        Vector3 offsetToCentre = new Vector3(walls.GetLength(1) / 2.0f, 0f, walls.GetLength(0) / 2.0f);
        Vector3 offsetToTile = new Vector3(0.5f, 0f, 0.5f);

        Debug.Log("Manor floor size: " + walls.GetLength(1) + "x" + walls.GetLength(0));

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

        wallDefinitions[WallType.NWindow] = new WallInfo() { rotate = 180, wall = window };
        wallDefinitions[WallType.EWindow] = new WallInfo() { rotate = -90, wall = window };
        wallDefinitions[WallType.SWindow] = new WallInfo() { rotate = 0, wall = window };
        wallDefinitions[WallType.WWindow] = new WallInfo() { rotate = 90, wall = window };

        wallDefinitions[WallType.NWindowSlide] = new WallInfo() { rotate = 180, wall = windowSlide };
        wallDefinitions[WallType.EWindowSlide] = new WallInfo() { rotate = -90, wall = windowSlide };
        wallDefinitions[WallType.SWindowSlide] = new WallInfo() { rotate = 0, wall = windowSlide };
        wallDefinitions[WallType.WWindowSlide] = new WallInfo() { rotate = 90, wall = windowSlide };
    }

    void FixedUpdate() {
        // TODO: this would be better to compile out at build time.
        // only create new instances when they change
        if (
            (GameManager.Instance.gameMode == GameMode.EditMode && furnitureInstances.Count != furniture.Count) ||
            (GameManager.Instance.gameMode == GameMode.PlayMode && initialFurniture)
        ) {
            initialFurniture = false;
            int currentInstances = furnitureInstances.Count;
            for (int i = 0; i < currentInstances; i++)
            {
                GameObject item = furnitureInstances[0];
                furnitureInstances.Remove(item);
                Destroy(item);
            }

            foreach (Furniture item in furniture)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.transform.parent = furnitureParent.transform;
                obj.transform.position = item.position;
                obj.transform.rotation = Quaternion.AngleAxis(item.rotation, Vector3.up);
                furnitureInstances.Add(obj);
            }

            PlayerManager.Instance.ReloadPlayerOptions();
        }

        if (GameManager.Instance.gameMode == GameMode.EditMode) {
            // always ensure our locations and rotations are up to date.
            for (int i = 0; i < furnitureInstances.Count; i++) {
                Furniture item = furniture[i];
                GameObject obj = furnitureInstances[i];

                obj.transform.position = item.position;
                obj.transform.rotation = Quaternion.AngleAxis(item.rotation, Vector3.up);
            }
        }
    }
}

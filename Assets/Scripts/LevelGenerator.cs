using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    float roomWidth = 9f;
    float roomHeight = 5f;
    [SerializeField] int levelWidth = 0;
    [SerializeField] int levelHeight = 0;

    [SerializeField] GameObject framePrefab = null;
    [SerializeField] GameObject criticalPathPrefab = null;

    List<GameObject> roomFrames = new List<GameObject>();
    List<Vector2Int> criticalPath = new();

    
    void Start()
    {
        MakeLevel();
    }

    private void MakeLevel()
    {
        ClearRooms();
        GenerateCriticalPath();
        CreateCriticalPath();
        PopulatePathWithRooms();
        FillEmptyTiles();
        AddRandomElementsToRooms();
    }


    /// <summary>
    /// Instantiate and connect all the rooms in the criticalPath list.
    /// </summary>
    private void CreateCriticalPath()
    {
        GameObject previousRoom = null;
        for (int i = 0; i < criticalPath.Count; i++)
        {
            Vector2Int currentPos = criticalPath[i];
            GameObject currentRoom = InstantiateRoomFrame(currentPos, criticalPathPrefab);
            
            if (previousRoom != null)
            {
                Vector2Int previousPos = criticalPath[i - 1];
                ConnectRooms(previousRoom, currentRoom);
            }
            previousRoom = currentRoom;
        }
    }

    /// <summary>
    /// Make sure each room has an open path to the other
    /// </summary>
    private void ConnectRooms(GameObject previousRoom, GameObject currentRoom)
    {
        var previousPos = previousRoom.transform.position;
        var currentPos = currentRoom.transform.position;

        var previousRoomFrame = previousRoom.GetComponent<RoomFrame>();
        var currentRoomFrame = currentRoom.GetComponent<RoomFrame>();

        if (previousPos.y == currentPos.y)
        {
            if(previousPos.x < currentPos.x)
            {
                // current room is to the left or previous room
                currentRoomFrame.LeftDoorClosed = false;
                previousRoomFrame.RightDoorClosed = false;

            } else if (previousPos.x > currentPos.x)
            {
                // current room is to the right or previous room
                currentRoomFrame.RightDoorClosed = false;
                previousRoomFrame.LeftDoorClosed = false;
            }
        } else
        {
            // current room is above previous room
            currentRoomFrame.BottomDoorClosed = false;
            previousRoomFrame.TopDoorClosed = false;
        }
    }

    private void AddRandomElementsToRooms()
    {
        // Add random blocks of stuff
        //foreach (GameObject ro in rooms)
        //{ 
        //}
    }

    /// <summary>
    /// Instantiate rooms around the critical path
    /// </summary>
    private void FillEmptyTiles()
    {
        for (int x = 0; x < levelWidth; x++) 
        { 
            for(int y = 0; y < levelHeight; y++)
            {
                Vector2Int currentTile = new Vector2Int(x, y);
                if (criticalPath.Contains(currentTile))
                    continue;

                InstantiateRoomFrame(currentTile, framePrefab);
            }
        }
    }

    private void PopulatePathWithRooms()
    {
        // Spawn rooms inside the room frames
        //foreach (Vector2Int cp in criticalPath) 
        //{ 
        //}
    }

    /// <summary>
    /// Generate a valid path through the level where the player can walk.
    /// </summary>
    private void GenerateCriticalPath()
    {
        // We start at a random x column in the top y row
        var x = Random.Range(0, levelWidth);
        Vector2Int roomPosition = new(x, 0);
        criticalPath.Add(roomPosition);

        int count = 0;
        while (true)
        {
            if (count > levelWidth * levelHeight)
            {
                Debug.LogError("Endless Loop Detected");
                break;
            }
            Vector2Int dir = GetValidDirectionFrom(roomPosition);
            // Break early
            if (dir == Vector2Int.zero)
            {
                break;
            }

            roomPosition = roomPosition + dir;
            criticalPath.Add(roomPosition);

            count++;
        }
    }

    /// <summary>
    /// Instantiate a RoomFrame at the given grid position
    /// </summary>
    private GameObject InstantiateRoomFrame(Vector2Int gridPosition, GameObject prefab)
    {
        // Return early
        if (prefab == null)
        {
            Debug.LogWarning("No room prefab found");
            return null;
        }

        // Convert Grid to World Position
        var worldPosition = new Vector2();
        worldPosition.x = gridPosition.x * roomWidth;
        worldPosition.y = gridPosition.y * roomHeight;

        var o = Instantiate(prefab, worldPosition, Quaternion.identity);
        roomFrames.Add(o);
        return o;
    }

    /// <summary>
    /// Returns a random Vector2Int direction or Vector2Int.zero if no valid direction was found
    /// </summary>
    private Vector2Int GetValidDirectionFrom(Vector2Int fromPosition)
    {
        // List to store valid directions
        List<Vector2Int> validDirections = new();


        // Check right direction
        if (fromPosition.x < levelWidth - 1 && !criticalPath.Contains(fromPosition + Vector2Int.right))
        {
            validDirections.Add(Vector2Int.right);
        }

        // Check left direction
        if (fromPosition.x > 0 && !criticalPath.Contains(fromPosition + Vector2Int.left))
        {
            validDirections.Add(Vector2Int.left);
        }

        // Check up direction
        if (fromPosition.y < levelHeight - 1 && !criticalPath.Contains(fromPosition + Vector2Int.up))
        {
            validDirections.Add(Vector2Int.up);
        }


        // Return early if no valid directions found
        if (validDirections.Count == 0)
        { 
            return Vector2Int.zero; 
        }

        // Choose a random valid direction
        int randomIndex = Random.Range(0, validDirections.Count);
        return validDirections[randomIndex];
    }

    /// <summary>
    /// Destroy all the gameobjects instantiated.
    /// </summary>
    private void ClearRooms()
    {
        foreach(var o in roomFrames)
            DestroyImmediate(o);

        roomFrames.Clear();
    }
}

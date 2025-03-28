using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEngine;

public class WorldModelInstance : MonoBehaviour
{
    public static WorldModelInstance Instance { get; private set; }

    WorldModel worldModel;
    public List<Station> stations;
    public Station GetStation(int id) => stations[id];

    public Dictionary<int, Dictionary<int, Dictionary<int, GridObjectInstance>>> allInstances = new();
    public void AddInstance(int x, int y, int z, GridObjectInstance gridObjectInstance, Collider collider)
    {
        if (!allInstances.ContainsKey(x))
        {
            allInstances[x] = new();
        }
        if (!allInstances[x].ContainsKey(y))
        {
            allInstances[x][y] = new();
        }
        allInstances[x][y][z] = gridObjectInstance;
        instanceColliders[collider] = gridObjectInstance;
    }

    public GridObjectInstance WhereAmI(Vector3 position)
    {
        Vector3Int pos = WhereAmIVectors(position);
        return allInstances[pos.x][pos.y][pos.z];
    }
    Vector3Int WhereAmIVectors(Vector3 position)
    {
        position /= WorldModel.SIZE_OF_GRID_CUBE;
        int x = Mathf.Clamp(Mathf.RoundToInt(position.x), -WorldModel.HalfWidth, WorldModel.HalfWidth);
        int y = Mathf.Clamp(Mathf.RoundToInt(position.y), -WorldModel.HalfWidth, WorldModel.HalfWidth);
        int z = Mathf.Clamp(Mathf.RoundToInt(position.z), -WorldModel.HalfWidth, WorldModel.HalfWidth);
        return new Vector3Int(x, y, z);
    }

    public string WhereAmIString(Vector3 position)
    {
        Vector3Int pos = WhereAmIVectors(position);
        return GetStringFromCoordinates(pos);
    }

    string GetStringFromCoordinates(Vector3Int position)
    {
        //1,2,3,4,5
        //A,B,C,D,E
        //I, II, III, IV, V
        StringBuilder builder = new();
        builder.Append(ConvertCoordinateToString(position.x, "1", "2", "3", "4", "5"));
        builder.Append(", ");
        builder.Append(ConvertCoordinateToString(position.y, "A", "B", "C", "D", "E"));
        builder.Append(", ");
        builder.Append(ConvertCoordinateToString(position.z, "I", "II", "III", "IV", "V"));
        return builder.ToString();

    }
    string ConvertCoordinateToString(int coordinate, params string[] strings)
    {
        return strings[coordinate + WorldModel.HalfWidth];
    }

    public Dictionary<Collider, GridObjectInstance> instanceColliders = new();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Instantiate(WorldModel worldModel)
    {
        this.worldModel = worldModel;

        worldModel.Instantiate(this);
    }
}

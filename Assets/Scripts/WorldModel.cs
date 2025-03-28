using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
using static UnityEngine.UI.GridLayoutGroup;
using System.Linq;

public class WorldModel 
{
    public const int SIZE_OF_GRID_CUBE = 200;
    public const int WORLD_WIDTH = 5;
    public static int HalfWidth => (WORLD_WIDTH - 1) / 2;

    Dictionary<int,Dictionary<int, Dictionary<int, GridObject>>> worldModel;

    List<Vector3> warpPositions = new();
    public IReadOnlyList<Vector3> WarpPositions => warpPositions;

    public GridObject GetAtPosition(int x, int y, int z)
    {
        if (x < -HalfWidth || y < -HalfWidth || z < -HalfWidth ||
            x > HalfWidth || y > HalfWidth || z > HalfWidth)
            return null;
        return worldModel[x][y][z];
    }

    public WorldModel(System.Random random)
    {


        worldModel = new();

        for (int i = -HalfWidth; i <= HalfWidth; i++)
        {
            worldModel[i] = new();
            for (int j = -HalfWidth; j <= HalfWidth; j++)
            {
                worldModel[i][j] = new();
            }
        }


        PlaceStationNearCorner(false,false,false);
        PlaceStationNearCorner(false, false, true);
        PlaceStationNearCorner(false, true, false);
        PlaceStationNearCorner(false, true, true);
        PlaceStationNearCorner(true, false, false);
        PlaceStationNearCorner(true, false, true);
        PlaceStationNearCorner(true, true, false);
        PlaceStationNearCorner(true, true, true);


        void PlaceStationNearCorner(bool _xCorner, bool _yCorner, bool _zCorner)
        {
            int x = ConvertBoolToCorner(_xCorner);
            int y = ConvertBoolToCorner(_yCorner);
            int z = ConvertBoolToCorner(_zCorner);
            worldModel[x][y][z] = new StationGridObject(random);
        }

        int ConvertBoolToCorner(bool isPositive)
        {
            int corner = isPositive ? HalfWidth : -HalfWidth;
            return corner + (int)(random.Next(10) > 7 ? -Mathf.Sign(corner) : 0);
        }


        int numberOfAdditionalStations = random.Next(1, 3);

        List<GridObject> remainingObjects = new();
        for (int i = 0; i < numberOfAdditionalStations; i++)
        {
            remainingObjects.Add(new StationGridObject(random));
        }
        for (int i =0; i < Mathf.Pow(WORLD_WIDTH, 3); i++)
        {
            remainingObjects.Add(new PlanetGridObject(random));
        }



        for (int i = -HalfWidth; i <= HalfWidth; i++)
        {
            for (int j = -HalfWidth; j <= HalfWidth; j++)
            {
                for (int k = -HalfWidth; k <= HalfWidth; k++)
                {
                    if (!worldModel[i][j].ContainsKey(k))
                    {
                        worldModel[i][j][k] = remainingObjects.PopRandom(random);
                    }

                }
            }
        }

        for (int i = 0; i < 100; i++)
        {
            int randomX = random.Next(-HalfWidth, HalfWidth + 1);
            int randomY = random.Next(-HalfWidth, HalfWidth + 1);
            int randomZ = random.Next(-HalfWidth, HalfWidth + 1);
            warpPositions.Add(new Vector3(randomX, randomY, randomZ) * SIZE_OF_GRID_CUBE);
        }
    } 


    public void Instantiate(WorldModelInstance instance)
    {
        for (int i = -HalfWidth; i <= HalfWidth; i++)
        {
            for (int j = -HalfWidth; j <= HalfWidth; j++)
            {
                for (int k = -HalfWidth; k <= HalfWidth; k++)
                {
                    worldModel[i][j][k].Instantiate(new Vector3Int(i, j, k), instance);
                }
            }
        }
    }
}

public abstract class GridObject
{
    public Vector3 SubObjectOffset { get; private set; }
    public Quaternion BaseRotation { get; private set; }
    public Quaternion SubObjectRotation { get; private set; }

    public int DecorationIndex { get; private set; }
    const int NUMBER_OF_DECORATIONS = 3;

    public Quaternion DecorationRotation { get; private set; }
    public Vector3 DecorationOffset { get; private set; }
    public Color DecorationColor { get; private set; }

    public float DecorationSizeMult { get; private set; }

    public abstract string PrefabResource { get; }

    const int MAX_OFFSET = 20;

    public GridObject(System.Random random)
    {
        SubObjectRotation = Quaternion.Euler(random.Next(360), random.Next(360), random.Next(360));
        BaseRotation = Quaternion.Euler(random.Next(3) * 90, random.Next(3) * 90, random.Next(3) * 90);
        SubObjectOffset = new Vector3(random.Next(-MAX_OFFSET, MAX_OFFSET), random.Next(-MAX_OFFSET, MAX_OFFSET), random.Next(-MAX_OFFSET, MAX_OFFSET));
        InternalGenerate(random);

        DecorationRotation = Quaternion.Euler(random.Next(3) * 90, random.Next(3) * 90, random.Next(3) * 90);
        DecorationOffset = new Vector3(random.Next(-MAX_OFFSET, MAX_OFFSET), random.Next(-MAX_OFFSET, MAX_OFFSET), random.Next(-MAX_OFFSET, MAX_OFFSET));
        DecorationIndex = random.Next(NUMBER_OF_DECORATIONS);
        DecorationColor = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        DecorationSizeMult = 2.5f + (float)random.NextDouble() * 5f;
    }

    protected abstract void InternalGenerate(System.Random random);

    public void Instantiate(Vector3Int gridPoint, WorldModelInstance worldModelInstance)
    {
        var obj = Resources.Load(PrefabResource) as GameObject;
        var prefabInstance = obj.GetComponent<GridObjectInstance>();
        var instance = GameObject.Instantiate(prefabInstance,  new Vector3( gridPoint.x, gridPoint.y, gridPoint.z ) * WorldModel.SIZE_OF_GRID_CUBE, BaseRotation);

        instance.SubObject.transform.position += SubObjectOffset;
        instance.SubObject.transform.rotation = SubObjectRotation;
        instance.gridObject = this;
        InternalInstantiate(gridPoint, instance, worldModelInstance);
        worldModelInstance.AddInstance(gridPoint.x, gridPoint.y, gridPoint.z, instance, instance.Collider);
        instance.Instantiate(this);
    }

    protected abstract void InternalInstantiate(Vector3Int gridPoint, GridObjectInstance gridObjectInstance, WorldModelInstance worldModelInstance);
}

public class PlanetGridObject : GridObject
{
    public override string PrefabResource => "PlanetGrid";

    public Color color1;
    public Color color2;
    public bool hasRing;
    public float size;

    public PlanetGridObject(System.Random random) : base(random)
    {
    }

    protected override void InternalGenerate(System.Random random)
    {
        color1 = new Color((float) random.NextDouble(), (float) random.NextDouble(), (float) random.NextDouble());
        color2 = color1 + new Color((float)random.NextDouble() / 5f, (float)random.NextDouble() / 5, (float)random.NextDouble() / 5);
        hasRing = random.Next(0,2) == 1;
        size = 1 + (float)(random.NextDouble()) / 3f;
    }

    protected override void InternalInstantiate(Vector3Int gridPoint, GridObjectInstance gridObjectInstance, WorldModelInstance worldModelInstance)
    {
        
    }
}

public class StationGridObject : GridObject
{
    public override string PrefabResource => "StationGrid";

    public StationGridObject(System.Random random) : base(random)
    {
    }

    protected override void InternalGenerate(System.Random random)
    {

    }

    protected override void InternalInstantiate(Vector3Int gridPoint, GridObjectInstance gridObjectInstance, WorldModelInstance worldModelInstance)
    {
        worldModelInstance.stations.Add(gridObjectInstance.GetComponentInChildren<Station>());
    }
}
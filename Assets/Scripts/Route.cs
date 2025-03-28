using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;
using Unity.VisualScripting;

[Serializable]
public class Route 
{
    public int startingStation;
    public int endingStation;
    public float speed;

    public TimeSpan StartingTime;
    public Train train;

    readonly List<Vector3> offsets = new();


    public Route(System.Random random)
    {
        train = new(random);
        for (int i =0; i < 5; i++)
        {
            offsets.Add(new Vector3(random.Next(-50,50), random.Next(-50,50), random.Next(-50,50)));
        }
        
        speed = 10f;
    }

    public Vector3 GetNextOffset()
    {
        Vector3 ret = offsets[0];
        offsets.RemoveAt(0);
        offsets.Add(ret);
        return ret;
    }

    RouteInstance myRouteInstance;
    public void Instantiate()
    {
        //RouteInstance
        myRouteInstance = ObjectPool.SpawnObject<RouteInstance>("RouteInstance");
        myRouteInstance.AssignRoute(this);
        myRouteInstance.CreateSpline();
    }

    public void Register()
    {
        myRouteInstance.Register();
    }

    public void Run()
    {
        myRouteInstance.Generate();
    }

    public string GetInfo()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine("<b>Shipping Route</b>");
        stringBuilder.AppendLine("Starting: " + WorldModelInstance.Instance.WhereAmIString( WorldModelInstance.Instance.GetStation(startingStation).transform.position));
        stringBuilder.AppendLine("Ending: " + WorldModelInstance.Instance.WhereAmIString(WorldModelInstance.Instance.GetStation(endingStation).transform.position));
        if (GameController.Instance.TimeInGame > StartingTime && myRouteInstance.TrainInstance != null && myRouteInstance.TrainInstance.HasAnyParts())
        {
            stringBuilder.AppendLine("In transit at: " + WorldModelInstance.Instance.WhereAmIString(myRouteInstance.TrainInstance.PartInstances[0].transform.position));
        }
        else
        {
            stringBuilder.AppendLine("Leaving in... " + String.Format(@"{0:m\:ss}", GameController.Instance.TimeInGame - StartingTime));
        }

        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Train:");
        stringBuilder.AppendLine(train.Stringify());

        return stringBuilder.ToString();
    }

}

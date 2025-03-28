using System.Collections.Generic;
using System;
using System.Linq;

public class GamePlan 
{
    readonly List<Route> routes = new();
    readonly List<Route> availableRoutes = new();


    public GamePlan(Random random, int numberOfStations)
    {
        List<int> stationList = new();
        TimeSpan currentTimeSpan = TimeSpan.Zero;
        while (currentTimeSpan.TotalMinutes < 5)
        {
            stationList.Clear();
            stationList.AddRange(Enumerable.Range(0, numberOfStations));

            currentTimeSpan = currentTimeSpan.Add(TimeSpan.FromSeconds(random.Next(10,30)));
            Route route1 = new(random)
            {
                StartingTime = currentTimeSpan,
                startingStation = stationList.PopRandom(random),
                endingStation = stationList.PopRandom(random)
            };
            routes.Add(route1);

            currentTimeSpan = currentTimeSpan.Add(TimeSpan.FromSeconds(random.Next(3, 10)));

            Route route2 = new(random)
            {
                StartingTime = currentTimeSpan,
                startingStation = stationList.PopRandom(random),
                endingStation = stationList.PopRandom(random)
            };
            routes.Add(route2);
            currentTimeSpan = currentTimeSpan.Add(TimeSpan.FromSeconds(random.Next(10, 25)));
        }
        availableRoutes.Clear();
        availableRoutes.AddRange(routes);
        CreateRouteInstances();
    }
    void CreateRouteInstances()
    {
        foreach (var route in routes)
        {
            route.Instantiate();
        }
    }
    public void StartGame()
    {
        foreach (var route in routes)
        {
            route.Register();
        }
    }


    public void Update(TimeSpan currentTimeSpan)
    {
        for (int i = availableRoutes.Count - 1; i >= 0; i--)
        {
            Route route = availableRoutes[i];
            if (currentTimeSpan > route.StartingTime)
            {
                route.Run();
                availableRoutes.Remove(route);
            }
        }
    }

}

public static class Extensions
{
    public static T PopRandom<T>(this List<T> list, Random random)
    {
        int index = random.Next(list.Count);
        T t = list[index];
        list.RemoveAt(index);
        return t;
    }
} 
using UnityEngine;

public interface IClickInfo
{
    public string InfoToShow { get; }
    public Transform transform { get; }

    public RouteInstance RouteInstance { get; }
}

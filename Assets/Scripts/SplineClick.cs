using UnityEngine;
using UnityEngine.EventSystems;

public class SplineClick : MonoBehaviour, IClickInfo
{
    RouteInstance routeInstance;
    public RouteInstance RouteInstance => routeInstance;
    public Renderer MyRenderer;



    private void Awake()
    {
        routeInstance = GetComponentInParent<RouteInstance>();
    }

    public string InfoToShow { get {
            return routeInstance.GetInfo();
        } }

}

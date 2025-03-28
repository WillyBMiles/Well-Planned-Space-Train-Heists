using UnityEngine;

public class SimpleTrainInstance : MonoBehaviour, IClickInfo
{
    TrainPartInstance trainPartInstance;
    public RouteInstance RouteInstance => trainPartInstance.RouteInstance;

    private void OnEnable()
    {
        trainPartInstance = GetComponentInParent<TrainPartInstance>();
    }

    public string InfoToShow => trainPartInstance.RouteInstance.GetInfo();
}

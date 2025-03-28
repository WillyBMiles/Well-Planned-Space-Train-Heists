using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class RouteInstance : MonoBehaviour, IPoolable
{
    public Route route;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] SplineInstantiate splineInstantiate;
    [SerializeField] SplineInstantiate splineInstantiate2;
    public SplineContainer SplineContainer => splineContainer;

    [SerializeField]
    TrainInstance trainInstance;
    public TrainInstance TrainInstance => trainInstance;

    public bool IsTrainAlive()
    {
        return trainInstance.HasAnyParts();
    }

    private void Awake()
    {

    }

    public void AssignRoute(Route route)
    {
        this.route = route;
        trainInstance.train = route.train;
    }
    public void CreateSpline()
    {
        Station startingStation = GameController.Instance.WorldModelInstance.GetStation(route.startingStation);
        Station endingStation = GameController.Instance.WorldModelInstance.GetStation(route.endingStation);

        Transform startingExit = BestExit(startingStation, endingStation);
        Transform endingExit = BestExit(endingStation, startingStation);
        Spline spline = splineContainer.Spline;

        AddKnot(spline, startingExit.position);
        AddKnot(spline, startingExit.position + startingExit.transform.forward * startingStation.exitDistance / 5);
        AddKnot(spline, startingExit.position + 2f * startingStation.exitDistance * startingExit.transform.forward / 5);
        AddKnot(spline, startingExit.position + 3f * startingStation.exitDistance * startingExit.transform.forward / 5);
        AddKnot(spline, startingExit.position + 4f * startingStation.exitDistance * startingExit.transform.forward / 5);
        AddKnot(spline, startingExit.position + startingExit.transform.forward * startingStation.exitDistance);

        Vector3 startPosition = startingExit.transform.position;
        Vector3 endPosition = endingExit.transform.position;
        var hits = Physics.RaycastAll(startPosition, endPosition - startPosition, Vector3.Distance(startPosition, endPosition), LayerMask.GetMask("GridCube"));
        foreach (var hit in hits.Take(hits.Length - 1))
        {
            if (!WorldModelInstance.Instance.instanceColliders.ContainsKey(hit.collider))
                continue;
            GridObjectInstance gridObject = WorldModelInstance.Instance.instanceColliders[hit.collider];
            AddKnot(spline, gridObject.PassThrough.position + route.GetNextOffset()); //Add a knot to the cennter of that cube
        }

        //AddKnot(spline, Vector3.Lerp(startingStation.transform.position, endingStation.transform.position, .5f));

        AddKnot(spline, endingExit.position + endingExit.transform.forward * endingStation.exitDistance);
        AddKnot(spline, endingExit.position + 4f * startingStation.exitDistance * endingExit.transform.forward / 5);
        AddKnot(spline, endingExit.position + 3f * startingStation.exitDistance * endingExit.transform.forward / 5);
        AddKnot(spline, endingExit.position + 2f * startingStation.exitDistance * endingExit.transform.forward / 5);
        AddKnot(spline, endingExit.position + endingExit.transform.forward * endingStation.exitDistance / 5);


        AddKnot(spline, endingExit.position);

        if (PilotController.Instance == null)
        {
            splineInstantiate.enabled = true;
            splineInstantiate2.enabled = true;
            splineInstantiate.UpdateInstances();
            splineInstantiate2.UpdateInstances();
        }


        Debug.Log("Spline set!");

    }

    bool isRegistered = false;
    public void Register()
    {
        if (!RouteShowController.ShouldRegister())
            return;

        if (PilotController.Instance == null)
        {
            splineInstantiate.enabled = true;
            splineInstantiate2.enabled = true;
            splineInstantiate.UpdateInstances();
            splineInstantiate2.UpdateInstances();
        }

        isRegistered = true;
        RouteShowController.Register(this, GetComponentsInChildren<SplineClick>());
    }


    public void Generate()
    {
        trainInstance.StartGeneratingTrain();
    }

    

    Transform BestExit(Station station, Station targetStation)
    {
        Vector3 preferredDirection = station.transform.position - targetStation.transform.position;
        return station.exits.OrderBy(exit =>
            Vector3.Dot(exit.forward, preferredDirection)).First();
    }

    void AddKnot(Spline spline, Vector3 position)
    {
        BezierKnot knot = new(position);
        spline.Add(knot, TangentMode.AutoSmooth);
    }

    public void ResetPoolable()
    {
        //pass just here for convinience
    }

    public string GetInfo()
    {
        return route.GetInfo();
    }
}

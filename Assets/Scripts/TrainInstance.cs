using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TrainInstance : MonoBehaviour
{
    [SerializeField]
    string splineFollowerPrefabResource;
    [SerializeField]
    RouteInstance routeInstance;

    [SerializeField]
    float spacing;
    
    public Train train;


    public void StartGeneratingTrain()
    {
        StartCoroutine(SpawnTrainCoroutine());
    }

    public bool HasAnyParts()
    {
        return partInstances.Count > 0;
    }

    readonly List<TrainPartInstance> partInstances = new();

    public void RemovePart(TrainPartInstance trainPartInstance)
    {
        partInstances.Remove(trainPartInstance);
    }

    public IReadOnlyList<TrainPartInstance> PartInstances => partInstances;
    IEnumerator SpawnTrainCoroutine()
    {
        //Release escort
        //
        if (PilotController.Instance == null)
        {
            var trainPart = train.trainParts[0];
            

            var partInstance = trainPart.Spawn();
            partInstances.Add(partInstance);
            float speed = routeInstance.route.speed;
            var splineFollower = ObjectPool.SpawnObject<SplineFollower>(splineFollowerPrefabResource);
            splineFollower.SetSpline(routeInstance.SplineContainer, speed);
            partInstance.SetStart(splineFollower, routeInstance);
            yield return new WaitForSeconds(trainPart.Distance / speed);
            var splineFollowerEnd = ObjectPool.SpawnObject<SplineFollower>(splineFollowerPrefabResource);
            splineFollowerEnd.SetSpline(routeInstance.SplineContainer, speed);
            partInstance.end = splineFollowerEnd;

           
            yield break;

        }
        foreach (var trainPart in train.trainParts)
        {
            var partInstance = trainPart.Spawn();
            partInstances.Add(partInstance);
            float speed = routeInstance.route.speed;
            var splineFollower = ObjectPool.SpawnObject<SplineFollower>(splineFollowerPrefabResource);
            splineFollower.SetSpline(routeInstance.SplineContainer, speed);
            partInstance.SetStart(splineFollower, routeInstance);
            yield return new WaitForSeconds(trainPart.Distance / speed);
            var splineFollowerEnd = ObjectPool.SpawnObject<SplineFollower>(splineFollowerPrefabResource);
            splineFollowerEnd.SetSpline(routeInstance.SplineContainer, speed);
            partInstance.end = splineFollowerEnd;
            yield return new WaitForSeconds(spacing / speed);
        }
        //INSTANTIATE A POOPER

    }

    public void ResetPoolable()
    {
        //Pass
    }
}

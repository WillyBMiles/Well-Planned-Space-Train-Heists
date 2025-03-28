using UnityEngine;

public class TrainPartInstance : MonoBehaviour, IPoolable
{
    public SplineFollower start;
    public SplineFollower end;
    [SerializeField]
    Transform endTransform;
    [SerializeField]
    Enemy enemy;
    [SerializeField]
    MeshRenderer simpleRenderer;
    [SerializeField]
    MeshRenderer complexRenderer;
    [SerializeField]
    float simpleAngularSize;
    [SerializeField]
    float simpleMinSize;

    
    RouteInstance routeInstance;
    public RouteInstance RouteInstance => routeInstance;

    public Enemy Enemy => enemy;

    private void Start()
    {
        if (PilotController.Instance == null && simpleRenderer != null)
        {
            simpleRenderer.enabled = true;
            complexRenderer.enabled = false;
        }
    }

    public void ResetPoolable()
    {
        enemy.ResetPoolable();
        start = null;
        end = null;
    }

    public void SetStart(SplineFollower start, RouteInstance routeInstance)
    {
        this.start = start;
        transform.position = start.transform.position;
        transform.forward = start.transform.forward;
        this.routeInstance = routeInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (start == null || !start.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            routeInstance.TrainInstance.RemovePart(this);
        }

        if (simpleRenderer!= null && simpleRenderer.enabled)
        {
            RouteShowController.SetSize(simpleRenderer.transform, simpleMinSize, simpleAngularSize);
        }

        transform.position = start.transform.position;
        if (start != null && end != null)
            transform.forward = end.transform.position - start.transform.position;
    }
    void LateUpdate()
    {
        if (start == null || !start.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            routeInstance.TrainInstance.RemovePart(this);
        }
    }
}

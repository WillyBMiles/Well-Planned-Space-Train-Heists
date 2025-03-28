using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class RouteShowController : MonoBehaviour
{
    static RouteShowController instance;

    Navigator navigator;
    private void Awake()
    {
        instance = this;
        LobbyController.isPilot = false;
        navigator = FindFirstObjectByType<Navigator>();
    }

    public static bool ShouldRegister()
    {
        return instance != null;
    }
    readonly Dictionary<RouteInstance, SplinesHolder> allSplineClicks = new();

    class SplinesHolder
    {
        public SplineClick[] array;
        public Material material;
    }

    public static void Register(RouteInstance routeInstance, SplineClick[] splineClicks)
    {
        if (instance == null)
        {
            return;
        }
        Debug.Log(string.Format("{0} registered {1} splineClicks", routeInstance.name, splineClicks.Length));
        instance.LocalRegister(routeInstance, splineClicks);
    }

    void LocalRegister(RouteInstance routeInstance, SplineClick[] splineClicks)
    {
        allSplineClicks[routeInstance] = new SplinesHolder
        {
            array = splineClicks,
            material = new(splineClicks[0].MyRenderer.material)
        };
        foreach (var splineClick in splineClicks)
        {
            splineClick.MyRenderer.material = allSplineClicks[routeInstance].material;
        }
        ToggleSplineShow(routeInstance, false);
    }

    public static void IGotClicked(SplineClick splineClick)
    {
        if (instance == null)
            return;
        instance.InternalIGotClicked(splineClick);
        Debug.Log(string.Format("{0} just got clicked", splineClick.name));
    }

    void InternalIGotClicked(SplineClick splineClick)
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    

    [SerializeField]
    float splineAngularWidth;
    [SerializeField]
    float minSize = 6;


    List<RouteInstance> activeRoutes = new();
    // Update is called once per frame
    void Update()
    {
        TimeSpan time = GameController.Instance.TimeInGame;
        foreach (RouteInstance routeInstance in allSplineClicks.Keys.ToArray())
        {
            
            if (!routeInstance.IsTrainAlive() && routeInstance.route.StartingTime < time - TimeSpan.FromSeconds(5))
            {
                ToggleSplineShow(routeInstance, false);
                activeRoutes.Remove(routeInstance);
                allSplineClicks.Remove(routeInstance);
            }
            else if (!activeRoutes.Contains(routeInstance) && (routeInstance.IsTrainAlive() || time > routeInstance.route.StartingTime - TimeSpan.FromSeconds(60)))
            {
                activeRoutes.Add(routeInstance);
                ToggleSplineShow(routeInstance, true);
                ChangeColor(routeInstance, Color.yellow);
            }
        }
    }

    private void LateUpdate()
    {
        TimeSpan time = GameController.Instance.TimeInGame;
        foreach (var route in activeRoutes)
        {
            float diff = (route.route.StartingTime - time).Seconds / 60f;
            if (navigator.CurrentClickInfo != null && route == navigator.CurrentClickInfo.RouteInstance)
            {
                ChangeColor(route, Color.blue);
            }
            
            else if (diff <= 0)
            {
                ChangeColor(route, Color.red);
            }
            else if (diff > .3f)
            {
                ChangeColor(route, Color.Lerp(Color.grey, Color.yellow, diff / .7f));
            }
            else
            {
                ChangeColor(route, Color.Lerp(new Color(1, 56f / 256, 0f), Color.yellow, diff / .3f));
            }
            foreach (var splineClick in allSplineClicks[route].array)
            {
                SetSize(splineClick.transform, minSize, splineAngularWidth);
            }
        }
    }

    public static void SetSize(Transform obj, float minSize, float angularWidth)
    {
        float distance = Vector3.Distance(Camera.main.transform.position, obj.position);
        if (distance < 1)
            distance = 1;
        float scale = 2 * Mathf.Tan(angularWidth / 2) * distance;
        if (scale < minSize)
        {
            scale = minSize;
        }
        obj.localScale = new Vector3(scale, scale, scale);
    }

    void ToggleSplineShow(RouteInstance routeInstance, bool toggleValue)
    {
        foreach (var click in allSplineClicks[routeInstance].array)
        {
            click.gameObject.SetActive(toggleValue);
        }
    }

    void ChangeColor(RouteInstance routeInstance, Color color)
    {
        allSplineClicks[routeInstance].material.SetColor("_BaseColor", color);
    }
}

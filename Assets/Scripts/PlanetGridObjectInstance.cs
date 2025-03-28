using UnityEngine;

public class PlanetGridObjectInstance : GridObjectInstance
{
    [SerializeField]
    Renderer planetRenderer;
    [SerializeField]
    Renderer ring;

    [SerializeField]
    Renderer navigatorPlanet;
    [SerializeField]
    Renderer navigatorRing;

    public override void Instantiate(GridObject gridObject)
    {
        base.Instantiate(gridObject);
        if (gridObject is PlanetGridObject pgo)
        {
            SubObject.transform.localScale = new Vector3(SubObject.transform.localScale.x * pgo.size,
                SubObject.transform.localScale.y * pgo.size, SubObject.transform.localScale.z * pgo.size);
            if (PilotController.Instance != null)
            {
                //PILOT
                navigatorPlanet.enabled = false;
                navigatorRing.enabled = false;

                planetRenderer.enabled = true;

                planetRenderer.material.SetColor("_Color", pgo.color1);
                planetRenderer.material.SetColor("_Color", pgo.color2);
                if (pgo.hasRing)
                    ring.material.SetColor("_Color", pgo.color2);
                ring.enabled = pgo.hasRing;
            }
            else
            {
                //NAVIGATOR
                navigatorPlanet.enabled = true;

                planetRenderer.enabled = false;
                ring.enabled = false;

                navigatorPlanet.material.SetColor("_BaseColor", pgo.color1);
                if (pgo.hasRing)
                    navigatorRing.material.SetColor("_BaseColor", pgo.color2);
                navigatorRing.enabled = pgo.hasRing;
            }
        }
    }
}

using UnityEngine;

public class WarpIndicator : MonoBehaviour, IClickInfo
{
    public string InfoToShow => "The pilot can warp here!\nThis position changes every 15 seconds.\n<i>Warping when position is about to change can lead to unusual results!</i>\n" +
        "\nWarp Position changing in...\n  " + (int) GameController.Instance.TimeTilWarp.TotalSeconds + " seconds"
        ;

    public RouteInstance RouteInstance => null;
    [SerializeField]
    public Renderer myRenderer;

    [SerializeField]
    float minSize;
    [SerializeField]
    float angularSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myRenderer.material.SetColor("_BaseColor", Color.Lerp(Color.red, Color.white, (float)GameController.Instance.TimeTilWarp.TotalSeconds / 15f));
        RouteShowController.SetSize(this.transform, minSize, angularSize);
        transform.position = GameController.Instance.WarpPosition;
    }
}

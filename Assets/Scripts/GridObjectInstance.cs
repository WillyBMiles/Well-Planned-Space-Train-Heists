using System.Collections.Generic;
using UnityEngine;

public class GridObjectInstance : MonoBehaviour
{
    [SerializeField]
    GameObject subObject;
    [SerializeField]
    GameObject simpleVersionSubObject;
    [SerializeField]
    GameObject complexVersionSubObject;

    public GameObject SubObject => subObject;

    [SerializeField]
    Transform passThrough;
    public Transform PassThrough => passThrough;

    public GridObject gridObject;

    [SerializeField]
    List<ParticleSystem> decorations = new();

    [SerializeField]
    Collider _collider;
    public Collider Collider => _collider;

    

    public virtual void Instantiate(GridObject gridObject)
    {
        if (PilotController.Instance == null)
        {
            if (simpleVersionSubObject != null)
            {
                simpleVersionSubObject.SetActive(true);
                complexVersionSubObject.SetActive(false);
            }
        }
        else
        {

            if (decorations.Count > gridObject.DecorationIndex)
            {
                var deco = Instantiate(decorations[gridObject.DecorationIndex], transform.position + gridObject.DecorationOffset, gridObject.DecorationRotation);
                deco.transform.localScale *= gridObject.DecorationSizeMult;

                var main = deco.main;
                main.startColor = new ParticleSystem.MinMaxGradient(Color.white, gridObject.DecorationColor) { mode = ParticleSystemGradientMode.TwoColors };
                deco.Play();
            }
        }


            
    }
}

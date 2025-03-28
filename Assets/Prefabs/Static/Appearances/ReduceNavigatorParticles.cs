using UnityEngine;

public class ReduceNavigatorParticles : MonoBehaviour
{
    [SerializeField]
    ParticleSystem system;
    [SerializeField]
    bool justDisable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PilotController.Instance != null)
        {
            //pilot
        }
        else
        {
            if (justDisable)
            {
                gameObject.SetActive(false);
                return;
            }

            var emission = system.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant / 5); //
                
        }
    }
}

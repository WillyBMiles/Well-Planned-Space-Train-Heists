using UnityEngine;

public class CameraShake : MonoBehaviour
{
    static CameraShake instance;

    float shakeAmount = 0f;
    public static void Shake(float amount)
    {
        if (instance != null)
        {
            instance.shakeAmount += amount;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeAmount > .001){
            transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shakeAmount = Mathf.Lerp(shakeAmount, 0f, Time.deltaTime * 5f);
        }
        
    }
}

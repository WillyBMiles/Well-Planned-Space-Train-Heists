using UnityEngine;

public class CameraRotater : MonoBehaviour
{
    [SerializeField]
    Camera cam;


    float fieldOfView;
    private void Start()
    {
        fieldOfView = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (Rotater.Rotated)
        {
            cam.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            cam.fieldOfView = 100f;
        }
        else
        {
            cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            cam.fieldOfView = fieldOfView;
        }
    }
}

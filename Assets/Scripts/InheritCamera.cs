using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InheritCamera : MonoBehaviour
{
    Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cam.fieldOfView = Camera.main.fieldOfView;
    }
}

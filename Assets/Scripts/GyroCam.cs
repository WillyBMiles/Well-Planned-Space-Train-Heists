using SK.GyroscopeWebGL;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GyroCam : MonoBehaviour
{
    public static Quaternion? startGyroRotation = null;
    Quaternion startRotation;

    [SerializeField]
    bool useRightClick = false;

#if UNITY_EDITOR
    [SerializeField]
    bool usePCInEditor;

    private void Awake()
    {

        if (usePCInEditor)
        {
            MenuScene.OnPc = true;
            StartGyroListening();
        }

    }
#endif
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startRotation = transform.rotation;

        if (Rotater.Rotated)
        {
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Camera.main.fieldOfView = 100f;
        }

        if (startGyroRotation != null)
        {
            SK_DeviceSensor.StartGyroscopeListener(OnGyroscopeReading);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (startGyroRotation != null && currentGyroRotation != null && !MenuScene.OnPc)
        {
            transform.rotation = startRotation * Quaternion.Inverse(startGyroRotation.Value) * currentGyroRotation.Value;
        }

        if (startGyroRotation != null && MenuScene.OnPc && (!useRightClick || Input.GetMouseButton(1)))
        {
            transform.rotation = transform.rotation * Quaternion.Euler(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }
    }

    public void StartGyroListening()
    {
        if (MenuScene.OnPc)
        {
            startGyroRotation = new Quaternion();
        }
        else
        {
            startGyroRotation = null;
            SK_DeviceSensor.StartGyroscopeListener(OnGyroscopeReading);
        }
    }

    void OnDestroy()
    {
        SK_DeviceSensor.StopGyroscopeListener();
    }

    
    Quaternion? currentGyroRotation = null;
    private void OnGyroscopeReading(GyroscopeData reading)
    {
        Quaternion unityRotation = reading.UnityRotation * Quaternion.Euler(90, 0, -90);

        if (startGyroRotation == null)
        {
            startGyroRotation = unityRotation;
        }
        currentGyroRotation = unityRotation;
    }



}

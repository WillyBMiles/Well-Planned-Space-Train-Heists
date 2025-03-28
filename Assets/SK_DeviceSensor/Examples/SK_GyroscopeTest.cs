using UnityEngine;
using UnityEngine.UI;
using SK.GyroscopeWebGL;

namespace SK.GyroscopeWebGL.Examples
{
    public class SK_GyroscopeTest : MonoBehaviour
    {
        public Text Label;
        public Transform Model;
        public Button Button;

        void Awake()
        {
            Button.onClick.AddListener(ToggleGyroscope);
        }

        private void Start()
        {
            Button.GetComponentInChildren<Text>().text = SK_DeviceSensor.IsGyroscopeStarted ? "Gyro Stop" : "Gyro Start";
        }

        void OnDestroy()
        {
            SK_DeviceSensor.StopGyroscopeListener();
        }

        private void OnGyroscopeReading(GyroscopeData reading)
        {
            Label.text = $"alpha: {reading.Alpha}, beta: {reading.Beta},gamma: {reading.Gamma} absolute: {reading.Absolute} ,unityRotation: {reading.UnityRotation}";
            Model.rotation = reading.UnityRotation;
        }

        private void ToggleGyroscope()
        {
            if (SK_DeviceSensor.IsGyroscopeStarted)
            {
                SK_DeviceSensor.StopGyroscopeListener();
            }
            else
            {
                SK_DeviceSensor.StartGyroscopeListener(OnGyroscopeReading);
            }

            Button.GetComponentInChildren<Text>().text = SK_DeviceSensor.IsGyroscopeStarted ? "Gyro Stop" : "Gyro Start";
        }
    }
}
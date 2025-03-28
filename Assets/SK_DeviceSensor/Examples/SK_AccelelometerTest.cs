using UnityEngine;
using UnityEngine.UI;

namespace SK.GyroscopeWebGL.Examples
{
    public class SK_AccelelometerTest : MonoBehaviour
    {
        public Text Label;
        public Button Button;
        public Transform Model;

        private Vector3 _defaultPos;
        private Vector3 _lastAcceleration;

        void Awake()
        {
            _defaultPos = Model.position;
            Button.onClick.AddListener(ToggleAccelelometer);
        }

        private void Start()
        {
            Button.GetComponentInChildren<Text>().text = SK_DeviceSensor.IsAccelerometerStarted ? "Accelelometer Stop" : "Accelelometer Start";
        }

        void OnDestroy()
        {
            SK_DeviceSensor.StopAccelelometerListener();
        }

        private void OnReading(AccelerometerData reading)
        {
            // update label with every property in new line
            Label.text = $"AccelerationX: {reading.AccelerationX}\n" +
                         $"AccelerationY: {reading.AccelerationY}\n" +
                         $"AccelerationZ: {reading.AccelerationZ}\n" +
                         $"AccelerationIncludingGravityX: {reading.AccelerationIncludingGravityX}\n" +
                         $"AccelerationIncludingGravityY: {reading.AccelerationIncludingGravityY}\n" +
                         $"AccelerationIncludingGravityZ: {reading.AccelerationIncludingGravityZ}\n" +
                         $"RotationAlpha: {reading.RotationAlpha}\n" +
                         $"RotationBeta: {reading.RotationBeta}\n" +
                         $"RotationGamma: {reading.RotationGamma}\n" +
                         $"Interval: {reading.Interval}\n" +
                         $"UnityRotation: {reading.UnityRotation}";

            var acc = SK_DeviceSensor.SensorVectorToUnityVector(reading.AccelerationX, reading.AccelerationY, reading.AccelerationZ);
            var diff = _lastAcceleration - acc;
            if (Mathf.Abs(diff.x) > 0.15f && Mathf.Abs(diff.y) > 0.15f && Mathf.Abs(diff.z) > 0.15f)
            {
                Model.position = _defaultPos + acc;
            }
            _lastAcceleration = acc;
        }

        private void ToggleAccelelometer()
        {
            if (SK_DeviceSensor.IsAccelerometerStarted)
            {
                SK_DeviceSensor.StopAccelelometerListener();
            }
            else
            {
                SK_DeviceSensor.StartAccelerometerListener(OnReading);
            }

            Button.GetComponentInChildren<Text>().text = SK_DeviceSensor.IsAccelerometerStarted ? "Accelelometer Stop" : "Accelelometer Start";
        }
    }
}
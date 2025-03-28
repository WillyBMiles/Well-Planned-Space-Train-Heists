using System;
using UnityEngine;
using UnityEngine.Events;

namespace SK.GyroscopeWebGL
{
    public class SK_DeviceSensor : MonoBehaviour
    {
        public static UnityEvent<GyroscopeData> OnGyroscopeReadingEvent = new UnityEvent<GyroscopeData>();
        public static UnityEvent<AccelerometerData> OnAccelerometerReadingEvent = new UnityEvent<AccelerometerData>();

        public static bool IsGyroscopeStarted => _isGyroscopeStarted;
        public static bool IsAccelerometerStarted => _isAccelerometerStarted;

        private static bool _initialized;

        private static bool _isGyroscopeStarted;
        private static bool _isAccelerometerStarted;

        private static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            SK_GyroscopeJsLib.Init("SK_DeviceSensor");
        }

        public static void StartGyroscopeListener(UnityAction<GyroscopeData> onReadingEventHandler)
        {
            if (_isGyroscopeStarted)
            {
                return;
            }

            if (onReadingEventHandler != null)
            {
                OnGyroscopeReadingEvent.RemoveAllListeners();
                OnGyroscopeReadingEvent.AddListener(onReadingEventHandler);
            }

#if UNITY_EDITOR || !UNITY_WEBGL
            Debug.LogWarning("Gyroscope is not supported in this platform.");
#else
            Initialize();

            SK_GyroscopeJsLib.StartGyroscope();
            SK_GyroscopeJsLib.OnGyroscopeReadingEvent.AddListener(OnGyroscopeReading);
#endif
            _isGyroscopeStarted = true;
        }

        public static void StopGyroscopeListener()
        {
            OnGyroscopeReadingEvent.RemoveAllListeners();
            if (!_isGyroscopeStarted)
            {
                return;
            }

#if UNITY_EDITOR || !UNITY_WEBGL
            Debug.LogWarning("Gyroscope is not supported in this platform.");
#else

            SK_GyroscopeJsLib.StopGyroscope();
            SK_GyroscopeJsLib.OnGyroscopeReadingEvent.RemoveListener(OnGyroscopeReading);
#endif
            _isGyroscopeStarted = false;
        }

        public static void StartAccelerometerListener(UnityAction<AccelerometerData> onReadingEventHandler)
        {
            if (_isAccelerometerStarted)
            {
                return;
            }

            if (OnAccelerometerReadingEvent != null)
            {
                OnAccelerometerReadingEvent.RemoveAllListeners();
                OnAccelerometerReadingEvent.AddListener(onReadingEventHandler);
            }

#if UNITY_EDITOR || !UNITY_WEBGL
            Debug.LogWarning("Accelerometer is not supported in this platform.");
#else
            Initialize();

            SK_GyroscopeJsLib.StartAccelerometer();
            SK_GyroscopeJsLib.OnDeviceMotionReadingEvent.AddListener(OnAccelerometerReading);
#endif
            _isAccelerometerStarted = true;
        }

        public static void StopAccelelometerListener()
        {
            OnAccelerometerReadingEvent.RemoveAllListeners();
            if (!_isAccelerometerStarted)
            {
                return;
            }

#if UNITY_EDITOR || !UNITY_WEBGL
            Debug.LogWarning("Accelerometer is not supported in this platform.");
#else

            SK_GyroscopeJsLib.StopAccelerometer();
            SK_GyroscopeJsLib.OnDeviceMotionReadingEvent.RemoveListener(OnAccelerometerReading);
#endif
            _isAccelerometerStarted = false;
        }

        private static void OnGyroscopeReading(string payload, byte[] buffer)
        {
            var reading = JsonUtility.FromJson<GyroscopeReadingEvent>(payload);

            var data = new GyroscopeData
            {
                Alpha = reading.alpha,
                Beta = reading.beta,
                Gamma = reading.gamma,
                Absolute = reading.absolute,
                UnityRotation = SensorRotationToQuaternion(reading)
            };

            if (OnGyroscopeReadingEvent != null)
            {
                OnGyroscopeReadingEvent.Invoke(data);
            }
        }

        private static void OnAccelerometerReading(string payload, byte[] buffer)
        {
            var reading = JsonUtility.FromJson<AccelelometerReadingEvent>(payload);

            var data = new AccelerometerData
            {
                AccelerationX = reading.accelerationX,
                AccelerationY = reading.accelerationY,
                AccelerationZ = reading.accelerationZ,
                AccelerationIncludingGravityX = reading.accelerationIncludingGravityX,
                AccelerationIncludingGravityY = reading.accelerationIncludingGravityY,
                AccelerationIncludingGravityZ = reading.accelerationIncludingGravityZ,
                RotationAlpha = reading.rotationAlpha,
                RotationBeta = reading.rotationBeta,
                RotationGamma = reading.rotationGamma,
                Interval = reading.interval,
                UnityRotation = SensorRotationToQuaternion(reading.rotationAlpha, reading.rotationBeta, reading.rotationGamma)
            };

            if (OnAccelerometerReadingEvent != null)
            {
                OnAccelerometerReadingEvent.Invoke(data);
            }
        }

        public static Quaternion SensorRotationToQuaternion(GyroscopeReadingEvent readingEvent)
        {
            return SensorRotationToQuaternion(readingEvent.alpha, readingEvent.beta, readingEvent.gamma);
        }

        public static Quaternion SensorRotationToQuaternion(double alpha, double beta, double gamma)
        {
            var a = (float)alpha;
            var b = (float)beta;
            var g = (float)gamma;

            return Quaternion.Euler(-b, -a, -g);
        }

        public static Vector3 SensorVectorToUnityVector(double x, double y, double z)
        {
            return new Vector3((float)-x, (float)-y, (float)-z);
        }

        public static bool IsGyroscopeSupported()
        {
#if UNITY_EDITOR || !UNITY_WEBGL
            return false;
#else
            return SK_GyroscopeJsLib.IsGyroscopeSupported();
#endif

        }

        public static bool IsAccelerometerSupported()
        {
#if UNITY_EDITOR || !UNITY_WEBGL
            return false;
#else
            return SK_GyroscopeJsLib.IsAccelelometerSupported();
#endif
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace SK.GyroscopeWebGL.Editor
{
    [CustomEditor(typeof(SK_DeviceSensor))]
    public class SK_DeviceSensorEditor : UnityEditor.Editor
    {
        private GyroscopeData _readingEvent;
        private AccelerometerData _accelerometerReadingEvent;

        private bool _debugGyroscopeEventEnabled;
        private bool _debugAccelerometerEventEnabled;

        void OnEnable()
        {
            _readingEvent = new GyroscopeData
            {
                Alpha = 0,
                Beta = 90,
                Gamma = 0,
                Absolute = false
            };

            _accelerometerReadingEvent = new AccelerometerData
            {
                AccelerationX = 1,
                AccelerationY = 2,
                AccelerationZ = 0,
                AccelerationIncludingGravityX = 0,
                AccelerationIncludingGravityY = 0,
                AccelerationIncludingGravityZ = 0,
                RotationAlpha = 0,
                RotationBeta = 0,
                RotationGamma = 0,
                Interval = 16,
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            DrawGyroscopeDebug();

            EditorGUILayout.Space();
            DrawAccelerometerDebug();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGyroscopeDebug()
        {
            EditorGUILayout.LabelField("Debug Gyroscope", EditorStyles.boldLabel);
            _debugGyroscopeEventEnabled = EditorGUILayout.Toggle("Enabled", _debugGyroscopeEventEnabled);
            var eventDataCopy = new GyroscopeData
            {
                Alpha = _readingEvent.Alpha,
                Beta = _readingEvent.Beta,
                Gamma = _readingEvent.Gamma,
                Absolute = _readingEvent.Absolute,
            };

            EditorGUI.BeginDisabledGroup(!_debugGyroscopeEventEnabled);
            _readingEvent.Alpha = EditorGUILayout.DoubleField("Alpha", _readingEvent.Alpha);
            _readingEvent.Beta = EditorGUILayout.DoubleField("Beta", _readingEvent.Beta);
            _readingEvent.Gamma = EditorGUILayout.DoubleField("Gamma", _readingEvent.Gamma);
            _readingEvent.Absolute = EditorGUILayout.Toggle("Absolute", _readingEvent.Absolute);
            EditorGUI.EndDisabledGroup();

            _readingEvent.UnityRotation = SK_DeviceSensor.SensorRotationToQuaternion(_readingEvent.Alpha, _readingEvent.Beta, _readingEvent.Gamma);

            if (eventDataCopy.Alpha != _readingEvent.Alpha
            || eventDataCopy.Beta != _readingEvent.Beta
            || eventDataCopy.Gamma != _readingEvent.Gamma
            || eventDataCopy.Absolute != _readingEvent.Absolute)
            {
                SK_DeviceSensor.OnGyroscopeReadingEvent.Invoke(_readingEvent);
            }
        }

        private void DrawAccelerometerDebug()
        {
            EditorGUILayout.LabelField("Debug Accelerometer", EditorStyles.boldLabel);
            _debugAccelerometerEventEnabled = EditorGUILayout.Toggle("Enabled", _debugAccelerometerEventEnabled);
            var eventDataCopy = new AccelerometerData
            {
                AccelerationX = _accelerometerReadingEvent.AccelerationX,
                AccelerationY = _accelerometerReadingEvent.AccelerationY,
                AccelerationZ = _accelerometerReadingEvent.AccelerationZ,
                AccelerationIncludingGravityX = _accelerometerReadingEvent.AccelerationIncludingGravityX,
                AccelerationIncludingGravityY = _accelerometerReadingEvent.AccelerationIncludingGravityY,
                AccelerationIncludingGravityZ = _accelerometerReadingEvent.AccelerationIncludingGravityZ,
                RotationAlpha = _accelerometerReadingEvent.RotationAlpha,
                RotationBeta = _accelerometerReadingEvent.RotationBeta,
                RotationGamma = _accelerometerReadingEvent.RotationGamma,
                Interval = _accelerometerReadingEvent.Interval,
                UnityRotation = _accelerometerReadingEvent.UnityRotation
            };

            EditorGUI.BeginDisabledGroup(!_debugAccelerometerEventEnabled);
            _accelerometerReadingEvent.AccelerationX = EditorGUILayout.DoubleField("AccelerationX", _accelerometerReadingEvent.AccelerationX);
            _accelerometerReadingEvent.AccelerationY = EditorGUILayout.DoubleField("AccelerationY", _accelerometerReadingEvent.AccelerationY);
            _accelerometerReadingEvent.AccelerationZ = EditorGUILayout.DoubleField("AccelerationZ", _accelerometerReadingEvent.AccelerationZ);
            _accelerometerReadingEvent.AccelerationIncludingGravityX = EditorGUILayout.DoubleField("AccelerationIncludingGravityX", _accelerometerReadingEvent.AccelerationIncludingGravityX);
            _accelerometerReadingEvent.AccelerationIncludingGravityY = EditorGUILayout.DoubleField("AccelerationIncludingGravityY", _accelerometerReadingEvent.AccelerationIncludingGravityY);
            _accelerometerReadingEvent.AccelerationIncludingGravityZ = EditorGUILayout.DoubleField("AccelerationIncludingGravityZ", _accelerometerReadingEvent.AccelerationIncludingGravityZ);
            _accelerometerReadingEvent.RotationAlpha = EditorGUILayout.DoubleField("RotationAlpha", _accelerometerReadingEvent.RotationAlpha);
            _accelerometerReadingEvent.RotationBeta = EditorGUILayout.DoubleField("RotationBeta", _accelerometerReadingEvent.RotationBeta);
            _accelerometerReadingEvent.RotationGamma = EditorGUILayout.DoubleField("RotationGamma", _accelerometerReadingEvent.RotationGamma);
            _accelerometerReadingEvent.Interval = EditorGUILayout.DoubleField("Interval", _accelerometerReadingEvent.Interval);
            EditorGUI.EndDisabledGroup();

            var eventDataChanged = eventDataCopy.AccelerationX != _accelerometerReadingEvent.AccelerationX
                                   || eventDataCopy.AccelerationY != _accelerometerReadingEvent.AccelerationY
                                   || eventDataCopy.AccelerationZ != _accelerometerReadingEvent.AccelerationZ
                                   || eventDataCopy.AccelerationIncludingGravityX != _accelerometerReadingEvent.AccelerationIncludingGravityX
                                   || eventDataCopy.AccelerationIncludingGravityY != _accelerometerReadingEvent.AccelerationIncludingGravityY
                                   || eventDataCopy.AccelerationIncludingGravityZ != _accelerometerReadingEvent.AccelerationIncludingGravityZ
                                   || eventDataCopy.RotationAlpha != _accelerometerReadingEvent.RotationAlpha
                                   || eventDataCopy.RotationBeta != _accelerometerReadingEvent.RotationBeta
                                   || eventDataCopy.RotationGamma != _accelerometerReadingEvent.RotationGamma
                                   || eventDataCopy.Interval != _accelerometerReadingEvent.Interval;
            if (eventDataChanged)
            {
                SK_DeviceSensor.OnAccelerometerReadingEvent.Invoke(_accelerometerReadingEvent);
            }
        }
    }
}

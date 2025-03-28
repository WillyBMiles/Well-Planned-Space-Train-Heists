using System;
using UnityEngine;

namespace SK.GyroscopeWebGL
{
    [Serializable]
    public class AccelerometerData
    {
        public double AccelerationX;
        public double AccelerationY;
        public double AccelerationZ;
        public double AccelerationIncludingGravityX;
        public double AccelerationIncludingGravityY;
        public double AccelerationIncludingGravityZ;
        public double RotationAlpha;
        public double RotationBeta;
        public double RotationGamma;
        public double Interval;

        public Quaternion UnityRotation;
    }
}
using System;
using UnityEngine;

namespace SK.GyroscopeWebGL
{
    [Serializable]
    public class AccelelometerReadingEvent
    {
        public double accelerationX;
        public double accelerationY;
        public double accelerationZ;
        public double accelerationIncludingGravityX;
        public double accelerationIncludingGravityY;
        public double accelerationIncludingGravityZ;
        public double rotationAlpha;
        public double rotationBeta;
        public double rotationGamma;
        public double interval;
    }
}
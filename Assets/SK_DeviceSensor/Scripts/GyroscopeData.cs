using System;
using UnityEngine;

namespace SK.GyroscopeWebGL
{
    public class GyroscopeData
    {
        public double Alpha;
        public double Beta;
        public double Gamma;
        public bool Absolute;
        public Quaternion UnityRotation;
    }
}
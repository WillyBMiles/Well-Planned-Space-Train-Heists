using System;
using UnityEngine;

namespace SK.GyroscopeWebGL
{
    [Serializable]
    public class GyroscopeReadingEvent
    {
        public double alpha;
        public double beta;
        public double gamma;
        public bool absolute;
    }
}
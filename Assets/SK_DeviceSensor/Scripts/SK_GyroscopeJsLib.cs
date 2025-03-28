namespace SK.GyroscopeWebGL
{
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class SK_GyroscopeJsLib : MonoBehaviour
{
[DllImport("__Internal")]
private static extern int SK_Gyroscope_init(string name, System.Action<byte[], int, byte[], int, byte[], int> onBytes);
[DllImport("__Internal")]
private static extern bool SK_Gyroscope_isGyroscopeSupported();
[DllImport("__Internal")]
private static extern bool SK_Gyroscope_isAccelelometerSupported();
[DllImport("__Internal")]
private static extern void SK_Gyroscope_startGyroscope();
[DllImport("__Internal")]
private static extern void SK_Gyroscope_stopGyroscope();
[DllImport("__Internal")]
private static extern void SK_Gyroscope_startAccelerometer();
[DllImport("__Internal")]
private static extern void SK_Gyroscope_stopAccelerometer();

public static void Init(string name)
{
SK_Gyroscope_init(name, SK_Gyroscope_OnDynamicCall);
}

public static bool IsGyroscopeSupported()
{
return SK_Gyroscope_isGyroscopeSupported();
}

public static bool IsAccelelometerSupported()
{
return SK_Gyroscope_isAccelelometerSupported();
}

public static void StartGyroscope()
{
SK_Gyroscope_startGyroscope();
}

public static void StopGyroscope()
{
SK_Gyroscope_stopGyroscope();
}

public static void StartAccelerometer()
{
SK_Gyroscope_startAccelerometer();
}

public static void StopAccelerometer()
{
SK_Gyroscope_stopAccelerometer();
}


public static UnityEvent<string, byte[]> OnDeviceMotionReadingEvent = new UnityEvent<string, byte[]>();
public static UnityEvent<string, byte[]> OnGyroscopeReadingEvent = new UnityEvent<string, byte[]>();

      [AOT.MonoPInvokeCallback(typeof(System.Action<byte[], int, byte[], int, byte[], int>))]
      public static void SK_Gyroscope_OnDynamicCall(
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 1)] byte[] funcNameBuff, int funcNameLen,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 3)] byte[] payloadBuff, int payloadLen,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 5)] byte[] buffer, int len)
{
var funcName = System.Text.Encoding.UTF8.GetString(funcNameBuff, 0, funcNameLen - 1);
var payload = System.Text.Encoding.UTF8.GetString(payloadBuff, 0, payloadLen - 1);
if(funcName == "OnDeviceMotionReading")
{
if (OnDeviceMotionReadingEvent != null) { OnDeviceMotionReadingEvent.Invoke(payload, buffer); }
return;
}

if(funcName == "OnGyroscopeReading")
{
if (OnGyroscopeReadingEvent != null) { OnGyroscopeReadingEvent.Invoke(payload, buffer); }
return;
}

}


}

}
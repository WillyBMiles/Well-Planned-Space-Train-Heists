
var _skJsLibGyroModule = {
  $callbacks: {
    onDynamicCall: {}
  },

  
      SK_Gyroscope_init: function(gameObjNameStr, onDynamicCall) {
        const gameObjName = UTF8ToString(gameObjNameStr);
        callbacks.onDynamicCall = onDynamicCall;
        window._skJsLibGyro = (function () {
  'use strict';

function UCALL(funcName, arg) {
      if(!window._unityInstance){
        console.log("Unity game instance could not be found. Please modify your index.html template.");
        return;
      }
      
      window._unityInstance.SendMessage(gameObjName, funcName, arg);
    }
      

      function DYNCALL(funcName, payload, data) {
        if (!(payload instanceof String)) {
          payload = JSON.stringify(payload);
        }

        if(!data) {
          data = new Uint8Array();
        }
    
        const payloadBufferSize = lengthBytesUTF8(payload) + 1;
        const payloadBuffer = _malloc(payloadBufferSize);
        stringToUTF8(payload, payloadBuffer, payloadBufferSize);
    
        const funcNameBufferSize = lengthBytesUTF8(funcName) + 1;
        const funcNameBuffer = _malloc(funcNameBufferSize);
        stringToUTF8(funcName, funcNameBuffer, funcNameBufferSize);
    
        const buffer = _malloc(data.length * data.BYTES_PER_ELEMENT);
        HEAPU8.set(data, buffer);
    
        Module.dynCall_viiiiii(
          callbacks.onDynamicCall,
          funcNameBuffer,
          funcNameBufferSize,
          payloadBuffer,
          payloadBufferSize,
          buffer,
          data.length
        );
    
        _free(payloadBuffer);
        _free(funcNameBuffer);
        _free(buffer);
      }
      

  class AccelelometerEvent {
      start() {
          window.addEventListener("devicemotion", this.onDeviceMotion);
      }
      stop() {
          window.removeEventListener("devicemotion", this.onDeviceMotion);
      }
      onDeviceMotion(event) {
          const result = {
              accelerationX: event.acceleration.x,
              accelerationY: event.acceleration.y,
              accelerationZ: event.acceleration.z,
              accelerationIncludingGravityX: event.accelerationIncludingGravity.x,
              accelerationIncludingGravityY: event.accelerationIncludingGravity.y,
              accelerationIncludingGravityZ: event.accelerationIncludingGravity.z,
              rotationAlpha: event.rotationRate.alpha,
              rotationBeta: event.rotationRate.beta,
              rotationGamma: event.rotationRate.gamma,
              interval: event.interval,
          };
          DYNCALL("OnDeviceMotionReading", result, null);
      }
  }

  class Gyroevent {
      start() {
          window.addEventListener("deviceorientation", this.onOrientationEvent);
      }
      stop() {
          window.removeEventListener("deviceorientation", this.onOrientationEvent);
      }
      onOrientationEvent(event) {
          const result = {
              beta: event.beta,
              gamma: event.gamma,
              alpha: event.alpha,
              absolute: event.absolute,
          };
          DYNCALL("OnGyroscopeReading", result, null);
      }
  }

  class UnityHooks {
      static isGyroscopeSupported() {
          return window.DeviceOrientationEvent !== undefined;
      }
      static isAccelelometerSupported() {
          return window.DeviceMotionEvent !== undefined;
      }
      static startGyroscope() {
          if (!DeviceOrientationEvent) {
              console.warn("DeviceOrientationEvent is not supported on this device.");
              return;
          }
          if (typeof DeviceOrientationEvent["requestPermission"] === "function") {
              DeviceMotionEvent["requestPermission"]()
                  .then((permissionState) => {
                  if (permissionState === "granted") {
                      this.gyroscopeEvent = new Gyroevent();
                      this.gyroscopeEvent.start();
                  }
              })
                  .catch(console.error);
              return;
          }
          UnityHooks.gyroscopeEvent = new Gyroevent();
          UnityHooks.gyroscopeEvent.start();
      }
      static stopGyroscope() {
          var _a;
          (_a = UnityHooks.gyroscopeEvent) === null || _a === void 0 ? void 0 : _a.stop();
      }
      static startAccelerometer() {
          if (!DeviceMotionEvent) {
              console.warn("DeviceMotionEvent is not supported on this device.");
              return;
          }
          if (typeof DeviceMotionEvent["requestPermission"] === "function") {
              DeviceMotionEvent["requestPermission"]()
                  .then((permissionState) => {
                  if (permissionState === "granted") {
                      this.accelelometerEvent = new AccelelometerEvent();
                      this.accelelometerEvent.start();
                  }
              })
                  .catch(console.error);
              return;
          }
          UnityHooks.accelelometerEvent = new AccelelometerEvent();
          UnityHooks.accelelometerEvent.start();
      }
      static stopAccelerometer() {
          var _a;
          (_a = UnityHooks.accelelometerEvent) === null || _a === void 0 ? void 0 : _a.stop();
      }
  }

  return UnityHooks;

})();

      },
    SK_Gyroscope_isGyroscopeSupported: function() {
var result = window._skJsLibGyro.isGyroscopeSupported();
var bs = lengthBytesUTF8(result);
var buff = _malloc(bs);
stringToUTF8(result, buff, bs);
return buff;
},
SK_Gyroscope_isAccelelometerSupported: function() {
var result = window._skJsLibGyro.isAccelelometerSupported();
var bs = lengthBytesUTF8(result);
var buff = _malloc(bs);
stringToUTF8(result, buff, bs);
return buff;
},
SK_Gyroscope_startGyroscope: function() {
window._skJsLibGyro.startGyroscope();
},
SK_Gyroscope_stopGyroscope: function() {
window._skJsLibGyro.stopGyroscope();
},
SK_Gyroscope_startAccelerometer: function() {
window._skJsLibGyro.startAccelerometer();
},
SK_Gyroscope_stopAccelerometer: function() {
window._skJsLibGyro.stopAccelerometer();
},

};

autoAddDeps(_skJsLibGyroModule, "$callbacks");
mergeInto(LibraryManager.library, _skJsLibGyroModule);

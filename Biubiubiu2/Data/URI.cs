using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class URI {

        public static string GetLocalUrl() {
            string path = "";
#if BUILD_SERVER
            path = "./Sausage2";
#elif UNITY_IOS
            path = Application.temporaryCachePath;
#else
            path = Application.persistentDataPath;
#endif
            return path;
        }
    }
}
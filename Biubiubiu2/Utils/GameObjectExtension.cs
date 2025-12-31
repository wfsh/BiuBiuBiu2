using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Util {
    public static class GameObjectExtension {
        public static void SetActiveEx(this GameObject gameObject, bool active) {
            if (gameObject == null) {
                throw new NullReferenceException("GameObject is null.");
            }

            if (gameObject.activeSelf != active) {
                gameObject.SetActive(active);
            }
        }
    }
}

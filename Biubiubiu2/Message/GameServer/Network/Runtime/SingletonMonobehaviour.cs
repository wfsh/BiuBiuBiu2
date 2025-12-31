using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T instance = null;

    public static T Instance {
        get {
            if (instance != null) {
                return instance;
            }
#if UNITY_EDITOR
            if (EditorApplication.isPlaying == false) {
                return null;
            }
#endif
            instance = new GameObject(typeof(T).Name).AddComponent<T>();
            if (Application.isPlaying) {
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake() {
        instance = this as T;
    }
}
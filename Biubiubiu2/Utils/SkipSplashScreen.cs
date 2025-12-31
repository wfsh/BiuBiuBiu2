using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

[Preserve]
public class SkipSplashScreen
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void BeforeSplashScreen() {
        Application.focusChanged += OnFocusChanged;
    }

    private static void OnFocusChanged(bool obj) {
        Application.focusChanged -= OnFocusChanged;
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
}

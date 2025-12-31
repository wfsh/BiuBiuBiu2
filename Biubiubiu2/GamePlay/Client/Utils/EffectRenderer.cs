using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRenderer : MonoBehaviour {

    [SerializeField]
    private Renderer[] renderers;

    public Renderer[] GetRenderers() {
        return renderers;
    }
}
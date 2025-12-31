using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class EntityMaterial : MonoBehaviour {
        [SerializeField]
        private Material[] materials;

        [SerializeField]
        private Renderer[] renderers;

        public Material GetOtherTeamMaterial() {
            return materials[1];
        }

        public Renderer[] GetRenderers() {
            return renderers;
        }
    }
}
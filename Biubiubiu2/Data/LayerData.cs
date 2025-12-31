using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class LayerData {
        public static readonly int ServerLayerMask = 1 << LayerMask.NameToLayer("ServerLayer");
        public static readonly int ClientLayerMask = 1 << LayerMask.NameToLayer("ClientLayer");
        public static readonly int DefaultLayerMask = 1 << LayerMask.NameToLayer("Default");
        public static readonly int TerrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");
        public static readonly int BeHitPartMask = 1 << LayerMask.NameToLayer("BeHitPart");
        public static readonly int RoleLayerMask = 1 << LayerMask.NameToLayer("Role");
        public static readonly int AirWallLayerMask = 1 << LayerMask.NameToLayer("AirWall");
    }
}
using System;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.CoreMessage {

    public class M_AssetLoad {
        public struct AssetLoader : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AssetLoader>();
            public int GetID() => _id;
            // 下面写你的参数
            public string Url;
            public long LoadID;
            public Action<UnityEngine.Object> onCompleted;
        }

        public struct AssetUnloader : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AssetLoader>();
            public int GetID() => _id;
            // 下面写你的参数
            public string Url;
            public long LoadID;
        }

        public struct AssetBundleUnloader : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AssetLoader>();
            public int GetID() => _id;
            // 下面写你的参数
            public string BundleName;
            public bool UnloadAsset;
            public long LoadID;
        }

        public struct AssetBundleLoader : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AssetLoader>();
            public int GetID() => _id;
            // 下面写你的参数
            public string Url;
            public long LoadID;
            public Action<bool> onCompleted;
        }
    }

}
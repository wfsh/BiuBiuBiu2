using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIItemPack : ServerGPOItemPack {
        private const int MaxPackGrid = 999; // 背包最大格子数量
        private const int MaxQuickPackGrid = 0; // 快捷背包最大格子数量
        protected override void OnAwake() {
            base.OnAwake();
            InitPack(MaxPackGrid, MaxQuickPackGrid);
        }
    }
}
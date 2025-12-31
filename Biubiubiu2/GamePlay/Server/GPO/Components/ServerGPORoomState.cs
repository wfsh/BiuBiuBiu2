using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine.Profiling;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPORoomState : ComponentBase {
        private const float CHECK_INTERVAL = 0.5f;

        private bool inRoom;
        private bool updating;
        private bool enableCheck;
        private float checkTimer;
        private List<RoomArea> roomAreas;

        protected override void OnAwake() {
            Register<SE_GPO.Event_GetIsInRoom>(OnGetIsInRoomCallBack);
            Register<SE_GPO.Event_SetCheckInRoomEnable>(OnSetCheckRoomEnableCallBack);
        }

        protected override void OnStart() {
            MsgRegister.Dispatcher(new SM_Scene.GetRoomAreas {
                CallBack = OnGetRoomAreasCallBack
            });
        }

        protected override void OnClear() {
            roomAreas = null;
            Unregister<SE_GPO.Event_GetIsInRoom>(OnGetIsInRoomCallBack);
            Unregister<SE_GPO.Event_SetCheckInRoomEnable>(OnSetCheckRoomEnableCallBack);
            if (updating) {
                RemoveUpdate(OnUpdate);
                updating = false;
            }
        }

        private void OnUpdate(float dt) {
            checkTimer -= dt;
            if (checkTimer > 0) {
                return;
            }
            checkTimer = CHECK_INTERVAL;
            RefreshInRoom();
        }

        private void OnSetCheckRoomEnableCallBack(ISystemMsg body, SE_GPO.Event_SetCheckInRoomEnable ent) {
            enableCheck = ent.IsEnable;
            RefreshUpdateFunc();
        }

        private void OnGetRoomAreasCallBack(List<RoomArea> roomAreas) {
            this.roomAreas = roomAreas;
            RefreshUpdateFunc();
        }

        private void RefreshUpdateFunc() {
            // TODO 现阶段不用同步 InRoom 的状态，所以不需要循环更新
            return;
            var needUpdate = enableCheck && roomAreas != null && roomAreas.Count > 0;
            if (needUpdate && !updating) {
                AddUpdate(OnUpdate);
                updating = true;
            } else if (!needUpdate && updating) {
                RemoveUpdate(OnUpdate);
                updating = false;
            }
        }

        private void OnGetIsInRoomCallBack(ISystemMsg body, SE_GPO.Event_GetIsInRoom ent) {
            if (!enableCheck || roomAreas == null || roomAreas.Count == 0) {
                ent.CallBack(false);
                return;
            }
            RefreshInRoom();
            ent.CallBack(inRoom);
        }

        private void RefreshInRoom() {
            var result = false;
            var pos = iEntity.GetPoint();
            foreach (var roomArea in roomAreas) {
                if (roomArea.obb.Contains(pos)) {
                    result = true;
                    break;
                }
            }
            if (inRoom == result) {
                return;
            }
            inRoom = result;
            OnInRoomChange();
        }

        protected virtual void OnInRoomChange() { }
    }
}
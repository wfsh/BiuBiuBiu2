using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIMachineGunHeadRota : ComponentBase {
        private Vector3 rpcTargetRota = Vector3.zero;
        private Transform rootBody; // 上半身的Transform
        private Transform gunBody; // 上半身的Transform
        private float barrelElevationLimit = -30f; // 基座的最大上抬角度
        private float gunElevationLimit = -180f; // 炮口的最大上抬角度（基座+枪）
        private float rotationSpeed = 10f; // 上半身旋转速度
        private float syncInterval = 0.1f; // 同步间隔
        private ClientGPO driveGPO;
        private float elevationAngle;

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            if (isSetEntityObj == false) {
                return;
            }
            UpdateRpcMachineGunUpperBodyRota();
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            RemoveProtoCallBack(Proto_AI.Rpc_SyncMachineGunUpperBodyRota.ID, OnSyncMachineGunUpperBodyRota);
            base.OnClear();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            rootBody = iEntity.GetBodyTran(GPOData.PartEnum.RootBody);
            gunBody = iEntity.GetBodyTran(GPOData.PartEnum.RightHand);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.Rpc_SyncMachineGunUpperBodyRota.ID, OnSyncMachineGunUpperBodyRota);
        }
        

        private void OnSyncMachineGunUpperBodyRota(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.Rpc_SyncMachineGunUpperBodyRota)docData;
            rpcTargetRota = rpcData.eulerAngles;
        }

        // 网络同步旋转
        private void UpdateRpcMachineGunUpperBodyRota() {
            var upperBodyCurrentRotation = rootBody.rotation;
            var upperBodyTargetRotation = Quaternion.Euler(new Vector3(0,rpcTargetRota.y,0) ); // 使用 eulerAngles 设置旋转
            rootBody.rotation = Quaternion.Slerp(upperBodyCurrentRotation, upperBodyTargetRotation, Time.deltaTime * rotationSpeed);
            
            var gunBodyLocalRotation = gunBody.localRotation;
            var gunBodyTargetLocalRatation =  Quaternion.Euler(new Vector3(rpcTargetRota.x,0,0)); // 使用 eulerAngles 设置旋转
            gunBody.localRotation = Quaternion.Slerp(gunBodyLocalRotation, gunBodyTargetLocalRatation, Time.deltaTime * rotationSpeed);
        }
    }
}
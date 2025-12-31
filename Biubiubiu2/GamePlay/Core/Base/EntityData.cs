using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class EntityData : IEntity {
        private bool isPointChange = false;
        private bool isRotaChange = false;
        private bool isScaleChange = false;
        private bool isDebug = false;

        public bool IsPointChange {
            get { return isPointChange; }
        }

        public bool IsRotaChange {
            get { return isRotaChange; }
        }

        public bool IsScaleChange {
            get { return isScaleChange; }
        }

        private int connId = -1;
        private Vector3 point = Vector3.zero;
        private Vector3 scale = Vector3.one;
        private Quaternion rota = Quaternion.identity;
        private IGPO iGPO;
        private bool isClear = false;
        private Transform parent;

        public void Clear() {
            iGPO = null;
            isClear = true;
        }

        public bool IsClear() {
            return isClear;
        }

        virtual public Vector3 GetPoint() {
            if (this.parent != null) {
                return this.parent.position + point;
            }
            return point;
        }

        virtual public void SetPoint(Vector3 point) {
            if (isDebug) {
                Debug.Log(point);
            }
            this.point = point;
            isPointChange = true;
        }

        virtual public Quaternion GetRota() {
            return rota;
        }

        virtual public void SetRota(Quaternion rota) {
            this.rota = rota;
            isRotaChange = true;
        }

        virtual public void SetLocalScale(Vector3 scale) {
            this.scale = scale;
            isScaleChange = true;
        }

        virtual public Vector3 GetLocalScale() {
            return scale;
        }
        
        virtual public Vector3 GetForward() {
            return GetRota() * Vector3.forward;
        }

        virtual public Vector3 GetEulerAngles() {
            return rota.eulerAngles;
        }

        virtual public void SetEulerAngles(Vector3 eulerAngles) {
            rota = Quaternion.Euler(eulerAngles);
            isRotaChange = true;
        }

        public int GetGpoID() {
            return iGPO == null ? 0 : iGPO.GetGpoID();
        }

        public int GetTeamID() {
            return iGPO == null ? -1 : iGPO.GetTeamID();
        }

        public void SetConnID(int connId) {
            this.connId = connId;
        }

        public void LookAT(Vector3 targetPoint) {
            var direction = (targetPoint - GetPoint()).normalized;
            if (direction == Vector3.zero) {
                return;
            }
            SetRota(Quaternion.LookRotation(direction));
        }

        public void SetGpo(IGPO iGPO) {
            this.iGPO = iGPO;
        }

        public IGPO GetGPO() {
            return iGPO;
        }

        public int GetGPOID() {
            return iGPO == null ? 0 : iGPO.GetGpoID();
        }

        public void StartDebug() {
            isDebug = true;
        }

        public string GetName() {
            return "EntityData";
        }

        public Transform GetBodyTran(GPOData.PartEnum partType) {
            return null;
        }
        public List<Transform> GetBodyTranList(GPOData.PartEnum partType) {
            return new List<Transform>();
        }

        public void SetParent(Transform parent) {
            if (parent == null) {
                return;
            }
            this.parent = parent;
        }

        public Transform GetParent() {
            return this.parent;
        }
    }
}
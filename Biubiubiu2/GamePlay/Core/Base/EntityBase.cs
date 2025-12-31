using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class EntityBase : MonoBehaviour, IEntity {
        private Transform myTransform;
        private GameObject myGameObject;
        private IGPO iGPO;
        private int connId = 0;
        private bool isDebug = false;
        private bool isClear = false;
        private int gpoId = 0;
        private GPOData.GPOType gpoType = GPOData.GPOType.NULL;
        private HitType[] bodyTrans;
        private List<IHitType> canHitParts = new List<IHitType>();
        private string gameObjName = "";
        private string setName = "";

        void Awake() {
            myTransform = transform;
            myGameObject = gameObject;
            if (Application.isEditor) {
                gameObjName = gameObject.name.Replace("(Clone)", "");
            }
            bodyTrans = gameObject.GetComponentsInChildren<HitType>(true);
            isClear = false;
            OnInit();
            InitCanHitParts();
        }

        virtual protected void OnInit() {
        }

        public void Clear() {
            iGPO = null;
            isClear = true;
            myTransform = null;
            myGameObject = null;
        }

        void OnDestroy() {
            Clear();
        }
        
        private void InitCanHitParts() {
            canHitParts.Clear();
            for (int i = 0; i < bodyTrans.Length; i++) {
                var hitType = bodyTrans[i];
                hitType.SetEntity(this);
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    continue;
                }
                if (GPOData.CanCheckHitPart(hitType.Part)) {
                    if (hitType.gameObject.GetComponent<Collider>() == true) {
                        canHitParts.Add(hitType);
                    }
                }
            }
        }

        public Vector3 GetPoint() {
            return myTransform.position;
        }

        public Vector3 GetLocalPoint() {
            return myTransform.localPosition;
        }

        public Vector3 GetForward() {
            return myTransform.forward;
        }

        public Quaternion GetRota() {
            return myTransform.rotation;
        }

        public Vector3 GetEulerAngles() {
            return myTransform.eulerAngles;
        }

        public Quaternion GetLocalRota() {
            return myTransform.localRotation;
        }

        public Vector3 GetLocalScale() {
            return myTransform.localScale;
        }

        public Vector3 GetLossyScale() {
            return myTransform.lossyScale;
        }

        public void SetPoint(Vector3 point) {
            myTransform.position = point;
        }

        public void SetLocalPoint(Vector3 localPoint) {
            myTransform.localPosition = localPoint;
        }

        public void SetRota(Quaternion rota) {
            myTransform.rotation = rota;
        }

        public void SetEulerAngles(Vector3 eulerAngles) {
            myTransform.eulerAngles = eulerAngles;
        }

        public void SetLocalRota(Quaternion rota) {
            myTransform.localRotation = rota;
        }

        public void SetLocalScale(Vector3 scale) {
            myTransform.localScale = scale;
        }

        public void Translate(Vector3 point) {
            myTransform.Translate(point);
        }

        public void LookAT(Vector3 point) {
            myTransform.LookAt(point);
        }

        public void LookAT(Transform tran) {
            myTransform.LookAt(tran);
        }

        public Vector3 TransformPoint(Vector3 point) {
            return myTransform.TransformPoint(point);
        }

        public Vector3 InverseTransformPoint(Vector3 point) {
            return myTransform.InverseTransformPoint(point);
        }
        
        public void SetParent(Transform parent) {
            myTransform.SetParent(parent);
            SetLocalPoint(Vector3.zero);
            SetLocalRota(Quaternion.identity);
            SetLocalScale(Vector3.one);
        }
        
        public Transform GetParent() {
            if (myTransform == null) {
                return null;
            }
            return myTransform.parent;
        }

        public int GetObjInstanceID() {
            return gameObject.GetInstanceID();
        }

        public Vector3 TransformDirection(Vector3 dir) {
            return myTransform.TransformDirection(dir);
        }

        public GameObject GetGameObj() {
            return myGameObject;
        }

        public int ChildCount() {
            return myTransform.childCount;
        }

        public Transform GetChild(int index) {
            return myTransform.GetChild(index);
        }

        public void SetGpo(IGPO iGPO) {
            if (iGPO != null) {
                gpoId = iGPO.GetGpoID();
                gpoType = iGPO.GetGPOType();
            } else {
                gpoId = 0;
                gpoType = GPOData.GPOType.NULL;
            }
            this.iGPO = iGPO;
            UpdateGameObjName();
        }

        public void SetConnID(int connId) {
            this.connId = connId;
            UpdateGameObjName();
        }

        public IGPO GetGPO() {
            return iGPO;
        }

        public int GetGPOID() {
            return iGPO == null ? 0 : iGPO.GetGpoID();
        }

        public int GetTeamID() {
            return iGPO == null ? -1 : iGPO.GetTeamID();
        }

        public void SetName(string name) {
            setName = name;
            UpdateGameObjName();
        }

        public string GetName() {
            return $"Conn:{this.connId}_GPO:{gpoId}_TId:{iGPO.GetTeamID()}_{gpoType}_{setName}";
        }

        public void SetActive(bool isTrue) {
            if (gameObject.activeSelf == isTrue) {
                return;
            }
            gameObject.SetActive(isTrue);
        }
        
        public bool ActiveSelf() {
            return gameObject.activeSelf;
        }

        private void UpdateGameObjName() {
            if (Application.isEditor && iGPO != null) {
                gameObject.name = GetName();
                gameObjName = gameObject.name;
            }
        }

        public Transform GetBodyTran(GPOData.PartEnum partType) {
            for (int i = 0; i < bodyTrans.Length; i++) {
                var hitType = bodyTrans[i];
                if (hitType.Part == partType) {
                    return hitType.transform;
                }
            }
            return null;
        }

        public List<Transform> GetBodyTranList(GPOData.PartEnum partType) {
            var list = new List<Transform>();
            for (int i = 0; i < bodyTrans.Length; i++) {
                var hitType = bodyTrans[i];
                if (hitType.Part == partType) {
                    list.Add(hitType.transform);
                }
            }
            return list;
        }
        
        public List<IHitType> GetAllCanHitPart() {
            return canHitParts;
        }
        
        public void StartDebug() {
            isDebug = true;
        }
        
        public bool IsClear() {
            return isClear;
        }
    }
}
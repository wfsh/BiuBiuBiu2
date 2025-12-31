using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Component {
    public class HitType : MonoBehaviour, IHitType {
        public GPOData.PartEnum Part = GPOData.PartEnum.Body;
        public GPOData.LayerEnum Layer = GPOData.LayerEnum.World;
        [NonSerialized]
        public IEntity MyEntity;
        public void SetEntity(IEntity entity) {
            MyEntity = entity;
        }
        
        public GPOData.PartEnum GetPart() {
            return Part;
        }
        
        public Transform GetTransform() {
            return transform;
        }
        
#if UNITY_EDITOR
        void Start() {
            var hitTypes = gameObject.GetComponents<HitType>();
            if (hitTypes.Length >= 2) {
                Debug.LogError($"不能同时存在 {hitTypes.Length} 个 BodyPart:"+ gameObject);
            }
        }
#endif
    }
}
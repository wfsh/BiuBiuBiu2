using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

public class ServerGoldDashCharacterAIPointCheck : ComponentBase {
    private EntityBase entity;
    private bool isRemoved = false;
    protected override void OnSetEntityObj(IEntity iEntity) {
        base.OnSetEntityObj(iEntity);
        entity = (EntityBase)iEntity;
        AddUpdate(OnUpdate);
    }
    
    protected override void OnClear() {
        RemoveUpdate(OnUpdate);
    }
    
    private void OnUpdate(float delta) {
        if (entity == null || isRemoved) {
            return;
        }
        var pos = entity.GetPoint();
        if (pos.y < -300f) {
            isRemoved = true;
            MsgRegister.Dispatcher(new SM_AI.Event_RemoveAI {
                GpoId = GpoID,
            });
        }
    }
}

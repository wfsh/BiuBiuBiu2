using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSausageAICharacterManager : ManagerBase {
        private List<ServerSausageAICharacterSystem> characterList = new List<ServerSausageAICharacterSystem>();
        
        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Register<SM_AICharacter.AddAICharacter>(OnAddAICharacterCallBack);
            MsgRegister.Register<SM_AICharacter.RemoveAICharacter>(OnRemoveAICharacterCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_AICharacter.AddAICharacter>(OnAddAICharacterCallBack);
            MsgRegister.Unregister<SM_AICharacter.RemoveAICharacter>(OnRemoveAICharacterCallBack);
        }

        public void OnAddAICharacterCallBack(SM_AICharacter.AddAICharacter ent) {
            if (GetAICharacter(ent.AiId) == null) {
                var teamId = GetTeamId(ent.TeamId);
                AddSystem(delegate(ServerSausageAICharacterSystem character) {
                    character.SetCharacterData(ent.AiId, ent.Name, teamId);
                    characterList.Add(character);
                });
            }
        }
        
        public void OnRemoveAICharacterCallBack(SM_AICharacter.RemoveAICharacter ent) {
            var aiCharacter = GetAICharacter(ent.AiId);
            aiCharacter?.ClearImmediate();
        }
        
        private ServerSausageAICharacterSystem GetAICharacter(long aiId) {
            for (int i = 0; i < characterList.Count; i++) {
                var character = characterList[i];
                if (character.AIId == aiId) {
                    return character;
                }
            }
            return null;
        }
    }
}
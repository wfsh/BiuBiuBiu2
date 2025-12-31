using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Playable.Runtime;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Component {
    public partial class SausagePlayable {
        /// <summary>
        ///  ------------------------------------------  编辑器用代码 -------------------------------------------
        /// </summary>
        public enum OperationType {
            PlayClip,
            PlayBlendTree,
            SetGroupSign,
        }

        // 条件设置历史记录类
        public class ConditionHistory {
            public string Sign; // 条件 Key
            public OperationType OperationType;
            public float Timestamp; // 时间戳
        }

        public class EditorPlayableData {
            public string PlayableSign;
            public SausagePlayable SPlayable;
            public Animator Animator;
            public PlayableController PlayableController;
            public List<ConditionHistory> ConditionHistoryList;
            public float StartTimestamp;
        }

#if UNITY_EDITOR
        public static Dictionary<string, EditorPlayableData> EditorPlayableDataMap =
            new Dictionary<string, EditorPlayableData>() {
            };

        /// <summary>
        /// 同时采集 Animator 和 PlayableController 的数据，用于编辑器中的数据展示
        /// </summary>
        /// <param name="playableSign"></param>
        /// <param name="animator"></param>
        /// <param name="playableController"></param>
        /// <param name="conditionDatas"></param>
        public static void AddPlayableData(string playableSign, Animator animator, SausagePlayable sausagePlayable, PlayableController playableController) {
            if (!EditorPlayableDataMap.ContainsKey(playableSign)) {
                EditorPlayableDataMap.Add(playableSign, new EditorPlayableData() {
                    PlayableSign = playableSign,
                    Animator = animator,
                    SPlayable = sausagePlayable,
                    PlayableController = playableController,
                    ConditionHistoryList = new List<ConditionHistory>(),
                    StartTimestamp = Time.realtimeSinceStartup,
                });
            }
        }

        public static void SetOperationType(string playableSign, string playSign, OperationType operationType) {
            if (EditorPlayableDataMap.ContainsKey(playableSign)) {
                var editorPlayableData = EditorPlayableDataMap[playableSign];
                editorPlayableData.ConditionHistoryList.Add(new ConditionHistory() {
                    Sign = playSign,
                    OperationType = operationType,
                    Timestamp = Time.realtimeSinceStartup - editorPlayableData.StartTimestamp,
                });
            }
        }

        public static void RemovePlayableData(string playableSign) {
            if (EditorPlayableDataMap.ContainsKey(playableSign)) {
                EditorPlayableDataMap.Remove(playableSign);
            }
        }
#endif
    }
}

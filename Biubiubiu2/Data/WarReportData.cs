
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class WarReportData {
        public static string WarReportUrl = "XXX";
        public static byte[] PlayWarReportBytes = null;
        public static bool IsStartSausageWarReport = false;
        
        public enum WarReportState {
            None,
            Saveing,
            Playing,
            Stoped,
        }
        
        public enum ReportFrameType {
            Mode,
            CreateNetwork,
            DestroyNetwork,
            RpcData,
            TargetRpcData,
            SyncCharacterData,
            CharacterPoint,
            CharacterRota,
        }
        public enum CharacterSyncType {
            PlayerId,
            NickName,
            GpoId,
            TeamId,
            JumpType,
            FlyType,
            StandType,
            IsDodge,
            UseHoldOn,
            Level,
            Hp,
            MaxHp,
            Atk,
            Def,
        }
        
        public class FrameData {
            public byte NetworkId;
            public ReportFrameType ReportType;
            public float Time;
        }
        
        public class ModeData : FrameData {
            public int ModeId;
            public int MatchId;
            public int SceneId;
        }
        
        public class CreateNetworkData : FrameData {
            public int CreateConnId;
            public bool IsCharacter;
        }
        
        public class CharacterSyncData : FrameData {
            public CharacterSyncType SyncType;
            public string Value;
        }
        
        public class CharacterPointData : FrameData {
            public Vector3 Position;
        }
        
        public class CharacterRotaData : FrameData {
            public Quaternion Rotation;
        }
        
        public class RpcFrameData: FrameData  {
            public byte TargetNetworkId;
            public int ConnId;
            public byte Channel;
            public byte[] Bytes;
        }
        
        public static WarReportState ReportState { get; private set; } = WarReportState.None;
        public static void SetWarReportState(WarReportState state) {
            Debug.Log("[WarReport]SetWarReportState:" + state);
            ReportState = state;
        }
        
        public static bool IsStartWarReport() {
            return IsStartSausageWarReport;
        }
        
        public static void Dispose() {
            IsStartSausageWarReport = false;
            PlayWarReportBytes = null;
            ReportState = WarReportState.None;
        }
    }
}
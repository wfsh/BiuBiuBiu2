using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Item {
        public const byte ModID = 8;

        public struct Rpc_AddDropItem : IRpc {
            public const string ID = "Proto_Item.Rpc_AddDropItem";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 1;
            public int itemId;
            public int autoItemId;
            public ushort itemNum;
            public Vector3 point;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(itemId);
                buffer.Write(autoItemId);
                buffer.Write(itemNum);
                buffer.Write(point);
            }

            public void UnSerialize(ByteBuffer buffer) {
                itemId = buffer.ReadInt();
                autoItemId = buffer.ReadInt();
                itemNum = buffer.ReadUShort();
                point = buffer.ReadVector3();
            }
        }

        public struct Rpc_DiscardItem : IRpc {
            public const string ID = "Proto_Item.Rpc_DiscardItem";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 2;
            public int autoItemId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
            }
        }

        public struct Rpc_PickUpItem : IRpc {
            public const string ID = "Proto_Item.Rpc_PickUpItem";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 3;
            public int autoItemId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
            }
        }

        public struct Cmd_PickItem : ICmd {
            public const string ID = "Proto_Item.Cmd_PickItem";
            public string GetID() => ID;
            public const byte FuncID = 4;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct TargetRpc_PickItem : ITargetRpc {
            public const string ID = "Proto_Item.TargetRpc_PickItem";
            public string GetID() => ID;
            public const byte FuncID = 5;
            public int itemId;
            public int autoItemId;
            public ushort itemNum;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(itemId);
                buffer.Write(autoItemId);
                buffer.Write(itemNum);
            }

            public void UnSerialize(ByteBuffer buffer) {
                itemId = buffer.ReadInt();
                autoItemId = buffer.ReadInt();
                itemNum = buffer.ReadUShort();
            }
        }

        public struct TargetRpc_ItemNumChange : ITargetRpc {
            public const string ID = "Proto_Item.TargetRpc_ItemNumChange";
            public string GetID() => ID;
            public const byte FuncID = 6;
            public int autoItemId;
            public ushort itemNum;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
                buffer.Write(itemNum);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
                itemNum = buffer.ReadUShort();
            }
        }

        public struct Cmd_UseAbilityItem : ICmd {
            public const string ID = "Proto_Item.Cmd_UseAbilityItem";
            public string GetID() => ID;
            public const byte FuncID = 7;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int autoItemId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_RemoveItem : ITargetRpc {
            public const string ID = "Proto_Item.TargetRpc_RemoveItem";
            public string GetID() => ID;
            public const byte FuncID = 8;
            public int autoItemId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
            }
        }

        public struct Cmd_EquipItem : ICmd {
            public const string ID = "Proto_Item.Cmd_EquipItem";
            public string GetID() => ID;
            public const byte FuncID = 9;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int autoItemId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
            }
        }

        public struct Cmd_UseHoldBallItem : ICmd {
            public const byte FuncID = 10;
            public const string ID = "Proto_Character.Cmd_UseHoldBallItem";
            public string GetID() => ID;
            public int autoItemId;
            public Vector3[] points;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
                var len = points.Length;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(points[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
                var len = buffer.ReadInt();
                points = new Vector3[len];
                for (int i = 0; i < len; i++) {
                    points[i] = buffer.ReadVector3();
                }
            }
        }

        public struct TargetRpc_AddQuickPackItem : ITargetRpc {
            public const string ID = "Proto_Item.TargetRpc_AddQuickPackItem";
            public string GetID() => ID;
            public const byte FuncID = 11;
            public int autoItemId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_RemoveQuickPackItem : ITargetRpc {
            public const string ID = "Proto_Item.TargetRpc_RemoveQuickPackItem";
            public string GetID() => ID;
            public const byte FuncID = 12;
            public int autoItemId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
            }
        }

        public struct Cmd_RemoveQuickPackItem : ICmd {
            public const string ID = "Proto_Item.Cmd_RemoveQuickPackItem";
            public string GetID() => ID;
            public const byte FuncID = 13;
            public int autoItemId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(autoItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                autoItemId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_AddDropItem : ITargetRpc {
            public const string ID = "Proto_Item.TargetRpc_AddDropItem";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 14;
            public int itemId;
            public int autoItemId;
            public ushort itemNum;
            public Vector3 point;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(itemId);
                buffer.Write(autoItemId);
                buffer.Write(itemNum);
                buffer.Write(point);
            }

            public void UnSerialize(ByteBuffer buffer) {
                itemId = buffer.ReadInt();
                autoItemId = buffer.ReadInt();
                itemNum = buffer.ReadUShort();
                point = buffer.ReadVector3();
            }
        }
        
        public struct TargetRpc_DropItemData : ITargetRpc {
            public const string ID = "Proto_Item.TargetRpc_DropItemData";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 15;
            public int itemId;
            public int maxItemNum;
            public Vector3 point;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(itemId);
                buffer.Write(maxItemNum);
                buffer.Write(point);
            }

            public void UnSerialize(ByteBuffer buffer) {
                itemId = buffer.ReadInt();
                maxItemNum = buffer.ReadInt();
                point = buffer.ReadVector3();
            }
        }
        
        public struct TargetRpc_GetMaxDropItemNum : ITargetRpc {
            public const string ID = "Proto_Item.TargetRpc_GetMaxDropItemNum";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 16;
            public int maxItemNum;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(maxItemNum);
            }

            public void UnSerialize(ByteBuffer buffer) {
                maxItemNum = buffer.ReadInt();
            }
        }

        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case Rpc_AddDropItem.FuncID:
                    doc = new Rpc_AddDropItem();
                    break;
                case Rpc_DiscardItem.FuncID:
                    doc = new Rpc_DiscardItem();
                    break;
                case Rpc_PickUpItem.FuncID:
                    doc = new Rpc_PickUpItem();
                    break;
                case TargetRpc_PickItem.FuncID:
                    doc = new TargetRpc_PickItem();
                    break;
                case TargetRpc_ItemNumChange.FuncID:
                    doc = new TargetRpc_ItemNumChange();
                    break;
                case TargetRpc_RemoveItem.FuncID:
                    doc = new TargetRpc_RemoveItem();
                    break;
                case TargetRpc_AddQuickPackItem.FuncID:
                    doc = new TargetRpc_AddQuickPackItem();
                    break;
                case TargetRpc_RemoveQuickPackItem.FuncID:
                    doc = new TargetRpc_RemoveQuickPackItem();
                    break;
                case TargetRpc_AddDropItem.FuncID:
                    doc = new TargetRpc_AddDropItem();
                    break;
                case TargetRpc_DropItemData.FuncID:
                    doc = new TargetRpc_DropItemData();
                    break;
                case TargetRpc_GetMaxDropItemNum.FuncID:
                    doc = new TargetRpc_GetMaxDropItemNum();
                    break;
                default:
                    Debug.LogError("Proto_Network:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
        
        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_PickItem.FuncID:
                    doc = new Cmd_PickItem();
                    break;
                case Cmd_UseAbilityItem.FuncID:
                    doc = new Cmd_UseAbilityItem();
                    break;
                case Cmd_EquipItem.FuncID:
                    doc = new Cmd_EquipItem();
                    break;
                case Cmd_UseHoldBallItem.FuncID:
                    doc = new Cmd_UseHoldBallItem();
                    break;
                case Cmd_RemoveQuickPackItem.FuncID:
                    doc = new Cmd_RemoveQuickPackItem();
                    break;
                default:
                    Debug.LogError("Proto_GPO:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}
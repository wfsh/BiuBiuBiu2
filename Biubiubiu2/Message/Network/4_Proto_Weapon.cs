using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Weapon {
        public const byte ModID = 4;

        public struct Cmd_GunFire : ICmd {
            public const string ID = "Proto_Weapon.Cmd_GunFire";
            public string GetID() => ID;
            public const byte FuncID = 1;
            public int weaponId;
            public Vector3 startPoint;
            public Vector3 targetPoint;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startPoint);
                buffer.Write(targetPoint);
                buffer.Write(weaponId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startPoint = buffer.ReadVector3();
                targetPoint = buffer.ReadVector3();
                weaponId = buffer.ReadInt();
            }
        }

        /// <summary>
        /// 大车轮攻击
        /// </summary>
        public struct Cmd_BatLoopAttack : ICmd {
            public const string ID = "Proto_Weapon.Cmd_BatLoopAttack";
            public string GetID() => ID;
            public const byte FuncID = 2;
            public bool IsAttack;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(IsAttack);
            }

            public void UnSerialize(ByteBuffer buffer) {
                IsAttack = buffer.ReadBoolean();
            }
        }

        /// <summary>
        /// 扇形击退
        /// </summary>
        public struct Cmd_BatEndAttack : ICmd {
            public const string ID = "Proto_Weapon.Cmd_BatEndAttack";
            public string GetID() => ID;
            public const byte FuncID = 3;
            public Quaternion startRota;
            public Vector3 startPoint;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startRota);
                buffer.Write(startPoint);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startRota = buffer.ReadQuaternion();
                startPoint = buffer.ReadVector3();
            }
        }

        /// <summary>
        /// 划
        /// </summary>
        public struct Cmd_PlungerAttack : ICmd {
            public const string ID = "Proto_Weapon.Cmd_PlungerAttack";
            public string GetID() => ID;
            public const byte FuncID = 4;
            public Quaternion startRota;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startRota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startRota = buffer.ReadQuaternion();
            }
        }

        /// <summary>
        /// 击飞
        /// </summary>
        public struct Cmd_PlungerEnd : ICmd {
            public const string ID = "Proto_Weapon.Cmd_PlungerEnd";
            public string GetID() => ID;
            public const byte FuncID = 5;
            public Quaternion startRota;
            public Vector3 startPoint;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startRota);
                buffer.Write(startPoint);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startRota = buffer.ReadQuaternion();
                startPoint = buffer.ReadVector3();
            }
        }

        public struct Rpc_Reload : IRpc {
            public const string ID = "Proto_Weapon.Rpc_Reload";
            public string GetID() => ID;
            public const byte FuncID = 6;
            public bool isTrue;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isTrue);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isTrue = buffer.ReadBoolean();
            }
        }

        public struct Rpc_GunFire : IRpc {
            public const string ID = "Proto_Weapon.Rpc_GunFire";
            public string GetID() => ID;
            public const byte FuncID = 7;
            public bool isTrue;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isTrue);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isTrue = buffer.ReadBoolean();
            }
        }

        public struct TargetRpc_AddGun : ITargetRpc {
            public const byte FuncID = 8;
            public const string ID = "Proto_Weapon.TargetRpc_AddGun";
            public string GetID() => ID;
            public string gunSign;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gunSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gunSign = buffer.ReadString();
            }
        }

        public struct Rpc_UseWeapon : IRpc {
            public const byte FuncID = 9;
            public const string ID = "Proto_Weapon.Rpc_UseWeapon";
            public string GetID() => ID;
            public int weaponId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(weaponId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                weaponId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_UseMelee : ITargetRpc {
            public const byte FuncID = 10;
            public const string ID = "Proto_Weapon.TargetRpc_UseMelee";
            public string GetID() => ID;
            public string meleeSign;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(meleeSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                meleeSign = buffer.ReadString();
            }
        }

        public struct TargetRpc_AddMelee : ITargetRpc {
            public const byte FuncID = 11;
            public const string ID = "Proto_Weapon.TargetRpc_AddMelee";
            public string GetID() => ID;
            public string meleeSign;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(meleeSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                meleeSign = buffer.ReadString();
            }
        }

        public struct Cmd_AttackAnim : ICmd {
            public const byte FuncID = 12;
            public const string ID = "Proto_Weapon.Cmd_AttackAnim";
            public string GetID() => ID;
            public int attackAnimId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(attackAnimId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                attackAnimId = buffer.ReadInt();
            }
        }

        public struct Rpc_AttackAnim : IRpc {
            public const byte FuncID = 13;
            public const string ID = "Proto_Weapon.Rpc_AttackAnim";
            public string GetID() => ID;
            public int attackAnim;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(attackAnim);
            }

            public void UnSerialize(ByteBuffer buffer) {
                attackAnim = buffer.ReadInt();
            }
        }

        public struct TargetRpc_AddWeapon : ITargetRpc {
            public const byte FuncID = 14;
            public const string ID = "Proto_Weapon.TargetRpc_AddWeapon";
            public string GetID() => ID;
            public int itemId;
            public int weaponId;
            public int skinItemId;
            public int ATK;
            public int magazineNum;
            public float intervalTime;
            public float reloadTime;
            public float fireOverHotTime;
            public float FireDistance;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(itemId);
                buffer.Write(skinItemId);
                buffer.Write(weaponId);
                buffer.Write(ATK);
                buffer.Write(magazineNum);
                buffer.Write(intervalTime);
                buffer.Write(reloadTime);
                buffer.Write(FireDistance);
                buffer.Write(fireOverHotTime);
            }

            public void UnSerialize(ByteBuffer buffer) {
                itemId = buffer.ReadInt();
                skinItemId = buffer.ReadInt();
                weaponId = buffer.ReadInt();
                ATK = buffer.ReadInt();
                magazineNum = buffer.ReadInt();
                intervalTime = buffer.ReadFloat();
                reloadTime = buffer.ReadFloat();
                FireDistance = buffer.ReadFloat();
                fireOverHotTime = buffer.ReadFloat();
            }
        }

        public struct TargetRpc_UseWeapon : ITargetRpc {
            public const byte FuncID = 15;
            public const string ID = "Proto_Weapon.TargetRpc_UseWeapon";
            public string GetID() => ID;
            public int weaponId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(weaponId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                weaponId = buffer.ReadInt();
            }
        }

        public struct Rpc_AddWeapon : IRpc {
            public const byte FuncID = 16;
            public const string ID = "Proto_Weapon.Rpc_AddWeapon";
            public string GetID() => ID;
            public int itemId;
            public int skinItemId;
            public int weaponId;
            public int ATK;
            public int magazineNum;
            public float intervalTime;
            public float reloadTime;
            public float FireDistance;
            public float fireOverHotTime;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(itemId);
                buffer.Write(skinItemId);
                buffer.Write(weaponId);
                buffer.Write(ATK);
                buffer.Write(magazineNum);
                buffer.Write(intervalTime);
                buffer.Write(reloadTime);
                buffer.Write(FireDistance);
                buffer.Write(fireOverHotTime);
            }

            public void UnSerialize(ByteBuffer buffer) {
                itemId = buffer.ReadInt();
                skinItemId = buffer.ReadInt();
                weaponId = buffer.ReadInt();
                ATK = buffer.ReadInt();
                magazineNum = buffer.ReadInt();
                intervalTime = buffer.ReadFloat();
                reloadTime = buffer.ReadFloat();
                FireDistance = buffer.ReadFloat();
                fireOverHotTime = buffer.ReadFloat();
            }
        }

        public struct Rpc_TakeBackWeapon : IRpc {
            public const byte FuncID = 17;
            public const string ID = "Proto_Weapon.Rpc_RemoveWeapon";
            public string GetID() => ID;
            public int weaponId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(weaponId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                weaponId = buffer.ReadInt();
            }
        }

        public struct Cmd_TakeBackWeapon : ICmd {
            public const byte FuncID = 18;
            public const string ID = "Proto_Weapon.Cmd_TakeBackWeapon";
            public string GetID() => ID;
            public int weaponId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(weaponId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                weaponId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_WeaponBullet : ITargetRpc {
            public const byte FuncID = 19;
            public const string ID = "Proto_Weapon.TargetRpc_WeaponBullet";
            public string GetID() => ID;
            public int weaponId;
            public ushort bulletCount;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(weaponId);
                buffer.Write(bulletCount);
            }

            public void UnSerialize(ByteBuffer buffer) {
                weaponId = buffer.ReadInt();
                bulletCount = buffer.ReadUShort();
            }
        }

        public struct TargetRpc_UseBullet : ITargetRpc {
            public const byte FuncID = 20;
            public const string ID = "Proto_Weapon.TargetRpc_UseBullet";
            public string GetID() => ID;
            public int bulletId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(bulletId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                bulletId = buffer.ReadInt();
            }
        }

        public struct Cmd_UseBullet : ICmd {
            public const byte FuncID = 21;
            public const string ID = "Proto_Weapon.Cmd_UseBullet";
            public string GetID() => ID;
            public int bulletId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(bulletId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                bulletId = buffer.ReadInt();
            }
        }

        public struct Cmd_UseWeapon : ICmd {
            public const string ID = "Proto_Weapon.Cmd_UseWeapon";
            public string GetID() => ID;
            public const byte FuncID = 22;
            public int weaponId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(weaponId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                weaponId = buffer.ReadInt();
            }
        }

        public struct Cmd_HoldOn : ICmd {
            public const string ID = "Proto_Weapon.Cmd_HoldOn";
            public string GetID() => ID;
            public const byte FuncID = 23;
            public string holdSign;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(holdSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                holdSign = buffer.ReadString();
            }
        }

        public struct Cmd_Throw : ICmd {
            public const string ID = "Proto_Weapon.Cmd_Throw";
            public string GetID() => ID;
            public const byte FuncID = 24;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct Rpc_Throw : IRpc {
            public const string ID = "Proto_Weapon.Rpc_Throw";
            public string GetID() => ID;
            public const byte FuncID = 25;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct TargetRpc_TakeBackWeapon : ITargetRpc {
            public const byte FuncID = 26;
            public const string ID = "Proto_Weapon.TargetRpc_TakeBackWeapon";
            public string GetID() => ID;
            public int weaponId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(weaponId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                weaponId = buffer.ReadInt();
            }
        } 

        public struct Cmd_Reload : ICmd {
            public const string ID = "Proto_Weapon.Cmd_Reload";
            public string GetID() => ID;
            public const byte FuncID = 27;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }      
        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_GunFire.FuncID:
                    doc = new Cmd_GunFire();
                    break;
                case Cmd_BatLoopAttack.FuncID:
                    doc = new Cmd_BatLoopAttack();
                    break;
                case Cmd_BatEndAttack.FuncID:
                    doc = new Cmd_BatEndAttack();
                    break;
                case Cmd_PlungerAttack.FuncID:
                    doc = new Cmd_PlungerAttack();
                    break;
                case Cmd_PlungerEnd.FuncID:
                    doc = new Cmd_PlungerEnd();
                    break;
                case Cmd_AttackAnim.FuncID:
                    doc = new Cmd_AttackAnim();
                    break;
                case Cmd_TakeBackWeapon.FuncID:
                    doc = new Cmd_TakeBackWeapon();
                    break;
                case Cmd_UseBullet.FuncID:
                    doc = new Cmd_UseBullet();
                    break;
                case Cmd_UseWeapon.FuncID:
                    doc = new Cmd_UseWeapon();
                    break;
                case Cmd_HoldOn.FuncID:
                    doc = new Cmd_HoldOn();
                    break;
                case Cmd_Throw.FuncID:
                    doc = new Cmd_Throw();
                    break;
                case Cmd_Reload.FuncID:
                    doc = new Cmd_Reload();
                    break;
                default:
                    Debug.LogError("Proto_Weapon:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
        
        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case Rpc_Reload.FuncID:
                    doc = new Rpc_Reload();
                    break;
                case Rpc_GunFire.FuncID:
                    doc = new Rpc_GunFire();
                    break;
                case TargetRpc_AddGun.FuncID:
                    doc = new TargetRpc_AddGun();
                    break;
                case Rpc_UseWeapon.FuncID:
                    doc = new Rpc_UseWeapon();
                    break;
                case TargetRpc_UseMelee.FuncID:
                    doc = new TargetRpc_UseMelee();
                    break;
                case TargetRpc_AddMelee.FuncID:
                    doc = new TargetRpc_AddMelee();
                    break;
                case Rpc_AttackAnim.FuncID:
                    doc = new Rpc_AttackAnim();
                    break;
                case TargetRpc_AddWeapon.FuncID:
                    doc = new TargetRpc_AddWeapon();
                    break;
                case TargetRpc_UseWeapon.FuncID:
                    doc = new TargetRpc_UseWeapon();
                    break;
                case Rpc_AddWeapon.FuncID:
                    doc = new Rpc_AddWeapon();
                    break;
                case Rpc_TakeBackWeapon.FuncID:
                    doc = new Rpc_TakeBackWeapon();
                    break;
                case TargetRpc_WeaponBullet.FuncID:
                    doc = new TargetRpc_WeaponBullet();
                    break;
                case TargetRpc_UseBullet.FuncID:
                    doc = new TargetRpc_UseBullet();
                    break;
                case Rpc_Throw.FuncID:
                    doc = new Rpc_Throw();
                    break;
                case TargetRpc_TakeBackWeapon.FuncID:
                    doc = new TargetRpc_TakeBackWeapon();
                    break;
                default:
                    Debug.LogError("Proto_Weapon:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}
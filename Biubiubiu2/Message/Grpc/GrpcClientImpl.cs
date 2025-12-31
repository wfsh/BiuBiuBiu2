#if BUILD_SERVER
using Grpc.Core;
#endif
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using Google.Protobuf;
using System.IO;
using UnityEngine;

#pragma warning disable 4014
namespace Sofunny.BiuBiuBiu2.Grpc {
    public struct GrpcCallBack {
        public ByteString data;
        public string errMsg;
    }

    public class GrpcClientImpl {
#if BUILD_SERVER
        private long isAlreadyCon;

        //重连次数
        private long tryReconCount;

        // 1 表示需要重连 0 表示连接完成
        private long reconn;
        private readonly string warId;
        private readonly string host;
        private readonly Int32 port;
        private readonly string area;
        private readonly string netConfig;
        private volatile bool isInited;
        private volatile bool isStop;
        private readonly WarServer.WarServerClient client;
        Dictionary<COMMON_MSG_TYPE, Func<string, ByteString, GrpcCallBack>> callbackDict;
        private AsyncDuplexStreamingCall<CommonData, CommonData> stream;
        private Channel channel;
        private Action<bool> connectCallback;
        private readonly SemaphoreSlim queueSemaphore = new SemaphoreSlim(0, int.MaxValue);
        private ConcurrentQueue<SendData> sendQueue = new ConcurrentQueue<SendData>();

        private struct SendData {
            public string sid;
            public COMMON_MSG_TYPE msgType;
            public ByteString data;
            public string targetService;
            public string errMsg;
            public long requestId;
            public Func<Task> asyncComplete;
            public Func<Task> asyncFail;
        }

        public GrpcClientImpl(string addr, string warId, string host, Int32 port, string area, string netConfig,
            Action<bool> connectCallback) {
#if UNITY_EDITOR
            Debug.Log($"GrpcClientImpl1 addr {addr} warId {warId}");
#else
        Console.WriteLine($"GrpcClientImpl1 addr {addr} warId {warId}");
#endif
            this.warId = warId;
            this.host = host;
            this.port = port;
            this.area = area;
            this.netConfig = netConfig;
            this.tryReconCount = 0;
            this.isAlreadyCon = 0;
            this.reconn = 1;
            this.channel = new Channel(addr, ChannelCredentials.Insecure);
            this.client = new WarServer.WarServerClient(this.channel);
            this.callbackDict = new Dictionary<COMMON_MSG_TYPE, Func<string, ByteString, GrpcCallBack>>();
            this.connectCallback = connectCallback;
            this.SendLoop();
            this.StartBidStream();
        }

        public void Dispose() {
            Debug.Log("GrpcClientImp.Dispose Begin");
            this.isStop = true;
            this.stream.Dispose();
            this.channel.ShutdownAsync().Wait();
            Debug.Log("GrpcClientImp.Dispose End");
        }

        // 接收Send返回的结果，或者是game_server直接请求过来的数据
        public void RegisterCallback(COMMON_MSG_TYPE msgType, Func<string, ByteString, GrpcCallBack> cb) {
            Func<string, ByteString, GrpcCallBack> tmp = null;
            if (this.callbackDict.TryGetValue(msgType, out tmp)) {
                Debug.Log("RegisterCallback dup msgType" + msgType);
                return;
            }
            this.callbackDict[msgType] = cb;
        }

        private Func<string, ByteString, GrpcCallBack> GetCallback(COMMON_MSG_TYPE msgType) {
            Func<string, ByteString, GrpcCallBack> cb = null;
            if (callbackDict.TryGetValue(msgType, out cb)) {
                return cb;
            }
            return null;
        }

        public void OnUpdate() {
            // 保留
        }

        // 所有的发往 war-server 的请求只能在 SendLoop 中进行
        // 发往 war-server 只能调用 Send 方法，不能直接调用 this.stream.RequestStream.WriteAsync
        private async Task SendLoop() {
#if UNITY_EDITOR
            Debug.Log($"GRPC SendLoop");
#else
        Console.WriteLine($"GRPC SendLoop");
#endif
            while (!this.isStop) {
                await this.queueSemaphore.WaitAsync();
                while (this.sendQueue.TryDequeue(out var sendData)) {
                    await this.SendProto(sendData.sid, sendData.msgType, sendData.data, sendData.errMsg,
                        sendData.requestId,
                        sendData.targetService, sendData.asyncComplete, sendData.asyncFail);
                }
            }
        }

        #region Send

        // 与好友服务通信
        public void SendToFriend(COMMON_MSG_TYPE msgType, ByteString data) {
            this.Send("", msgType, data, "", 0, "friend");
        }

        // 与game_server通信(包括 matchserver)
        public void Send(string sid, COMMON_MSG_TYPE msgType, ByteString data) {
            this.Send(sid, msgType, data, "", 0, "game_server");
        }

        private void Send(string sid, COMMON_MSG_TYPE msgType, ByteString data, string errMsg, long requestId,
            string targetService) {
            this.Send(sid, msgType, data, errMsg, requestId, targetService, null, null);
        }

        private void Send(string sid, COMMON_MSG_TYPE msgType, ByteString data, string errMsg, long requestId,
            string targetService, Func<Task> asyncComplete, Func<Task> asyncFail) {
            var sendData = new SendData {
                data = data,
                sid = sid,
                msgType = msgType,
                targetService = targetService,
                asyncComplete = asyncComplete,
                asyncFail = asyncFail,
                errMsg = errMsg,
                requestId = requestId,
            };
            sendQueue.Enqueue(sendData);
            this.queueSemaphore.Release();
        }

        private async Task SendProto(string sid, COMMON_MSG_TYPE msgType, ByteString data, string errMsg,
            long requestId,
            string targetService, Func<Task> asyncComplete, Func<Task> asyncFail) {
#if UNITY_EDITOR
            Debug.Log($" GRPC WriteAsync {msgType}");
#else
        Console.WriteLine($" GRPC WriteAsync {msgType}");
#endif
            try {
                await this.stream.RequestStream.WriteAsync(new CommonData {
                    Id = sid,
                    MsgType = msgType,
                    Data = data,
                    TargetService = targetService,
                    ErrMsg = errMsg,
                    RequestId = requestId,
                });
                if (asyncComplete != null) {
                    Task.Run(() => {
                        asyncComplete(); // 这里不使用 await 不需要等待返回
                    });
                }
            } catch (Exception ex) {
#if UNITY_EDITOR
                Debug.Log($"SendProto Exception: {ex.ToString()} MsgType: {msgType}");
#else
            Console.WriteLine($"SendProto Exception: {ex.ToString()} MsgType: {msgType}");
#endif
                if (asyncFail != null) {
                    await asyncFail();
                }
            }
        }

        #endregion

        #region post data

        public void PostReportData(MESSAGE_TYPE msgType, long playerId, ByteString data) {
            this.PostData(POST_DATA_TYPE.Kafka, msgType, playerId, data);
        }

        public void PostEventData(MESSAGE_TYPE msgType, long playerId, ByteString data) {
            this.PostData(POST_DATA_TYPE.Nsq, msgType, playerId, data);
        }
        
        public void PostEventLog(string eventName, string value) {
            this.PostData(POST_DATA_TYPE.Funnydb, MESSAGE_TYPE.WarLog, 0, new LogData() {
                Name = eventName,
                Props = value,
            }.ToByteString());
        }

        public void PostData(POST_DATA_TYPE dataType, MESSAGE_TYPE msgType, long playerId, ByteString data) {
            Task.Run(() => {
                try {
                    var req = new PostDataReq {
                        DataType = dataType, MsgType = msgType, PlayerId = playerId, Data = data,
                    };
                    this.client.PostDataAsync(req); // 异步不等待
                } catch (Exception e) {
#if UNITY_EDITOR
                    Debug.LogError($"grpc PostData exception: e.Message: {e.Message} - dataType: {dataType} - msgType: {msgType} - playerId: {playerId} - data: {data}");
#else
                    Console.WriteLine($"grpc PostData exception: e.Message: {e.Message} - dataType: {dataType} - msgType: {msgType} - playerId: {playerId} - data: {data}");
#endif
                }
            });
        }

        #endregion

        #region StartBidStream

        static long GetTimeStampInMilliseconds() {
            // 获取当前时间
            DateTime currentTime = DateTime.Now;

            // 获取当前时间戳（毫秒）
            TimeSpan timeSpan = currentTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long timeStamp = (long)timeSpan.TotalMilliseconds;
            return timeStamp;
        }

        private async Task StartBidStreamOk() {
#if UNITY_EDITOR
            Debug.LogError("GRPC StartBidStreamOk");
#else
        Console.WriteLine("GRPC StartBidStreamOk");
#endif
            try {
                await this.stream.ResponseStream.MoveNext();
                var curRegister = this.stream.ResponseStream.Current;
                if (!curRegister.ErrMsg.Equals("")) {
#if UNITY_EDITOR
                    Debug.Log($"Error in StartBidStreamOk: {curRegister.ErrMsg}");
#else
                Console.WriteLine($"Error in StartBidStreamOk: {curRegister.ErrMsg}");
#endif
                    return;
                }
#if UNITY_EDITOR
                Debug.Log($"GRPC StartBidStreamOk {curRegister.Data}");
#else
            Console.WriteLine($"GRPC StartBidStreamOk {curRegister.Data}");
#endif

                // 重置重连状态
                Interlocked.Exchange(ref this.reconn, 0);
                Interlocked.Exchange(ref this.tryReconCount, 0);
                Interlocked.Exchange(ref this.isAlreadyCon, 1);
                if (connectCallback != null) {
                    connectCallback.Invoke(true);
                }
                await Task.Run(async () => {
#if UNITY_EDITOR
                    Debug.Log($"GRPC StartBidStreamOk Run while");
#else
                Console.WriteLine($"GRPC StartBidStreamOk Run while");
#endif
                    while (await this.stream.ResponseStream.MoveNext()) {
                        var cur = this.stream.ResponseStream.Current;
                        var cb = GetCallback(cur.MsgType);
                        if (cb == null) {
                            continue;
                        }
                        GrpcCallBack data;
                        try {
                            // Debug.Log($"GRPC callback before, ts: {GetTimeStampInMilliseconds()}, {cur.MsgType}, empty, {cur.RequestId}");
                            data = cb(this.warId, cur.Data);
                            // Debug.Log($"GRPC callback before, ts: {GetTimeStampInMilliseconds()}, {cur.MsgType}, {data.data}, {cur.RequestId}");
                        } catch (Exception e) {
#if UNITY_EDITOR
                            Debug.Log($"GRPC StartBidStreamOk callback run {e}");
#else
                        Console.WriteLine($"GRPC StartBidStreamOk callback run {e}");
#endif
                            this.Send("", cur.MsgType, null, $"Exception({e})", cur.RequestId, "game_server");
                            continue;
                        }
                        if (cur.MsgType == COMMON_MSG_TYPE.InitRoom) {
                            this.isInited = true;
                        }

                        // Debug.Log($"GRPC send ready, ts: {GetTimeStampInMilliseconds()}, {cur.MsgType}, {data.data}, {cur.RequestId}");
                        // 来自 war-server 的主动请求
                        if (!data.data.IsEmpty && cur.RequestId > 0) {
#if UNITY_EDITOR
                            Debug.Log($"GRPC WriteAsync {cur.MsgType}");
#else
                        Console.WriteLine($"GRPC WriteAsync {cur.MsgType}");
#endif
                            // Debug.Log($"GRPC send before, ts: {GetTimeStampInMilliseconds()}, {cur.MsgType}, {data.data}, {cur.RequestId}");
                            this.Send("", cur.MsgType, data.data, data.errMsg, cur.RequestId, "game_server");
                            // Debug.Log($"GRPC send after, ts: {GetTimeStampInMilliseconds()}, {cur.MsgType}, {data.data}, {cur.RequestId}"); 
                        }
                    }
                });
            } catch (RpcException e) {
                if (StatusCode.Cancelled == e.StatusCode) {
#if UNITY_EDITOR
                    Debug.Log("Cancelled Normally");
#else
                Console.WriteLine("Cancelled Normally");
#endif
                    return;
                }
#if UNITY_EDITOR
                Debug.Log($"StartBidStreamOk RPC failed:{e.ToString()}");
#else
            Console.WriteLine($"StartBidStreamOk RPC failed:{e.ToString()}");
#endif
                await this.StartBidStreamFail();
            }
        }

        private async Task StartBidStreamFail() {
#if UNITY_EDITOR
            Debug.Log("GRPC StartBidStreamFail");
#else
        Console.WriteLine("GRPC StartBidStreamFail");
#endif
            if (Interlocked.Read(ref this.tryReconCount) != 0) {
                return;
            }
            Interlocked.Exchange(ref this.reconn, 1);
            Interlocked.Exchange(ref this.isAlreadyCon, 0);
            var delayTime = 1000;
            var maxTry = 20;
            long tryTimes = 0;
            while (true) {
                if (this.tryReconCount >= maxTry / 2) {
                    delayTime = 3000;
                }
                tryTimes = this.tryReconCount;
                await Task.Delay(delayTime);
                if (Interlocked.Read(ref this.tryReconCount) < maxTry && Interlocked.Read(ref this.isAlreadyCon) != 1) {
                    Interlocked.Increment(ref this.tryReconCount);
#if UNITY_EDITOR
                    Debug.Log($"Try {tryReconCount} times, ready to reconnect");
#else
                Console.WriteLine($"Try {tryReconCount} times, ready to reconnect");
#endif
                    await this.StartBidStream();
                } else {
                    if (this.isInited) {
                        Interlocked.Exchange(ref this.tryReconCount, 0);
                        await Task.Delay(60000);
                    } else {
                        break;
                    }
                }
            }
            if (Interlocked.Read(ref this.isAlreadyCon) != 1 && connectCallback != null) {
                connectCallback.Invoke(false);
            }
#if UNITY_EDITOR
            Debug.Log($"Try {tryTimes} times, max times {maxTry}");
#else
        Console.WriteLine($"Try {tryTimes} times, max times {maxTry}");
#endif
        }

        // 建立双向通信
        private async Task StartBidStream() {
#if UNITY_EDITOR
            Debug.Log("GRPC StartBidStream");
#else
        Console.WriteLine("GRPC StartBidStream");
#endif
            try {
                if (isStop || (Interlocked.Read(ref this.reconn) == 0)) {
                    return;
                }
                this.stream = client.BidStream();
#if UNITY_EDITOR
                Debug.Log("GRPC WriteAsync register");
#else
            Console.WriteLine("GRPC WriteAsync register");
#endif
                var registerRoom = new RegisterRoomInfo {
                    WarId = this.warId,
                    Host = this.host,
                    Port = this.port,
                    Area = this.area,
                    NetworkConfig = this.netConfig,
                    Order = getOrder(),
                };
                this.Send(this.warId, COMMON_MSG_TYPE.RegisterRoom, registerRoom.ToByteString(), "", 0, "",
                    this.StartBidStreamOk, this.StartBidStreamFail);
            } catch (RpcException e) {
                if (StatusCode.Cancelled == e.StatusCode) {
#if UNITY_EDITOR
                    Debug.Log("Cancelled Normally");
#else
                Console.WriteLine("Cancelled Normally");
#endif
                    return;
                }
#if UNITY_EDITOR
                Debug.Log($"StartBidStream RPC failed:{e.ToString()}");
#else
            Console.WriteLine($"StartBidStream RPC failed:{e.ToString()}");
#endif
                await this.StartBidStreamFail();
            }
        }

        private int getOrder() {
            try {
                // 读取文件中的内容
                string content = File.ReadAllText("./flag_order");

                // 尝试将读取的内容转换为整数
                if (int.TryParse(content, out int number)) {
                    return number;
                }
            } catch (FileNotFoundException) {
                // 文件不存在，可能是未开启编号的节点，视为0
            } catch (IOException) {
#if UNITY_EDITOR
                Debug.Log("IO error while read order file。");
#else
            Console.WriteLine("IO error while read order file。");
#endif
            } catch (Exception ex) {
#if UNITY_EDITOR
                Debug.Log("Read order file error：" + ex.Message);
#else
            Console.WriteLine("Read order file error：" + ex.Message);
#endif
            }
            return 0;
        }

        #endregion
#else
        public GrpcClientImpl(string addr, string warId, string host, Int32 port, string area, string netConfig,
            Action<bool> connectCallback) {
        }

        public void RegisterCallback(COMMON_MSG_TYPE msgType, Func<string, ByteString, GrpcCallBack> cb) {
        }
        
        public void PostEventData(MESSAGE_TYPE msgType, long playerId, ByteString data) {
        }
        
        public void PostEventLog(string eventName, string value) {
        }

        public void Dispose() {
        }
        
        public void PostReportData(MESSAGE_TYPE msgType, long playerId, ByteString data) {
        }
#endif
    }
}
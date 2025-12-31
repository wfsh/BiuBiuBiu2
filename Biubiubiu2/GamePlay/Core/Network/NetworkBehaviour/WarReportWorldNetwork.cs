using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class WarReportWorldNetwork : WarReportNetworkBase, INetwork {
        override protected void OnStart() {
            MsgRegister.Dispatcher(new M_Network.SetNetwork {
                iNetwork = this
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;

namespace UniversalSend.Models
{
    public class ReceiveManager
    {
        public static event EventHandler SendRequestReceived;//接收到Send-Request事件

        public static event EventHandler AcceptSendRequset;//用户接受Send-Request事件

        public static event EventHandler SendDataReceived;//接收到Send事件

        public static event EventHandler CancelReceived;//接收到Cancel事件

        public static void SendRequestEvent(SendRequestData sendRequestData)
        {
            SendRequestReceived?.Invoke(sendRequestData,EventArgs.Empty);
        }

        public static void SendDataReceivedEvent(ReceiveTask receiveTask)
        {
            SendDataReceived?.Invoke(receiveTask,EventArgs.Empty);
        }

        public static void CancelReceivedEvent()
        {
            CancelReceived?.Invoke(null,EventArgs.Empty);
        }
    }
}

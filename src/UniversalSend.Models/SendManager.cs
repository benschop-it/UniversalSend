using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;

namespace UniversalSend.Models
{
    public class SendManager
    {
        public static event EventHandler SendCreated; //发送任务已创建

        public static event EventHandler SendPrepared;//准备发送

        public static event EventHandler SendResuestSended;//发送请求已发送

        public static event EventHandler SendStarted;//发送已开始

        public static event EventHandler SendStateChanged;//发送状态改变

        public static event EventHandler SendCanceled;//发送任务已被取消

        public static void SendCreatedEvent()
        {
            SendCreated?.Invoke(null,EventArgs.Empty);
        }

        public static void SendPreparedEvent(Device device)
        {
            SendPrepared?.Invoke(device, EventArgs.Empty);
        }

        public static void SendRequestSendedEvent()
        {
            SendResuestSended?.Invoke(null, EventArgs.Empty);
        }

        public static void SendStateChangedEvent()
        {
            SendStateChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void SendStartedEvent()
        {
            SendStarted?.Invoke(null, EventArgs.Empty);
        }

        public static void SendCanceledEvent()
        {
            SendCanceled?.Invoke(null, EventArgs.Empty);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        public void SendMsg(string UserFrom, string UserTo, string MessageData)
        {
            //foreach (var item  in users) 
            //{
            //    if (item.Equals(UserTo)) 
            //    {
            //        string message = DateTime.Now.ToShortTimeString();
            //        var user = users.FirstOrDefault(i => i.Login == UserFrom);
            //        if (user != null)
            //        {
            //            message += ": " + user.Login + " ";
            //        }
            //        message += MessageData;
            //        item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(message);
            //    }
                
            //}


            if (users.FirstOrDefault(u => u.Login == UserTo)!=null)
            {
                // отправка юзеру
                if (users.FirstOrDefault(u=>u.Login == UserTo).operationContext!=null)
                {
                    users.FirstOrDefault(u => u.Login == UserTo).operationContext.GetCallbackChannel<IServerChatCallback>()
                        .MsgCallback($"{DateTime.Now.ToShortTimeString()} | {users.FirstOrDefault(u => u.Login == UserFrom).Login}: {MessageData}");
                }
                //отпечатка у отправителя
                users.FirstOrDefault(u => u.Login == UserFrom).operationContext.GetCallbackChannel<IServerChatCallback>()
                    .MsgCallback($"{DateTime.Now.ToShortTimeString()} | me: {MessageData}");
            }
        }
    }
}

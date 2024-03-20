using System.Collections.Generic;
using System.ServiceModel;

namespace WCF_Library_Server
{
    [ServiceContract(CallbackContract = typeof(IWCF_ServiceChatCallBack))]
    public interface IWCF_Service
    {
        [OperationContract]
        void SendMessage(int User_From_Id,  int User_To_Id, string Content);
    }

    public interface IWCF_ServiceChatCallBack
    {
        [OperationContract(IsOneWay = true)]
        void MessageCallBack(string answer);
    }

}

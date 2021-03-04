using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        void SendTEBucks(int senderid, int receiverid, decimal amount);
        void CreatesTransferInDatabase(int senderid, int receiverid, decimal amount);
        void UpdateReceiverBalance(int receiverid, decimal amount);
        void UpdateSenderBalance(int senderid, decimal amount);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        List<Transfer> ViewAllTransfers(User user);
        void CreatesTransferInDatabase(int senderid, int receiverid, decimal amount);
        void UpdateReceiverBalance(int receiverid, decimal amount);
        void UpdateSenderBalance(int senderid, decimal amount);
    }
}

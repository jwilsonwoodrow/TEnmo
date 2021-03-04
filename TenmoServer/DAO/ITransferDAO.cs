using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        void SendTEBucks(int senderid, int receiverid, decimal amount);
    }
}

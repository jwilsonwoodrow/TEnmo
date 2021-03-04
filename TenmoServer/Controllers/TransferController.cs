using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("transfer")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private ITransferDAO transferDAO;
        public TransferController(ITransferDAO dao)
        {
            this.transferDAO = dao;
        }

        [HttpPut("update/sender/{id}")] //{amount}
    
        [Authorize]
        public void UpdateSenderBalance(int id, decimal amount)
        {
            transferDAO.UpdateSenderBalance(id, amount);

        }

        [HttpPut("update/receiver/{id}")]

        [Authorize]
        public void UpdateReceiverBalance(int id, decimal amount)
        {
            transferDAO.UpdateSenderBalance(id, amount);

        }

        [HttpPost]
        public void CreatesTransferInDatabase(int senderid, int receiverid, decimal amount)
        {
            transferDAO.CreatesTransferInDatabase(senderid, receiverid, amount);
        }
    }
}

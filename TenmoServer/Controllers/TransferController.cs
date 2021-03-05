using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("transfer/")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private ITransferDAO transferDAO;
        public TransferController(ITransferDAO dao)
        {
            this.transferDAO = dao;
        }

        [HttpPut("{id}")] //{amount}
        [Authorize]
        public void UpdateSenderBalance(Transfer transfer)
        {
            transferDAO.UpdateSenderBalance(transfer.AccountFrom, transfer.Amount);
            transferDAO.UpdateReceiverBalance(transfer.AccountTo, transfer.Amount);
        }

        [HttpPost]
        public void CreatesTransferInDatabase(Transfer transfer)
        {
            transferDAO.CreatesTransferInDatabase(transfer.AccountFrom, transfer.AccountTo, transfer.Amount);
        }
    }
}

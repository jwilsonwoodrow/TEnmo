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

        [HttpPut]
        [Authorize]
        public void UpdateBalances()
        {

        }


        [HttpPost]
    }
}

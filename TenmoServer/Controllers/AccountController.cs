using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{ 
    [Route("accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountDAO dao;
        public AccountController(IAccountDAO dao)
        {
            this.dao = dao;
        }

        [HttpGet]
        [Authorize]
        public Account ViewBalance()
        {
            return dao.ViewBalance(User.Identity.Name);
        } 




    }
}

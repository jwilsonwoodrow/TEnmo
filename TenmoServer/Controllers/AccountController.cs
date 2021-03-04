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
    [Route("accounts")] //localhost34334/accounts/balance
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountDAO dao;
        public AccountController(IAccountDAO dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// get the current logged in users information and store it into a user
        /// </summary>
        /// <returns></returns>
        //[HttpGet("accountinfo")]
        //[Authorize]
        //public User AccountExample()
        //{
        //    User newUser = new User();
        //    string userName = User.Identity.Name;
        //    bool isAdmin = User.IsInRole("Admin");
        //    int userId = int.Parse(User.FindFirst("sub").Value);
        //    newUser.Username = userName;
        //    newUser.UserId = userId;

        //    return newUser; 
               
        //}

        [HttpGet("balance")]
        [Authorize]
        public Account ViewBalance()
        {
            User newUser = new User();
            string userName = User.Identity.Name;
            bool isAdmin = User.IsInRole("Admin");
            int userId = int.Parse(User.FindFirst("sub").Value);
            newUser.Username = userName;
            newUser.UserId = userId;

            return dao.ViewBalance(newUser.UserId);   //User.Identity.Name);
        } 




    }
}

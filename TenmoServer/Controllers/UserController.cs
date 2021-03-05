using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserDAO userDao;

        public UserController(IUserDAO dao)
        {
            this.userDao = dao;
        }

        [HttpGet("userlist")]
        [Authorize]
        public List<DisplayUser> GetUsers()
        {
            List<User> users = userDao.GetUsers();
            List<DisplayUser> listOfUsersToDisplay = new List<DisplayUser>();
            foreach (User user in users)
            {
                DisplayUser displayUser = new DisplayUser();
                displayUser.Username = user.Username;
                displayUser.UserId = user.UserId;
                listOfUsersToDisplay.Add(displayUser);
            }
            return listOfUsersToDisplay;

        }

        [HttpGet("{id}")]
        [Authorize]

        public User GetUserById(int id)
        {
           return userDao.GetUserByID(id);
        }

    }
}

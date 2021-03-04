using MenuFramework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using TenmoClient.Data;
namespace TenmoClient.Views
{
    public class MainMenu : ConsoleMenu
    {
        string ApiUrl;
        RestClient client;
        public MainMenu(string ApiUrl)
        {
            this.ApiUrl = ApiUrl;
            client = new RestClient(ApiUrl);
            
            AddOption("View your current balance", ViewBalance)
                .AddOption("View your past transfers", ViewTransfers)
                .AddOption("View your pending requests", ViewRequests)
                .AddOption("Send TE bucks", SendTEBucks)
                .AddOption("Request TE bucks", RequestTEBucks)
                .AddOption("Log in as different user", Logout)
                .AddOption("Exit", Exit);
        }


        

        protected override void OnBeforeShow()
        {
            Console.WriteLine($"TE Account Menu for User: {UserService.GetUserName()}");
        }

        private MenuOptionResult ViewBalance()
        {
            RestRequest request = new RestRequest("accounts/balance");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            IRestResponse<Account> response = client.Get<Account>(request);

            Account userAccount = response.Data;
            Console.WriteLine($"Your Current Balance is {userAccount.Balance}");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewTransfers()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewRequests()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult SendTEBucks()
        {
            RestRequest request = new RestRequest("user/userlist");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<User>> response = client.Get<List<User>>(request);
            foreach (User user in response.Data)
            {
                Console.WriteLine($"User ID: {user.UserId}   Username: {user.Username}");
            }
            Console.WriteLine("");
            int receiverId = GetInteger("Enter ID of desired recipient, 0 to cancel");

            if (receiverId == 0)
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }

            RestRequest request1 = new RestRequest("transfer/pay");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());


            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult RequestTEBucks()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult Logout()
        {
            UserService.SetLogin(new API_User()); //wipe out previous login info
            return MenuOptionResult.CloseMenuAfterSelection;
        }

    }
}

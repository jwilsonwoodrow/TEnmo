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

        Account userAccount;
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

            userAccount = response.Data;
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
            RestRequest userListRequest = new RestRequest("user/userlist");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<User>> userListResponse = client.Get<List<User>>(userListRequest);
            foreach (User user in userListResponse.Data)
            {
                Console.WriteLine($"User ID: {user.UserId}   Username: {user.Username}");
            }
            Console.WriteLine("");
            int receiverId = GetInteger("Enter ID of desired recipient (Press 0 to cancel): ");
            decimal amountToTransfer = GetDecimal("Enter Amount: ");

            if (receiverId == 0)
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }

            //Gets the current users balance 
            RestRequest getBalanceRequest = new RestRequest("accounts/balance");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            IRestResponse<Account> getBalanceResponse = client.Get<Account>(getBalanceRequest);

            Account userAccount = getBalanceResponse.Data;
            if (amountToTransfer > userAccount.Balance)
            {
                Console.WriteLine("You dont have enough money");
                return MenuOptionResult.WaitAfterMenuSelection;
            }

            RestRequest updateSenderBalanceRequest = new RestRequest($"transfer/update/sender/{receiverId}/{amountToTransfer}");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            IRestResponse updateSenderBalanceResponse = client.Put(updateSenderBalanceRequest);

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

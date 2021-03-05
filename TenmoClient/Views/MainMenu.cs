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
            Console.WriteLine($"Your Current Balance is ${userAccount.Balance}");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewTransfers()
        {
            RestRequest viewTransferRequst = new RestRequest("transfer");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            IRestResponse<List<Transfer>> viewTransferResponse = client.Get<List<Transfer>>(viewTransferRequst);

            //gets the username of whoever we are interacting with
            Console.WriteLine($"Transfer ID         From/To            Amount");
            Console.WriteLine("-------------------------------------------------------");
            foreach (Transfer transfer in viewTransferResponse.Data)
            {
                if (transfer.AccountFrom == UserService.GetUserId()) // if u arethe sender, it should be the transfer id of the username of whoever sending to
                {
                    RestRequest GetUserByIdRequest = new RestRequest($"user/{transfer.AccountTo}");
                    client.Authenticator = new JwtAuthenticator(UserService.GetToken());
                    IRestResponse<User> GetUserByIdResponse = client.Get<User>(GetUserByIdRequest);
                    string usernameOfOtherParty = GetUserByIdResponse.Data.Username;
                    Console.WriteLine($"#{transfer.TransferId}                  To: {usernameOfOtherParty}           ${transfer.Amount}");
                }
                else
                {
                    RestRequest GetUserByIdRequest = new RestRequest($"user/{transfer.AccountFrom}");
                    client.Authenticator = new JwtAuthenticator(UserService.GetToken());
                    IRestResponse<User> GetUserByIdResponse = client.Get<User>(GetUserByIdRequest);
                    string usernameOfOtherParty = GetUserByIdResponse.Data.Username;

                    Console.WriteLine($"#{transfer.TransferId}                  From: {usernameOfOtherParty}         ${transfer.Amount}");
                }
            }
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewRequests()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult SendTEBucks()
        {
            //shows the list of users that you can transfer money to
            RestRequest userListRequest = new RestRequest("user/userlist");
            Transfer tranfer = new Transfer();
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<User>> userListResponse = client.Get<List<User>>(userListRequest);
            foreach (User user in userListResponse.Data)
            {
                Console.WriteLine($"User ID: {user.UserId}   Username: {user.Username}");
            }
            Console.WriteLine("");

            //Asks user the account to transfer to and the amount as well. 
            tranfer.AccountTo = GetInteger("Enter ID of desired recipient (Press 0 to cancel): ");
            tranfer.Amount = GetDecimal("Enter Amount: ");
            tranfer.AccountFrom = UserService.GetUserId();
            if (tranfer.AccountTo == 0)
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }

            //Gets the current users balance 
            RestRequest getBalanceRequest = new RestRequest("accounts/balance");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Account> getBalanceResponse = client.Get<Account>(getBalanceRequest);

            Account userAccount = getBalanceResponse.Data;
            if (tranfer.Amount > userAccount.Balance)
            {
                Console.WriteLine("You dont have enough money");
                return MenuOptionResult.WaitAfterMenuSelection;
            }

            //update balances
            RestRequest updateBalancesRequest = new RestRequest($"transfer/{tranfer.AccountTo}");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            updateBalancesRequest.AddJsonBody(tranfer);
            IRestResponse<Transfer> updateBalancesResponse = client.Put<Transfer>(updateBalancesRequest);

            //insert into tranfer log
            RestRequest tranferLogRequest = new RestRequest("transfer");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            tranferLogRequest.AddJsonBody(tranfer);
            IRestResponse<Transfer> transferLogResponse = client.Post<Transfer>(tranferLogRequest);

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

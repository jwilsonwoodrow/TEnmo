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
            try
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
                        string usernameOfOtherParty = GetUsernameById(transfer.AccountTo);
                        Console.WriteLine($"#{transfer.TransferId}                  To: {usernameOfOtherParty}           ${transfer.Amount}");
                    }
                    else
                    {
                        string usernameOfOtherParty = GetUsernameById(transfer.AccountFrom);
                        Console.WriteLine($"#{transfer.TransferId}                  From: {usernameOfOtherParty}         ${transfer.Amount}");
                    }
                }
                Console.WriteLine("");
                int tranferDetailsId = GetInteger("Enter Tranfer Id for details (0 to cancel): ");

                if (tranferDetailsId == 0)
                {
                    return MenuOptionResult.DoNotWaitAfterMenuSelection;
                }



                RestRequest transferDetailsRequest = new RestRequest($"transfer/{tranferDetailsId}");
                IRestResponse<TransferDetails> transferDataResponse = client.Get<TransferDetails>(transferDetailsRequest);
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
                Console.WriteLine("");
                Console.WriteLine("Transfer Details");
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine($"Id: {transferDataResponse.Data.TransferId}");
                Console.WriteLine($"From: {transferDataResponse.Data.FromUsername}");
                Console.WriteLine($"To: {transferDataResponse.Data.ToUsername}");
                Console.WriteLine($"Type: {transferDataResponse.Data.TransferType}");
                Console.WriteLine($"Status: {transferDataResponse.Data.StatusDesc}");
                Console.WriteLine($"Amount: ${transferDataResponse.Data.Amount}");



                return MenuOptionResult.WaitAfterMenuSelection;
            }
            catch
            {
                Console.WriteLine("An error occured");
                return MenuOptionResult.CloseMenuAfterSelection;
            }
        }

        private MenuOptionResult ViewRequests()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult SendTEBucks()
        {
            try
            {

                //shows the list of users that you can transfer money to
                RestRequest userListRequest = new RestRequest("user/userlist");
                Transfer transfer = new Transfer();
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
                IRestResponse<List<User>> userListResponse = client.Get<List<User>>(userListRequest);
                foreach (User user in userListResponse.Data)
                {
                    Console.WriteLine($"User ID: {user.UserId}   Username: {user.Username}");
                }
                Console.WriteLine("");

                //Asks user the account to transfer to and the amount as well. 
                transfer.AccountTo = GetInteger("Enter ID of desired recipient (Press 0 to cancel): ");
                transfer.Amount = GetDecimal("Enter Amount: ");
                transfer.AccountFrom = UserService.GetUserId();
                if (transfer.AccountTo == 0)
                {
                    return MenuOptionResult.DoNotWaitAfterMenuSelection;
                }


                //Gets the current users balance 
                RestRequest getBalanceRequest = new RestRequest("accounts/balance");
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
                IRestResponse<Account> getBalanceResponse = client.Get<Account>(getBalanceRequest);

                Account userAccount = getBalanceResponse.Data;
                if (transfer.Amount > userAccount.Balance)
                {
                    Console.WriteLine("You dont have enough money");
                    return MenuOptionResult.WaitAfterMenuSelection;
                }

                //update balances
                RestRequest updateBalancesRequest = new RestRequest($"transfer/{transfer.AccountTo}");
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
                updateBalancesRequest.AddJsonBody(transfer);
                IRestResponse<Transfer> updateBalancesResponse = client.Put<Transfer>(updateBalancesRequest);

                //insert into tranfer log
                RestRequest tranferLogRequest = new RestRequest("transfer");
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
                tranferLogRequest.AddJsonBody(transfer);
                IRestResponse<Transfer> transferLogResponse = client.Post<Transfer>(tranferLogRequest);

                return MenuOptionResult.WaitAfterMenuSelection;
            }
            catch
            {
                Console.WriteLine("Error");
                return MenuOptionResult.WaitThenCloseAfterSelection;
            }
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

        private string GetUsernameById (int id)
        {
            RestRequest GetUserByIdRequest = new RestRequest($"user/{id}");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<User> GetUserByIdResponse = client.Get<User>(GetUserByIdRequest);
            return GetUserByIdResponse.Data.Username;
        }

    }
}

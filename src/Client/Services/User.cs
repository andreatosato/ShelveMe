using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class User : IUser
    {
        private readonly HttpClient client;
        private UserModel _user;

        public User(HttpClient client)
        {
            this.client = client;
        }

        public async Task<UserModel> GetUserAsync()
        {
            if(_user == null)
                _user = await client.GetFromJsonAsync<UserModel>("/.auth/me");
                
            return _user;
        }
    }

    public class UserVirtual : IUser
    {
        private readonly HttpClient client;
        private UserModel _user;

        public UserVirtual(HttpClient client)
        {
            this.client = client;
        }

        public async Task<UserModel> GetUserAsync()
        {
            if(_user == null)
                _user = new UserModel{ UserId = "VirtualUser"};
                
            return _user;
        }
    }
}

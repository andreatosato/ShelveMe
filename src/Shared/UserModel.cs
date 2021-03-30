using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedModels
{
    public class UserModel
    {
        public string IdentityProvider {get; set;}
        public string UserId {get; set;}
        public string UserDetails {get; set;}
        public string[] UserRoles {get; set;}
    }
}

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RandomUsersV2
{
    public class User
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        
        // construct via json payload
        public User(string payload){
            JObject o = JObject.Parse(payload);
            
            Title = (string)o.SelectToken("results[0].user.name.title");
            FirstName = (string)o.SelectToken("results[0].user.name.first");
            LastName = (string)o.SelectToken("results[0].user.name.last");
            Gender = (string)o.SelectToken("results[0].user.gender");
            Email = (string)o.SelectToken("results[0].user.email");
        }
        
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace RandomUsersV2
{
    public class Program
    {
        // keep a native list of users
        public static IList<User> UserList { get; set; }
        
        public static void Main(string[] args)
        {
            UserList = new List<User>();
            
            // choose a random number of users to get
            Random rnd = new Random();
            int n = rnd.Next(5, 10);
            
            // make n number of async requests
            for(int i = 0; i < n; i++ ){
                RunAsync().Wait();
            }
            
            if(UserList.Count > 0){
                WriteToFile();
                GroupAndPrintUsers();
            }
            
        }
        
        static async Task RunAsync()
        {
            
            string uri = "https://randomuser.me/api/";
            using(HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                string payload = await response.Content.ReadAsStringAsync();
                // construct a new User with the payload
                User user = new User(payload);
                UserList.Add(user);    
            }
        }
        
        // iterate through user list, serialize objects, writing each to file
        public static void WriteToFile()
        {
            string path = "users.txt";
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach(User user in UserList)
                {
                    var serializedUser = JsonConvert.SerializeObject(user);
                    writer.WriteLine(serializedUser);
                }
            }     
        }
        
        // group users by gender and print results to terminal window
        static void GroupAndPrintUsers()
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            
            var grouped = (from user in UserList 
                    group user by user.Gender into byGender
                    select byGender).ToList();
                
                foreach( var tgroup in grouped)
                {
                    Console.WriteLine("------------");
                    Console.WriteLine(textInfo.ToTitleCase(tgroup.Key));
                    Console.WriteLine("------------");
                    foreach(var user in tgroup){
                        var title = textInfo.ToTitleCase(user.Title);
                        var first = textInfo.ToTitleCase(user.FirstName);
                        var last = textInfo.ToTitleCase(user.LastName);
                        var email = user.Email;
                        string listing = title + " " + first + " " + last 
                            + " (" + email + ")";
                        Console.WriteLine(listing);
                       
                    }
                }
                Console.WriteLine("\r\n");
        } 
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace DataLayer
{

    public class UserDataHandler
    {
        private static List<usermodel> userdata;
        public UserDataHandler()
        {
            if (userdata == null)
            {
                userdata = new List<usermodel>()
                {
                    new usermodel()
                    {
                        UserName = "user1",
                        Role="user"
                    },
                    new usermodel()
                    {
                         UserName = "user2",
                        Role="user"
                    },
                     new usermodel()
                    {
                         UserName = "user2",
                        Role="admin"
                    }
                };
            }
        }

        public ICollection<usermodel> GetAllUsers()
        {
            return userdata;
        }


       

    }
}
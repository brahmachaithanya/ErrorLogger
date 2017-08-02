using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataBase;
namespace DataLayer
{

    public class AppDataHandler
    {
        private static  List<application> appdata;
        public AppDataHandler()
        {
            if (appdata == null)
            {
                appdata = new List<application>();
                 
                using (bsadashiDataBaseEntities context = new bsadashiDataBaseEntities())
                {

                    foreach (application app in context.applications)
                    {
                        new application()
                        {
                            app_id = app.app_id,
                            app_name = app.app_name,
                            app_description = app.app_description,
                            owner = app.owner,
                            reg_date = app.reg_date,
                            is_active = app.is_active
                        };
                    }

                }
            }
        }

        public ICollection<application> GetAllApps()
        {
            return appdata;
        }

        //public ICollection<appmodel> GetAppByUser(string user)
        //{
        //    appmodel app = null;

        //    if (appdata.Any(x => x.User.Equals(user)))
        //    {
                
        //        app = appdata.Single((x => x.User.Equals(user)));
        //        appdata.Clear();
        //        appdata.Add(app);
        //    }

        //    return appdata;
        //}
    }
}
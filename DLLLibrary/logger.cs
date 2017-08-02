namespace DLL
{
    using System;
    using System.Net.Http;
    using DataBase;
    
    public class Logger
    {
        /// <summary>
        /// Constructor. Put whatever initialization code in here that you need
        /// </summary>
        private  int SERVICE_PORT;
        private  string SERVICE_URL;
        private  string ADD_ACTION;
        private string app_id;
        public Logger()
        {
         SERVICE_PORT = 62574;
         SERVICE_URL = "http://localhost:{0}/";
         ADD_ACTION = "api/rest";
            app_id = "123";
         }


        /// <summary>
        /// This method is called by the test harness. So inside of it you should call your logger..
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        /// <param name="logLevel">Error Log Level</param>
        /// <param name="ex">Optional Exception</param>
        public  void   Log(string errorMessage, int logLevel, Exception ex)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(String.Format(SERVICE_URL, SERVICE_PORT));
                DataBase.error_log error = new error_log();

                error.app_id = "678";
                error.datetime = DateTime.Now;
                error.error_message = errorMessage;
                error.error_cat_id = "ERR2";
                error.error_id = "abcd";
                error.log_level = logLevel.ToString();
                if (ex.ToString().Length > 2)
                {

                }
                else
                {
                    error.exception = ex.ToString();
                }

                 client.PostAsJsonAsync(ADD_ACTION, error);

                //HttpResponseMessage response=await client.PostAsJsonAsync(ADD_ACTION,error);
                //if (response.IsSuccessStatusCode)
                //{
                //    Console.WriteLine("s");
                //}
                
            }



            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }
    }
}

﻿using DraconiusGoGUI.Enums;
using DraconiusGoGUI.DracoManager;
using System.Threading.Tasks;

namespace DraconiusGoGUI.Captcha.Anti_Captcha
{
    public class AntiCaptchaClient
    {
       
        private const string Host = "api.anti-captcha.com";
        //private  string ClientKey = "xxxxxxx";
        private const string ProxyHost = "xx.xx.xx.xx";
        private const int ProxyPort = 8282;
        private const string ProxyLogin = "";
        private const string ProxyPassword = "";

        private const string UserAgent =
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36"
            ;


        public static async Task<string> SolveCaptcha(Client client, string captchaURL, string apiKey, string googleSiteKey,
            string proxyHost, int proxyPort, string proxyAccount = "", string proxyPassword = "")
        {
            var task1 = AnticaptchaApiWrapper.CreateNoCaptchaTaskProxyless(
                Host,
                apiKey,
                captchaURL, //target website address
                googleSiteKey,
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36"
            );

            return await ProcessTask(client, task1, apiKey);
        }

        private static async Task<string> ProcessTask(Client client, AnticaptchaTask task, string apikey)
        {
            AnticaptchaResult response;

            do
            {
                response = AnticaptchaApiWrapper.GetTaskResult(Host, apikey, task);

                if (response.GetStatus().Equals(AnticaptchaResult.Status.ready))
                {
                    break;
                }

                await Task.Delay(3000);
            } while (response != null && response.GetStatus().Equals(AnticaptchaResult.Status.processing));

            if (response == null || response.GetSolution() == null)
            {
                client.ClientManager.LogCaller(new LoggerEventArgs("Unknown error occurred...", LoggerTypes.FatalError));
                //Console.WriteLine("Response dump:");
                //Console.WriteLine(response);
            }
            else
            {
                //Console.WriteLine("The answer is '" + response.GetSolution() + "'");
            }

            return response.GetSolution();
        }
    }
}
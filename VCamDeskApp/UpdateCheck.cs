using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


//https://docs.microsoft.com/de-de/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client

namespace VCamDeskApp
{
    public class GithubRelease
    {
        public string Tag_name { get; set; } //v0.9.7637.29934
        public string Target_commitish { get; set; } //master
        public string Name { get; set; } //vCamDesk Release v0.9.7637.29934, Description of the release
        public string Draft { get; set; } //false

    }

    class UpdateCheck
    {
        static HttpClient client = new HttpClient();

        static void ShowGithubRelease(GithubRelease githubRelease)
        {
            if (githubRelease != null)
            {
                Console.WriteLine($"Name: {githubRelease.Name} \nTag_name: " +
                $"{githubRelease.Tag_name} \nTarget_commitish: {githubRelease.Target_commitish}");
            }
            else
            {
                Console.WriteLine("Error. Release was null");
            }
        }


        static async Task<GithubRelease> GetGithubReleaseAsync(string path)
        {
            GithubRelease githubRelease = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                githubRelease = await response.Content.ReadAsAsync<GithubRelease>();
            }
            return githubRelease;
        }

   
        public static async Task UpdateCheckAsync(ParentForm parentForm)
        {
            string localAppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string remoteAppVersion = "";
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://api.github.com/repos/tharmes42/vCamDesk/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Clear();
            ProductHeaderValue header = new ProductHeaderValue("VCamDeskApp", localAppVersion);
            ProductInfoHeaderValue userAgent = new ProductInfoHeaderValue(header);
            client.DefaultRequestHeaders.UserAgent.Add(userAgent);
            // User-Agent: VCamDeskApp/1.0.0.0


            try
            {
                GithubRelease githubRelease = null;

                // Get the latest release
                // see https://developer.github.com/v3/repos/releases/#list-releases
                githubRelease = await GetGithubReleaseAsync("releases/latest");
                ShowGithubRelease(githubRelease);
                
                if (githubRelease != null) {
                    remoteAppVersion = githubRelease.Tag_name.Substring(1); //omit leading "v"

                    var localVersion = new Version(localAppVersion);
                    var remoteVersion = new Version(remoteAppVersion);

                    var result = localVersion.CompareTo(remoteVersion);
                    if (result > 0) { 
                        Console.WriteLine("Local version is greater, no update available.");
                    }
                    else if (result < 0)
                    {
                        Console.WriteLine("Remote version is greater. Update available!");
                        //notify: update available!
                        parentForm.Invoke(parentForm.showUpdateAvailableDelegate);
                    }
                    else { 
                        Console.WriteLine("Versions are equal, no update available.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}


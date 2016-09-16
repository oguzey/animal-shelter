using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HttpServer
{
    public class Server
    {
        private IPAddress _ip;
        private int _port;
        private Encoding _encoding;

        private string[] _hostnames;
        private string[] _dbhosts;
        private string[] _hazelhosts;

        private HttpListener _http;
        private Database _db;
        private Cache _cache;

        public Server(IPAddress ip, int port)
        {
            _http = new HttpListener();
            _ip = ip;
            _encoding = Encoding.UTF8;

            LoadConfig();
            _port = port;//if specified

            FillPrefixes();

            _db = new Database(_dbhosts);
            _cache = new Cache(_hazelhosts);
        }

        public void Start()
        {
            _http.Start();

            Task.Run(() =>
            {
                while (_http.IsListening)
                {
                    try
                    {
                        var context = _http.GetContext();
                        var req = context.Request;
                        var res = context.Response;

                        Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {req.HttpMethod}: {req.RawUrl} from {req.RemoteEndPoint.Address}");

                        if (req.HttpMethod == "GET")
                        {
                            switch (req.RawUrl)
                            {
                                case "/data":
                                    SendJson(res, JsonConvert.SerializeObject(new Animal() { Id = Guid.NewGuid(), Name = "Vaska", Type = "Cat", Age = 0.3f }));
                                    break;
                                case "/all":
                                    SendJson(res, JsonConvert.SerializeObject(_db.GetAllAnimalsAsync().Result));
                                    break;
                                default:
                                    SendHtml(res, "Hello. Make GET /data to get json");
                                    break;
                            }
                        }
                        else if (req.HttpMethod == "POST")
                        {
                            var buffer = new byte[req.ContentLength64];
                            req.InputStream.ReadAsync(buffer, 0, buffer.Length);
                            var incoming = _encoding.GetString(buffer);
                            Console.WriteLine($"Received data: {incoming}");

                            switch (req.RawUrl)
                            {
                                case "/me":
                                    {
                                        var token = JsonConvert.DeserializeObject<int>(incoming);
                                        var person = _cache.GetPersonAsync(token).Result;
                                        if (person == null)
                                            SendHtml(res, "We don't know you");
                                        else
                                        {
                                            var animals = _db.GetTakenAnimalsByAsync(person);
                                            SendJson(res, JsonConvert.SerializeObject(animals.Result));
                                        }
                                    }
                                    break;
                                case "/mebyemail":
                                    {
                                        var email = JsonConvert.DeserializeObject<string>(incoming);
                                        var token = email.GetHashCode();
                                        var person = _cache.GetPersonAsync(token).Result;
                                        if (person == null)
                                            SendHtml(res, "We don't know you");
                                        else
                                        {
                                            var animals = _db.GetTakenAnimalsByAsync(person);
                                            SendJson(res, JsonConvert.SerializeObject(animals.Result));
                                        }
                                    }
                                    break;
                                case "/add":
                                    {
                                        var animal = JsonConvert.DeserializeObject<Animal>(incoming);
                                        if (animal == null)
                                            SendHtml(res, "Error: couldn't deserialize given Animal");
                                        else
                                        {
                                            _db.AddAnimalAsync(animal);
                                            SendHtml(res, "Animal added successfully");
                                        }
                                    }
                                    break;
                                case "/request":
                                    {
                                        var person = JsonConvert.DeserializeObject<Person>(incoming);
                                        var token = person.Email.GetHashCode();
                                        _cache.AddPersonAsync(token, person);
                                        _db.TakeAnimalAsync(person);
                                        SendJson(res, JsonConvert.SerializeObject(token));
                                    }
                                    break;
                                default:
                                    SendHtml(res, "Hello. Make GET /data to get json");
                                    break;
                            }
                        }
                        //SendHtml(res, $"Congratulations, {person.Name}! Pet with ID#{person.PetID} is ordered by you!");                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            });
            Console.WriteLine($"\nServer is listening to http://{_ip}:{_port}...");
        }

        private void FillPrefixes()
        {
            _http.Prefixes.Add($"http://{_ip}:{_port}/");//for testing
            for (var i = 0; i < _hostnames.Length; i++)
                _http.Prefixes.Add($"http://{_hostnames[i]}:{_port}/");

            Console.WriteLine("Resolving hosts:");
            foreach (var prefix in _http.Prefixes)
                Console.WriteLine(prefix);
        }

        private void LoadConfig()
        {
            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(File.ReadAllText("Server.config"));

                _port = int.Parse(xml.SelectNodes("/configuration/ports").Item(0).ChildNodes[0].Attributes["default"].Value);

                var hostnames = xml.SelectNodes("/configuration/hostnames").Item(0).ChildNodes;
                _hostnames = new string[hostnames.Count];
                for (var i = 0; i < hostnames.Count; i++)
                    _hostnames[i] = hostnames[i].Attributes["value"].Value;

                var dbhosts = xml.SelectNodes("/configuration/cassandra").Item(0).ChildNodes;
                _dbhosts = new string[dbhosts.Count];
                for (var i = 0; i < dbhosts.Count; i++)
                    _dbhosts[i] = dbhosts[i].Attributes["ip"].Value;

                var hazelhosts = xml.SelectNodes("/configuration/hazelcast").Item(0).ChildNodes;
                _hazelhosts = new string[hazelhosts.Count];
                for (var i = 0; i < hazelhosts.Count; i++)
                    _hazelhosts[i] = hazelhosts[i].Attributes["ip"].Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }        

        private void SendJson(HttpListenerResponse response, object jsonobj)
        {
            try
            {
                var json = JsonConvert.SerializeObject(jsonobj);
                var buffer = _encoding.GetBytes(json);

                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                response.ContentType = "application/json; charset=utf-8";
                response.ContentLength64 = buffer.Length;
                using (var stream = response.OutputStream)
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        private void SendHtml(HttpListenerResponse response, string str)
        {
            try
            {
                var buffer = _encoding.GetBytes(str);

                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                response.ContentType = "text/html; charset=utf-8";
                response.ContentLength64 = buffer.Length;

                using (var stream = response.OutputStream)
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        private void SendHtml(HttpListenerResponse response, byte[] arr)
        {
            try
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                response.ContentType = "text/html; charset=utf-8";
                response.ContentLength64 = arr.Length;

                using (var stream = response.OutputStream)
                {
                    stream.Write(arr, 0, arr.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
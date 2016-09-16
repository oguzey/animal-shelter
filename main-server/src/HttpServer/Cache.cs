using Hazelcast.Client;
using Hazelcast.Config;
using Hazelcast.Core;
using System;
using System.Threading.Tasks;

namespace HttpServer
{
    class Cache
    {
        private IHazelcastInstance _hazelcast;
        private IMap<int, Person> _map;

        public Cache(string[] hosts)
        {
            try
            {
                var clientConfig = new ClientConfig();
                clientConfig.GetNetworkConfig().AddAddress(hosts);
                clientConfig.GetSerializationConfig().
                    AddPortableFactory(PersonPortableFactory.FACTORY_ID, new PersonPortableFactory());

                _hazelcast = HazelcastClient.NewHazelcastClient(clientConfig);
                _map = _hazelcast.GetMap<int, Person>("persons");

                Console.WriteLine("Hazelcast cluster addresses:");
                foreach (var h in hosts)
                    Console.WriteLine(h);
            }
            catch (Exception e)
            {
                Console.WriteLine("Hazelcast: connection failed.");
                Console.WriteLine(e.Message);
            }
        }

        public async void AddPersonAsync(int token, Person person)
        {
            try
            {
                await _map.PutAsync(token, person);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<Person> GetPersonAsync(int token)
        {
            try
            {
                if (_map.ContainsKey(token))
                    return await _map.GetAsync(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
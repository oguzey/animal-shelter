using Cassandra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HttpServer
{
    class Database
    {
        private Cluster _cluster;

        public Database(string[] hosts)
        {
            try
            {
                _cluster = new Builder().AddContactPoints(hosts).Build();

                Console.WriteLine("Cassandra cluster contact points:");
                foreach (var dbhost in hosts)
                    Console.WriteLine(dbhost);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cassandra: connection failed.");
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            try
            {
                using (var session = _cluster.Connect("shelter"))
                {
                    var statement = new SimpleStatement("select * from animals");
                    var rows = await session.ExecuteAsync(statement);

                    var animals = new List<Animal>();
                    foreach (var row in rows)
                        animals.Add(new Animal()
                        {
                            Id = (Guid)row["id"],
                            Type = (string)row["type"],
                            Name = (string)row["name"],
                            Age = (float)row["age"],
                            Image = (string)row["image"],
                            Gender = (string)row["gender"],
                            Phone = (string)row["phone"],
                            Status = (bool)row["status"]
                        });

                    return animals.FindAll(x => x.Status);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<Animal>();
        }

        public async void AddAnimalAsync(Animal animal)
        {
            try
            {
                using (var session = _cluster.Connect("shelter"))
                {
                    var statement = new SimpleStatement($"insert into animals (id, name, type, age, gender, phone, image, status) values ({Guid.NewGuid()}, '{animal.Name}', '{animal.Type}', {animal.Age}, '{animal.Gender}', '{animal.Phone}', '{animal.Image}', true);");
                    await session.ExecuteAsync(statement);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async void TakeAnimalAsync(Person owner)
        {
            try
            {
                using (var session = _cluster.Connect("shelter"))
                {
                    var statement = new SimpleStatement($"insert into orders (id, name, address, email, phone, pet_id) values ({Guid.NewGuid()}, '{owner.Name}', '{owner.Address}', '{owner.Email}', '{owner.Phone}', {owner.PetID});");
                    await session.ExecuteAsync(statement);
                    statement = new SimpleStatement($"update animals set status = false where id = {owner.PetID};");
                    await session.ExecuteAsync(statement);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<Animal>> GetTakenAnimalsByAsync(Person person)
        {
            try
            {
                var animals = new List<Animal>();
                using (var session = _cluster.Connect("shelter"))
                {
                    var statement = new SimpleStatement($"select pet_id from orders where email = '{person.Email}';");
                    var pet_id_rows = await session.ExecuteAsync(statement);

                    var p = session.Prepare($"select * from animals where id = ?;");
                    foreach (var pet_id_row in pet_id_rows)
                    {
                        var rows = await session.ExecuteAsync(p.Bind((Guid)pet_id_row["pet_id"]));
                        foreach (var row in rows)
                        {
                            animals.Add(new Animal()
                            {
                                Id = (Guid)row["id"],
                                Type = (string)row["type"],
                                Name = (string)row["name"],
                                Age = (float)row["age"],
                                Image = (string)row["image"],
                                Gender = (string)row["gender"],
                                Phone = (string)row["phone"],
                                Status = (bool)row["status"]
                            });
                        }
                    }
                }
                return animals;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<Animal>();
        }
    }
}
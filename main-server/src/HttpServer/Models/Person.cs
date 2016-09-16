using Hazelcast.IO.Serialization;
using System;

namespace HttpServer
{
    public class PersonPortableFactory : IPortableFactory
    {
        public const int FACTORY_ID = 1;
        public IPortable Create(int classId)
        {
            if (Person.ID == classId)
                return new Person();
            else
                return null;
        }
    }

    public class Person : IPortable
    {
        public const int ID = 5;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Guid PetID { get; set; }

        public int GetClassId()
        {
            return ID;
        }

        public int GetFactoryId()
        {
            return PersonPortableFactory.FACTORY_ID;
        }

        public void ReadPortable(IPortableReader reader)
        {
            try
            {
                Id = new Guid(reader.ReadByteArray("id"));
                Name = reader.ReadUTF("name");
                Email = reader.ReadUTF("email");
                Phone = reader.ReadUTF("phone");
                Address = reader.ReadUTF("address");
                PetID = new Guid(reader.ReadByteArray("pet_id"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Hazelcast: read failed.");
                Console.WriteLine(e.Message);
            }
        }

        public void WritePortable(IPortableWriter writer)
        {
            try
            {
                writer.WriteByteArray("id", Id.ToByteArray());
                writer.WriteUTF("name", Name);
                writer.WriteUTF("email", Email);
                writer.WriteUTF("phone", Phone);
                writer.WriteUTF("address", Address);
                writer.WriteByteArray("pet_id", PetID.ToByteArray());
            }
            catch (Exception e)
            {
                Console.WriteLine("Hazelcast: write failed.");
                Console.WriteLine(e.Message);
            }
        }
    }
}
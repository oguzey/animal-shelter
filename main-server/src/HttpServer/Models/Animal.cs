using System;

namespace HttpServer
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public float Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
    }
}
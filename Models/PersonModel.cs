using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI.Models
{
    public class PersonModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid? Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public List<HotelModel>? Bookings { get; set; } 
    }
}

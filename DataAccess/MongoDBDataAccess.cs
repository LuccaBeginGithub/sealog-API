using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

using WebAPI.Models;


namespace WebAPI.DataAccess
{
    public class MongoDBDataAccess
    {
        private readonly IMongoCollection<PersonModel> _personCollection;

        public MongoDBDataAccess(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _personCollection = database.GetCollection<PersonModel>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<PersonModel>> GetAsync() { 
            return await _personCollection.Find(new BsonDocument()).ToListAsync();
        }

       public async Task<PersonModel> GetPersonByEmail(string email)
        {
            FilterDefinition<PersonModel> filter = Builders<PersonModel>.Filter.Eq("EmailAddress", email);
            return await _personCollection.Find(filter).FirstOrDefaultAsync(); 
        }
        public async Task CreateAsync(PersonModel personModel) {
            await _personCollection.InsertOneAsync(personModel);
            return;

        }
        public async Task AddBooking(string email, HotelModel hotel) {
            FilterDefinition<PersonModel> filter = Builders<PersonModel>.Filter.Eq("EmailAddress", email);
            UpdateDefinition<PersonModel> update = Builders<PersonModel>.Update.Push("Bookings", hotel);
            await _personCollection.UpdateOneAsync(filter, update);
            return;
        }
        public async Task DeleteAsync(string id) {
            FilterDefinition<PersonModel> filter = Builders<PersonModel>.Filter.Eq("Id", id);
            await _personCollection.DeleteOneAsync(filter);
            return;
        }
        public async Task<bool> EmailExistsAsync(string emailAddress)
        {
            // Check if there is any user with the given email address
            var filter = Builders<PersonModel>.Filter.Eq(x => x.EmailAddress, emailAddress);
            var count = await _personCollection.CountDocumentsAsync(filter);

            return count > 0;
        }
    }
}

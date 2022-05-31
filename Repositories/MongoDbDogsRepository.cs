using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DogsCatalog.Entities;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

namespace DogsCatalog.Repositories
{

    public class MongoDbDogsRepository : IDogsRepository
    {
        private const string databaseName = "dogscatalog";
        private const string collectionName = "dogs";
        private readonly IMongoCollection<Dog> dogsCollection;
        private readonly FilterDefinitionBuilder<Dog> filterBuilder = Builders<Dog>.Filter;
        public MongoDbDogsRepository(IMongoClient mongoClient)
        {
            //изменения лев тут
            
            //end
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            dogsCollection = database.GetCollection<Dog>(collectionName);
        }
        public async Task CreateDogsAsync(Dog dog)
        {
            await dogsCollection.InsertOneAsync(dog);
        }

        public async Task DeleteDogsAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(dog => dog.Id, Id);
            await dogsCollection.DeleteOneAsync(filter);
        }

        public async Task<Dog> GetDogAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(dog => dog.Id, Id);
            return await dogsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task< IEnumerable<Dog>> GetDogsAsync()
        {
            return await dogsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateDogsAsync(Dog dog)
        {
            var filter = filterBuilder.Eq(existingDog => existingDog.Id, dog.Id);
            await dogsCollection.ReplaceOneAsync(filter,dog);

        }
    }
}
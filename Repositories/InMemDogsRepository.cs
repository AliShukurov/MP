using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogsCatalog.Entities;


namespace DogsCatalog.Repositories
{
    public class InMemDogsRepository : IDogsRepository
    {
        private readonly List<Dog> Dogs = new()
        {
            new Dog {Id = Guid.NewGuid(),Model = "T-90", Price = 9000, CreatedDate = DateTimeOffset.UtcNow},
            new Dog {Id = Guid.NewGuid(),Model = "T-80", Price = 8000, CreatedDate = DateTimeOffset.UtcNow},
            new Dog {Id = Guid.NewGuid(),Model = "T-64", Price = 1000, CreatedDate = DateTimeOffset.UtcNow}
        };

        public async Task<IEnumerable<Dog>> GetDogsAsync()
        {
            return await Task.FromResult(Dogs);
        }

        public async Task<Dog?> GetDogAsync(Guid id)
        {
            var Dog = Dogs.Where(Dog => Dog.Id == id).SingleOrDefault();
            return await Task.FromResult(Dog);
        }

        public async Task CreateDogsAsync(Dog Dog)
        {
            Dogs.Add(Dog);
            await Task.CompletedTask;
        }

        public async Task UpdateDogsAsync(Dog Dog)
        {
            var index = Dogs.FindIndex(existingDog => existingDog.Id == Dog.Id);
            Dogs[index] = Dog;
            await Task.CompletedTask;
        }

        public async Task DeleteDogsAsync(Guid Id)
        {
            var index = Dogs.FindIndex(existingDog => existingDog.Id == Id);
            Dogs.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DogsCatalog.Entities;


namespace DogsCatalog.Repositories
{
    public interface IDogsRepository{
        Task<Dog> GetDogAsync(Guid Id);
        Task<IEnumerable<Dog>> GetDogsAsync();

        Task CreateDogsAsync(Dog dog);

        Task UpdateDogsAsync(Dog dog);

        Task DeleteDogsAsync(Guid Id);
    }
}
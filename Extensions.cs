using DogsCatalog.Dtos;
using DogsCatalog.Entities;

namespace DogsCatalog
{
    public static class Extensions
    {
        public static DogDto AsDto(this Dog dog)
        {
            return new DogDto
            {
                Id = dog.Id,
                Model = dog.Model,
                Price = dog.Price,
                CreatedDate = dog.CreatedDate,
                DogsSpeed = dog.DogsSpeed,
                FoodGauge = dog.FoodGauge
            };
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DogsCatalog.Repositories;
using DogsCatalog.Entities;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using DogsCatalog.Dtos;

namespace DogsCatalog.Controllers
{
    //GET /dog
    [ApiController]
    [Route("[controller]")]
    public class CatsController : ControllerBase
    {
        private readonly IDogsRepository repositry;

        public CatsController(IDogsRepository repository)
        {
            this.repositry = repository;
        }

        // GET /dog
        [HttpGet]
        public async Task<IEnumerable<DogDto>> GetDogsAsync()
        {
            var dogs = (await repositry.GetDogsAsync()).Select(dog => dog.AsDto());
            return dogs;
        }

        // GET / dogs/{id}
        [HttpGet("{Id}")]
        public async Task<ActionResult<DogDto>> GetDogAsync(Guid Id)
        {
            var dog = await repositry.GetDogAsync(Id);
            if (dog is null)
            {
                return NotFound();
            }
            return dog.AsDto();
        }

        //POST /items
        [HttpPost]
        public async Task<ActionResult<DogDto>> CreateDogAsync(CreateDog dogDto)
        {
            dog dog = new()
            {
                Id = Guid.NewGuid(),
                Model = dogDto.Model,
                Price = dogDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
                DogsSpeed = dogDto.DogsSpeed,
                FoodGauge = dogDto.FoodGauge
            };

            await repositry.CreateDogAsync(dog);

            return CreatedAtAction(nameof(GetDogAsync), new { id = dog.Id }, dog.AsDto());
        }

        //PUT /dogs/{id}
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateDogAsync(Guid Id, UpdateDogDto dogDto)
        {
            var existingDog = await repositry.GetDogAsync(Id);

            if (existingDog is null)
            {
                return NotFound();
            }

            dog updatedDog = existingDog with
            {
                Model = dogDto.Model,
                Price = dogDto.Price,
                DogsSpeed = dogDto.DogsSpeed,
                FoodGauge = dogDto.FoodGauge
            };

            await repositry.UpdateDogsAsync(updatedDog);

            return NoContent();
        }

        //DELETE /dog/{id}
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteDogAsync(Guid Id)
        {
            var existingDog = await repositry.GetDogAsync(Id);

            if (existingDog is null)
            {
                return NotFound();
            }

            await repositry.DeleteDogAsync(Id);

            return NoContent();
        }

    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace DogsCatalog.Dtos
{
    public record CreateDog
    {
        [Required]
        public string? Model { get; init; }
        [Required]
        [Range(1,10000000)]
        public decimal Price { get; init; }
        [Required]
        [Range(1,100000)]
        public decimal DogsSpeed {get; init;}
        [Required]
        [Range(30,255)]
        public decimal FoodGauge {get;init;}
    }
}
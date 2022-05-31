namespace DogsCatalog.Entities
{
    public record Dog
    {
        public Guid Id { get; init; }
        public string? Model { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
        public decimal DogsSpeed {get; init;}
        public decimal FoodGauge {get;init;}
    }
}
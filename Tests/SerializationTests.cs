using System.Text.Json;
using System.Text.Json.Serialization;
using Wivuu.JsonPolymorphism;
using Xunit;

namespace Tests
{
    enum AnimalType
    {
        Insect,
        Mammal,
        Reptile,
    }

    enum MammalSpecies
    {
        Dog,
        Cat,
        Monkey,
    }

    abstract partial record Animal( [JsonDiscriminator] AnimalType type, string Name );

    // Animals
    record Insect(int NumLegs = 6, int NumEyes=4) : Animal(AnimalType.Insect, "Insectoid");
    partial record Mammal([JsonDiscriminator] MammalSpecies species, int NumNipples = 2) : Animal(AnimalType.Mammal, "Mammalian");
    record Reptile(bool ColdBlooded = true) : Animal(AnimalType.Reptile, "Reptilian");

    // Mammals
    record Dog() : Mammal(MammalSpecies.Dog, NumNipples: 8);
    record Cat() : Mammal(MammalSpecies.Cat, NumNipples: 8);
    record Monkey() : Mammal(MammalSpecies.Monkey, NumNipples: 2);

    public class SerializationTests
    {
        [Fact]
        public void TestEnumCamelCaseSerialization()
        {
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            options.Converters.Add(new JsonStringEnumConverter());
            
            Animal[] animals = 
            {
                new Insect(NumLegs: 8, NumEyes: 6),
                new Dog(),
                new Reptile(ColdBlooded: false),
            };

            var serialized          = JsonSerializer.Serialize(animals, options);
            var animalsDeserialized = JsonSerializer.Deserialize<Animal[]>(serialized, options);
            
            if (animalsDeserialized is null)
                throw new System.Exception("Unable to deserialize");

            Assert.Equal(animals.Length, animalsDeserialized.Length);

            for (var i = 0; i < animals.Length; ++i)
                Assert.Equal(animals[i], animalsDeserialized[i]);
        }
        
        [Fact]
        public void TestEnumPascalSerialization()
        {
            JsonSerializerOptions options = new();
            options.Converters.Add(new JsonStringEnumConverter());
            
            Animal[] animals = 
            {
                new Insect(NumLegs: 8, NumEyes: 6),
                new Dog(),
                new Reptile(ColdBlooded: false),
            };

            var serialized          = JsonSerializer.Serialize(animals, options);
            var animalsDeserialized = JsonSerializer.Deserialize<Animal[]>(serialized, options);
            
            if (animalsDeserialized is null)
                throw new System.Exception("Unable to deserialize");
                
            Assert.Equal(animals.Length, animalsDeserialized.Length);

            for (var i = 0; i < animals.Length; ++i)
                Assert.Equal(animals[i], animalsDeserialized[i]);
        }
        
        [Fact]
        public void TestIntSerialization()
        {
            JsonSerializerOptions options = new();
            
            Animal[] animals = 
            {
                new Insect(NumLegs: 8, NumEyes: 6),
                new Dog(),
                new Reptile(ColdBlooded: false),
            };

            var serialized          = JsonSerializer.Serialize(animals, options);
            var animalsDeserialized = JsonSerializer.Deserialize<Animal[]>(serialized, options);
            
            if (animalsDeserialized is null)
                throw new System.Exception("Unable to deserialize");
                
            Assert.Equal(animals.Length, animalsDeserialized.Length);

            for (var i = 0; i < animals.Length; ++i)
                Assert.Equal(animals[i], animalsDeserialized[i]);
        }
    }
}
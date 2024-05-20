using Xunit;

namespace TestProject2;

public class UnitTest1 : IClassFixture<CarDataFixture>
{
    private readonly CarDataFixture p_CarDataFixture;

    public UnitTest1(CarDataFixture carDataFixture)
    {
        p_CarDataFixture = carDataFixture;

        TagHelper.Register<int>("Properties", ExtractProperties);
        TagHelper.Register<int>("Property", ExtractProperty);
    }

    string ExtractProperties(TagHelper.TagKey tagKey, int id)
    {
        var car = p_CarDataFixture.Find(id) ?? throw new ArgumentNullException(id.ToString());

        return string.Join(";", car.Properties.Select(a => a.ToString()));
    }

    string ExtractProperty(TagHelper.TagKey tagKey, int id)
    {
        var car = p_CarDataFixture.Find(id) ?? throw new ArgumentNullException(id.ToString());

        if (tagKey.Index != null)
        {
            var property = car.Properties[tagKey.Index.Value];

            if (!string.IsNullOrWhiteSpace(tagKey.PropertyName))
            {
                var propertyInfo = typeof(CarProperty).GetProperty(tagKey.PropertyName);

                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(property)?.ToString() ?? string.Empty;
                }
            }

            return property.ToString();
        }
        else if (!string.IsNullOrWhiteSpace(tagKey.IndexPropertyName) 
            && !string.IsNullOrWhiteSpace(tagKey.IndexPropertyValue))
        {
            var property = car.Properties.FirstOrDefault(p =>
                p.GetType().GetProperty(tagKey.IndexPropertyName)?.GetValue(p)?.ToString() == tagKey.IndexPropertyValue);

            if (property != null)
            {
                if (string.IsNullOrWhiteSpace(tagKey.PropertyName))
                {
                    return property.ToString();
                }

                return property.GetType().GetProperty(tagKey.PropertyName)?.GetValue(property)?.ToString() ?? string.Empty;
            }
        }

        return string.Empty;
    }

    [Theory]
    [InlineData("Properties", "Wheel:Round;Paint:Dry")]
    [InlineData("Property[0]", "Wheel:Round")]
    [InlineData("Property[0].Name", "Wheel")]
    [InlineData("Property[0].Description", "Round")]
    [InlineData("Property[1].Name", "Paint")]
    [InlineData("Property[Name=Wheel]", "Wheel:Round")]
    [InlineData("Property[Name=Wheel].Description", "Round")]
    public void Test1(string key, string expectedResult)
    {
        var result = TagHelper.RunTag(key, 1);

        Assert.Equal(expectedResult, result);
    }
}

public class CarDataFixture
{
    private List<Car> TempData = new List<Car>();

    public CarDataFixture()
    {
        TempData.Add(new Car
        {
            ID = 1,
            Name = "BR-V",
            Properties = 
            [
                new CarProperty("Wheel", "Round"), 
                new CarProperty("Paint", "Dry")
            ]
        });
    }

    public Car? Find(int id)
    {
        return TempData.SingleOrDefault(a => a.ID == id);
    }
}


public class Car
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;

    public CarProperty[] Properties { get; set; } = Array.Empty<CarProperty>();
}

public class CarProperty
{
    public CarProperty(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string? Description { get; set; }

    public override string ToString()
    {
        return $"{Name}:{Description}";
    }
}
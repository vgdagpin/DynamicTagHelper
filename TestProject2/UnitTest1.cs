using Xunit;

namespace TestProject2;

public class UnitTest1 : IClassFixture<CarDataFixture>
{
    private readonly CarDataFixture p_CarDataFixture;

    public UnitTest1(CarDataFixture carDataFixture)
    {
        p_CarDataFixture = carDataFixture;

        TagHelper.Register<int>("Properties", ExtractTags);
        TagHelper.Register<int>("Property", ExtractTag);
    }

    string ExtractTags(TagHelper.TagKey tagKey, int id)
    {
        var car = p_CarDataFixture.Find(id);

        return string.Empty;
    }

    string ExtractTag(TagHelper.TagKey tagKey, int id)
    {
        var car = p_CarDataFixture.Find(id);

        return string.Empty;
    }

    [Theory]
    [InlineData("Properties", "")]
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
        TempData.Add(new Car { ID = 1, Name = "BR-V", Properties = new List<CarProperty> { new CarProperty("WheelColor", "Black") } });
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

    public IEnumerable<CarProperty> Properties { get; set; } = null!;
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
}
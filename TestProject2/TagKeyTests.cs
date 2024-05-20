using System.Text.RegularExpressions;

namespace TestProject2;

public class MyTagKey
{
    public string Key { get; set; }
    public int? Index { get; set; }
    public string? PropertyName { get; set; }
    public string? PropertyValue { get; set; }

    public MyTagKey(string key)
    {
        Key = key;
    }

    public static MyTagKey? Parse(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        var myTagKey = new MyTagKey(key);


        var propertyVal = Regex.Match(key, @"^(?<key>\w+)(\[(?<property>\w+)=(?<propertyValue>\w+)\])$");

        if (propertyVal.Success)
        {
            myTagKey.Key = propertyVal.Groups["key"].Value;
            myTagKey.Index = null;
            myTagKey.PropertyName = NullIfEmpty(propertyVal.Groups["property"].Value);
            myTagKey.PropertyValue = propertyVal.Groups["propertyValue"].Value;
        }
        else
        {
            var indexProp = Regex.Match(key, @"^(?<key>\w+)(\[(?<index>\d+)\])\.(?<property>\w+)");

            if (indexProp.Success)
            {
                myTagKey.Key = indexProp.Groups["key"].Value;
                myTagKey.Index = int.Parse(indexProp.Groups["index"].Value);
                myTagKey.PropertyName = NullIfEmpty(indexProp.Groups["property"].Value);
                myTagKey.PropertyValue = null;
            }
            else
            {
                var index = Regex.Match(key, @"^(?<key>\w+)(\[(?<index>\d+)\])$");

                if (index.Success)
                {
                    myTagKey.Key = index.Groups["key"].Value;
                    myTagKey.Index = int.Parse(index.Groups["index"].Value);
                    myTagKey.PropertyName = null;
                    myTagKey.PropertyValue = null;
                }
            }
        }

        string? NullIfEmpty(string? data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            return data;
        }

        return myTagKey;
    }
}

public class TagKeyTests
{
    [Fact]
    public void ParseKey()
    {
        var result = MyTagKey.Parse("Tag");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Null(result.Index);
        Assert.Null(result.PropertyName);
        Assert.Null(result.PropertyValue);
    }

    [Fact]
    public void ParseKeyNull()
    {
        var result = MyTagKey.Parse(null);

        Assert.Null(result);
    }

    [Fact]
    public void ParseKeyEmpty()
    {
        var result = MyTagKey.Parse("");

        Assert.Null(result);
    }

    [Fact]
    public void ParseKeyIndex()
    {
        var result = MyTagKey.Parse("Tag[2]");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Equal(2, result.Index);
        Assert.Null(result.PropertyName);
        Assert.Null(result.PropertyValue);
    }

    [Fact]
    public void ParseKeyProperty()
    {
        var result = MyTagKey.Parse("Tag[2].Description");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Equal(2, result.Index);
        Assert.Equal("Description", result.PropertyName);
        Assert.Null(result.PropertyValue);
    }

    [Fact]
    public void ParseKeyPropertyByValue()
    {
        var result = MyTagKey.Parse("Tag[Name=Test]");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Null(result.Index);
        Assert.Equal("Name", result.PropertyName);
        Assert.Equal("Test", result.PropertyValue);
    }
}
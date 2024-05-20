using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestProject1;

public static class TagHelper
{
    public class TagKey
    {
        public string Key { get; set; }
        public int? Index { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyValue { get; set; }

        public TagKey(string key)
        {
            Key = key;
        }

        public static TagKey Parse(string key)
        {
            return new TagKey(key);
        }
    }

    public static string[] GetTags(string input)
    {
        var str = new List<string>();

        foreach (Match match in Regex.Matches(input, @"~([^~]+)~"))
        {
            str.Add(match.Groups[1].Value);
        }

        return str.ToArray();
    }

    public static string[] GetRegisteredTags()
    {
        return data.Keys.Select(a => a).ToArray();
    }

    private static Dictionary<string, object> data = new Dictionary<string, object>();

    public static void Register<T>(string tag, Func<TagKey, T, string> func)
    {
        data[tag] = func;
    }

    public static string? RunTag<T>(string tag, T arg)
    {
        if (data.ContainsKey(tag) && data[tag] is Func<TagKey, T, string> func)
        {
            var tagKey = TagKey.Parse(tag);

            return func(tagKey, arg);
        }

        return null;
    }
}

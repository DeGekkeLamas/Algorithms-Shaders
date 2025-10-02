using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

static public class StringTools
{
    static public string StrikeThrough(string str)
    {
        return $"<s>{str}</s>";
    }
    static public string StrikeThrough(string full, string part)
    {
        return full.Replace(part, $"<s>{part}</s>");
    }

    static public string Bold(string str)
    {
        return $"<b>{str}</b>";
    }
    static public string Bold(string full, string part)
    {
        return full.Replace(part, $"<b>{part}</b>");
    }

    static public string Color(string str, Color color)
    {
        return $"<color=#{color.ToHexString()}>{str}</color>";
    }
    static public string Color(string full, string part, Color color)
    {
        return full.Replace(part, $"<color=#{color.ToHexString()}>{part}</color>");
    }

    static public string CamelcaseToRegular(string str)
    {
        List<int> capitalIndexes = GetCapitalIndexes(str);

        for (int i = capitalIndexes.Count-1; i >= 0; i--)
        {
            if (capitalIndexes[i] == 0) continue;

            char c = str[ capitalIndexes[i] ];
            str = str.Replace($"{c}", $" {char.ToLower(c)}");
        }

        return str;
    }

    static public List<int> GetCapitalIndexes(string str)
    {
        List<int> capitalIndexes = new ();
        for(int i = 0; i<str.Length; i++)
        {
            if (char.IsUpper(str[i])) capitalIndexes.Add(i);
        }
        return capitalIndexes;
    }
}

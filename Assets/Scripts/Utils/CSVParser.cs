using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSVParser
{
    static readonly string SPLIT_CH = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static readonly string LINE_SPLIT_CH = @"\r\n|\n\nr|\n|\r";
    static readonly char[] TRIM_CH = { '\"' };
    public static List<Dictionary<string, object>> Read(string fileName)
    {
        var returnData = new List<Dictionary<string, object>>();

        string fileData = TextFileReader.ReadFileAsText(fileName);

        var linesOfData = Regex.Split(fileData, LINE_SPLIT_CH);

        // if csv has only the header or nothing at all, return the empty list
        if (linesOfData.Length <= 1) return returnData;

        var header = Regex.Split(linesOfData[0], SPLIT_CH);
        for (var i = 1; i < linesOfData.Length; i++)
        {
            var values = Regex.Split(linesOfData[i], SPLIT_CH);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>(); // this entry is the single line of data, with key (header name) and value pairs
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CH).TrimEnd(TRIM_CH).Replace("\\", "");
                object finalValue = value;
                if (int.TryParse(value, out int n))
                {
                    finalValue = n;
                }
                else if (float.TryParse(value, out float f))
                {
                    finalValue = f;
                }

                entry[header[j]] = finalValue;
            }

            returnData.Add(entry);
        } 

        return returnData;
    }
}

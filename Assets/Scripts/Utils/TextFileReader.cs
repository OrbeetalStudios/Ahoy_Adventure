using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public static class TextFileReader
{
    public static string ReadFileAsText(string fileName)
    {
        TextAsset fileData = Resources.Load(fileName) as TextAsset;

        return fileData.text;
    }
}

using UnityEngine;

public class SavedDataManager
{
    public enum ESavedDataType
    {
        HighScore,
        DoubloonAmount
    }
    private static readonly int defaultValue = 0;

    static public int ReadInt(ESavedDataType dataType)
    {
        return PlayerPrefs.GetInt(DataTypeToString(dataType), defaultValue);
    }
    static public void WriteInt(ESavedDataType dataType, int _value)
    {
        PlayerPrefs.SetInt(DataTypeToString(dataType), _value);
    }
    static private string DataTypeToString(ESavedDataType dataType)
    {
        string res = "";
        switch (dataType)
        {
            case ESavedDataType.HighScore:
                res = "HighScore";
                break;
            case ESavedDataType.DoubloonAmount:
                res = "DoubloonAmount";
                break;
            default:
                break;
        }

        return res;
    }
}
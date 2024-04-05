using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class WavesController : MonoBehaviour
{
    [SerializeField] private string filenameCsv;
    [SerializeField] private string filenameJson;
    private List<Dictionary<string, object>> wavesDataCsv = new();
    private JSONWaves wavesDataJson = new();

    // Start is called before the first frame update
    void Start()
    {
        // CSV version
        wavesDataCsv = CSVParser.Read(filenameCsv);

        //JSON version
        wavesDataJson = JSONWaves.CreateFromJSON(TextFileReader.ReadFileAsText(filenameJson));
    }
}

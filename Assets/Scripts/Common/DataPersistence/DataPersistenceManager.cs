using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPeristence> dataPeristenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance!=null)
        {
            Debug.Log("Found more than one DataPersistenceManager in scene");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
      
    }

    public void NewGame()
    {
        this.gameData=new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data was found. newgame set");
            NewGame();
            return;
        }

        foreach (IDataPeristence dataPeristenceObj in dataPeristenceObjects)
        {
            dataPeristenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            return;
        }
        foreach (IDataPeristence dataPeristenceObj in dataPeristenceObjects)
        {
            dataPeristenceObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    public void NowLoad()
    {
        this.dataPeristenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }


    public void NowSave()
    {
        this.dataPeristenceObjects = FindAllDataPersistenceObjects();
        SaveGame();
    }


  

    private List<IDataPeristence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPeristence> dataPeristenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPeristence>();
        return new List<IDataPeristence>(dataPeristenceObjects);
    }

}

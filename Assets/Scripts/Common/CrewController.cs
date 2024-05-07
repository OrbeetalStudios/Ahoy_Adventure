using System.Collections.Generic;

public class CrewController : MonoSingleton<CrewController>, IDataPeristence
{
    public List<CrewData> crewData;
    public List<CrewData> assignedCrew = new List<CrewData>();
    public List<int> idPurchased = new List<int>();
    public List<int> idAssigned = new List<int>();
    public int doubloons;

    private void Start()
    {
        DataPersistenceManager.instance.NowLoad();
    }
    public void SetCrewData()
    {
        int index;
        assignedCrew.Clear();
        foreach (int id in idAssigned)
        {
           index = crewData.FindIndex(x => x.characterID == id);
           if (index!=-1)
           {
                assignedCrew.Add(crewData[index]);
           }
        }
    }
    public void ActivateCrew()
    {
        foreach (CrewData crew in assignedCrew)
        {
            crew.powerUpData.CollectPowerUp();
        }
    }
    public void LoadData(GameData data)
    {
        idPurchased = data.idPurchased;
        idAssigned = data.idAssigned;
        doubloons = data.doubloons;
    }

    public void SaveData(ref GameData data)
    {
        data.idPurchased = idPurchased;
        data.idAssigned = idAssigned;
        data.doubloons = doubloons;
    }
}

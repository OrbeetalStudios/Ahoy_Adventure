using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public GameObject[] items;

	public void SpawnItem (string itemName) {
        foreach (GameObject g in items)
        {
            if (g.name == itemName)
            {
                Instantiate(g, transform.position, g.transform.rotation);
                break;
            }
        }
	}

    public void ResetScene()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Test2DScene()
    {
        Application.LoadLevel("2DDemoScene");
    }

    public void Test3DScene()
    {
        Application.LoadLevel("DemoScene");
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpPanel : MonoBehaviour, IPowerUpEvent
{
    struct PowerUpUI
    {
        public EPowerUpType id;
        public GameObject goUI;
    }
    private List<PowerUpUI> images = new();

    private void Start()
    {
        // iscriviti a eventlistener per ricevere gli eventi
        EventListener.Instance.AddListener(this.gameObject);

        foreach (var data in PowerUpController.Instance.DataList)
        {
            if (data.IsPermanent) continue;

            GameObject obj = new GameObject(data.ObjectName);
            Image img = obj.AddComponent<Image>();
            img.sprite = data.img;
            obj.GetComponent<RectTransform>().SetParent(this.transform);
            obj.SetActive(false);

            PowerUpUI newItem;
            newItem.id = data.Type;
            newItem.goUI = obj;
            images.Add(newItem);
        }
    }
    private void UpdateElement(EPowerUpType id, bool activate)
    {
        int ind = images.FindIndex(x => x.id == id);
        if (ind != -1)
        {
            images[ind].goUI.SetActive(activate);
        }
    }
    public void OnPowerUpCollected(PowerUpData data)
    {
        if (data.IsPermanent) return;

        UpdateElement(data.Type, true);
    }
    public void OnPowerUpExpired(PowerUpData data)
    {
        UpdateElement(data.Type, false);
    }
}
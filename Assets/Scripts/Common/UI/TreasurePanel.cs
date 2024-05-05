using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class TreasurePanel : BaseSpritePanel
{
    [SerializeField] private Sprite evenCoinsImage;
    [SerializeField] private Sprite oddCoinsImage;

    void Start()
    {
        GameObject objRed = new GameObject("RC");
        Image img = objRed.AddComponent<Image>();
        img.sprite = oddCoinsImage;
        objRed.GetComponent<RectTransform>().SetParent(this.transform);
        objRed.GetComponent<RectTransform>().localScale = spriteScale;
        objRed.SetActive(false);
        
        for (int i = 0; i < Island.Instance.MaxTreasure; i++)
        {
            GameObject obj = new GameObject("GC_" + i.ToString());
            img = obj.AddComponent<Image>();
            img.sprite = evenCoinsImage;
            obj.GetComponent<RectTransform>().SetParent(this.transform);
            obj.GetComponent<RectTransform>().localScale = spriteScale;
            images.Add(obj);
            obj.SetActive(i < Island.Instance.CurrentTreasure);
        }

        images.Add(objRed);

        Timing.RunCoroutine(UpdateTreasureUI().CancelWith(gameObject));
    }
    private IEnumerator<float> UpdateTreasureUI()
    {
        float oldTreasure = Island.Instance.CurrentTreasure;

        while (true)
        {
            if (oldTreasure != Island.Instance.CurrentTreasure)
            {
                for (int i = 0; i < Island.Instance.MaxTreasure; i++)
                {
                    images[i].SetActive(i < Mathf.FloorToInt(Island.Instance.CurrentTreasure));
                }

                images[images.Count - 1].SetActive(Island.Instance.CurrentTreasure % 1 != 0);
            }

            oldTreasure = Island.Instance.CurrentTreasure;

            yield return Timing.WaitForOneFrame;
        }
    }
}
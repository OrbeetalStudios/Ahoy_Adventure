using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class LivesPanel : BaseSpritePanel
{
    [SerializeField] private Sprite lifeImage;

    void Start()
    {
        for (int i = 0; i < GameController.Instance.MaxLives; i++)
        {
            GameObject obj = new GameObject("Life_" + i.ToString());
            Image img = obj.AddComponent<Image>();
            img.sprite = lifeImage;
            obj.GetComponent<RectTransform>().SetParent(this.transform);
            obj.GetComponent<RectTransform>().localScale = spriteScale;
            images.Add(obj);
            obj.SetActive(false);
        }

        Timing.RunCoroutine(UpdateLifeUI().CancelWith(gameObject));
    }
    private IEnumerator<float> UpdateLifeUI()
    {
        int oldValue = GameController.Instance.CurrentLives;

        while (true)
        {
            if (oldValue != GameController.Instance.CurrentLives)
            {
                for (int i = 0; i < GameController.Instance.MaxLives; i++)
                {
                    images[i].SetActive(i < GameController.Instance.CurrentLives);
                }
            }

            oldValue = GameController.Instance.CurrentLives;

            yield return Timing.WaitForOneFrame;
        }
    }
}

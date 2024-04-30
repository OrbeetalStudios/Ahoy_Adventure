using MEC;
using System.Collections.Generic;
using UnityEngine;

public class FollowShipUI : MonoBehaviour
{
    public Transform ship; // Riferimento al GameObject del giocatore
    public Vector3 offset = new Vector3(0f, 1f, 0f); // Offset per posizionare l'UI sopra il giocatore

    private RectTransform uiRectTransform; // Riferimento al RectTransform dell'UI

    void Start()
    {
        // Ottieni il riferimento al RectTransform dell'UI
        uiRectTransform = GetComponent<RectTransform>();
        Timing.RunCoroutine(FollowShip());
    }

    protected IEnumerator<float> FollowShip()
    {
        while (true)
        {
            // Assicurati che il riferimento al giocatore esista e che l'UI RectTransform sia stato ottenuto
            if (ship != null && uiRectTransform != null)
            {
                // Ottieni le coordinate del giocatore nel mondo e trasformale in coordinate dello schermo
                Vector3 shipScreenPosition = Camera.main.WorldToScreenPoint(ship.position + offset);

                // Aggiorna la posizione dell'UI RectTransform in base alle coordinate dello schermo del giocatore
                uiRectTransform.position = shipScreenPosition;
            }

            yield return Timing.WaitForOneFrame;
        }
    }

}

using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShipUI : MonoBehaviour
{
    public Transform player; // Riferimento al GameObject del giocatore
    public Vector3 offset = new Vector3(0f, 1f, 0f); // Offset per posizionare l'UI sopra il giocatore

    private RectTransform uiRectTransform; // Riferimento al RectTransform dell'UI

    void Start()
    {
        // Ottieni il riferimento al RectTransform dell'UI
        uiRectTransform = GetComponent<RectTransform>();
        Timing.RunCoroutine(FollowPlayer());
    }

    protected IEnumerator<float> FollowPlayer()
    {
        while (true)
        {
            // Assicurati che il riferimento al giocatore esista e che l'UI RectTransform sia stato ottenuto
            if (player != null && uiRectTransform != null)
            {
                // Ottieni le coordinate del giocatore nel mondo e trasformale in coordinate dello schermo
                Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(player.position + offset);

                // Aggiorna la posizione dell'UI RectTransform in base alle coordinate dello schermo del giocatore
                uiRectTransform.position = playerScreenPosition;
            }

            yield return Timing.WaitForOneFrame;
        }
    }

}

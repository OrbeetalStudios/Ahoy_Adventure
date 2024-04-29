using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BonusAnimation : MonoBehaviour
{
    public Transform player; // Riferimento al GameObject del giocatore
    public Vector3 offset; // Offset per posizionare l'UI sopra il giocatore
    public float slideDuration = 1.5f;
    public float slideHeight = 50f;
    Vector3 startPosition;

    private void OnEnable() {
        if (player != null)
        {
            Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(player.position + offset);
            startPosition = transform.position = playerScreenPosition;
        }
        StartCoroutine(Slide());
        StartCoroutine(Fade());
    }

    IEnumerator Slide(){
        float timeElapsed = 0f;
        var endPosition = startPosition + Vector3.up * slideHeight;
        while (timeElapsed < slideDuration)
        {
            float t = timeElapsed/slideDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator Fade(){
        for (float i = 1f; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}

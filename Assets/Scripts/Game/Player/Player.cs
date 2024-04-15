using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)]
    private float speed;
    protected float currentSpeed;

    public float Speed { get { return speed; } set { speed = value; } }

    protected void Update()
    {
        currentSpeed = speed;//For Prototype changes in-game
    }

    [HideInInspector]
    protected bool clockwiseMotion = false;

    protected float angle = 0.0f;

    public void InvertDirection() => clockwiseMotion = !clockwiseMotion;
}

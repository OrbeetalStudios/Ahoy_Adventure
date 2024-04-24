using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Player : PlayerMovement, IPowerUpEvent
{
    private PlayerControls controls;
    [SerializeField, Range(0f,30f)] 
    private float reloadCannonTime;
    [SerializeField, Range(0f, 10f)]
    private float defaultFireRatio;
    private float fireRatio;
    [SerializeField, Range(0f, 2f)] 
    private float cannonShotVfxOffset;
    [SerializeField] 
    private int cannonShotSfxIndex;
    private int ammoCount=3;
    private bool canFire = true;
    private float reload;
    private bool isLoading = false;
    private bool invulnerabilityOn = false;

    private void Start()
    {
        base.Start();
        // iscriviti a eventlistener per ricevere gli eventi
        EventListener.Instance.AddListener(this.gameObject);
    }
    private void OnEnable()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Player.Movement.performed += OnMovePerformed;
        GameController.Instance.ImgAmmoDeactivated();
        anim.Play("OnIiland",0);
        reload = reloadCannonTime;
        fireRatio = defaultFireRatio;
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Movement.performed -= OnMovePerformed;     
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") ||
            other.CompareTag("Mine"))
        {
            if (invulnerabilityOn) return;

            // Send message to any listeners
            foreach (GameObject go in EventListener.Instance.listeners)
            {
                ExecuteEvents.Execute<IPlayerEvent>(go, null, (x, y) => x.OnPlayerHit());
            }
        }
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        controls.Player.Fire.performed += ctx => StartFire();
        controls.Player.Pause.performed += ctx => GameController.Instance.Pause();
        // Velocity of Input
        if (inputVector == Vector2.zero)
        {
            // Se il vettore di input � zero, chiama direttamente OnMoveCanceled per fermare il movimento
            Timing.RunCoroutine(DecelerationCo().CancelWith(gameObject));
        }
        else
        {
            // Altrimenti, imposta la direzione del movimento in base all'input
            SetMovementDirection(new Vector3(inputVector.x, 0f, inputVector.y).normalized);
        }
        
    }
    private void StartFire()
    {  
        if (ammoCount > 0 && canFire)
        {
            ammoCount--;
            GameController.Instance.UpdateAmmo(ammoCount);
            if (clockwiseMotion)
            {
                anim.Play("Fire",1);
            }
            else
            {
                anim.Play("NoClocwiseFire",1);
            }
            GameObject bullet = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.bullet);
            if (bullet != null)
            {
                bullet.SetActive(true);
                Timing.RunCoroutine(FireRatio(fireRatio).CancelWith(gameObject));
                if (ammoCount < 3 && !isLoading)
                {
                    Timing.RunCoroutine(LoadingCannon().CancelWith(gameObject));
                }
            }
            GameObject cannonShot = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.cannon_shot);
            PlayCannonShotVFX(gameObject,cannonShot);
            PlaySFX(cannonShotSfxIndex);
        }  
    }
    protected  IEnumerator<float> LoadingCannon()
    {
        isLoading = true;
        while (ammoCount!=3)
        {
            if(ammoCount<=0)
            {
                yield return Timing.WaitForSeconds(reloadCannonTime);
                ammoCount = 3;
                GameController.Instance.UpdateAmmo(ammoCount);
                reloadCannonTime = reload;
                break;
            }
            yield return Timing.WaitForOneFrame;
        }

        isLoading = false;
    }
    protected IEnumerator<float> FireRatio(float fireRatio)
    {
        while (fireRatio > 0)
        {
            canFire = false;
            fireRatio--;
            yield return Timing.WaitForSeconds(1f);
        }
        canFire = true;
    }
    public void OnPowerUpCollected(PowerUpData data)
    {
        switch (data.Type)
        {
            case EPowerUpType.FireRate:
                fireRatio = data.Value;
                break;
            case EPowerUpType.Speed:
                speed = data.Value;
                break;
            case EPowerUpType.Invulnerability:
                invulnerabilityOn = true;
                break;
            default:
                break;
        }
    }
    public void OnPowerUpExpired(PowerUpData data)
    {
        switch (data.Type)
        {
            case EPowerUpType.FireRate:
                fireRatio = defaultFireRatio;
                break;
            case EPowerUpType.Speed:
                speed = defaultSpeed;
                break;
            case EPowerUpType.Invulnerability:
                invulnerabilityOn = false;
                break;
            default:
                break;
        }
    }
    private void PlayCannonShotVFX(GameObject parent, GameObject effect){
        effect.transform.position = parent.transform.TransformPoint(Vector3.back * cannonShotVfxOffset);
        effect.transform.rotation = parent.transform.rotation;
        effect.SetActive(true);
    }

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
}
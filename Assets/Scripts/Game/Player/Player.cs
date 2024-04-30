using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Linq;

public class Player : PlayerMovement, IPowerUpEvent
{
    [SerializeField, Range(0f,30f)] private float reloadCannonTime;
    [SerializeField, Range(0f, 10f)] private float defaultFireRatio;
    [SerializeField, Range(0f, 2f)] private float cannonShotVfxOffset;
    [SerializeField] private int cannonShotSfxIndex;
    [SerializeField] private GameObject defaultSkin;
    [SerializeField] private GameObject fireratePowSkin;
    [SerializeField] private GameObject speedPowSkin;
    [SerializeField] private GameObject invulnerabilityPowSkin;
    [SerializeField] private List<GameObject> skinList;
    private GameObject currentSkin;
    private PlayerControls controls;
    private float fireRatio;
    private int ammoCount=3;
    private bool canFire = true;
    private float reload;
    private bool isLoading = false;
    private bool invulnerabilityOn = false;

    private new void Start()
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
        currentSkin = defaultSkin;
        skinList.Add(defaultSkin);
        ChangeSkin();
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
            PlayAnimation();

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
    private void PlayAnimation()
    {
        if (clockwiseMotion)
        {
            anim.Play("Fire", 1);
        }
        else
        {
            anim.Play("NoClocwiseFire", 1);
        }
    }
    private void StartFire()
    {  
        if (ammoCount > 0 && canFire)
        {
            ammoCount--;
            GameController.Instance.UpdateAmmo(ammoCount);
            PlayAnimation();
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
                CheckAndMoveSkin(fireratePowSkin);
                break;
            case EPowerUpType.Speed:
                speed = data.Value;
                CheckAndMoveSkin(speedPowSkin);
                break;
            case EPowerUpType.Invulnerability:
                invulnerabilityOn = true;
                CheckAndMoveSkin(invulnerabilityPowSkin);
                break;
            default:
                break;
        }
        ChangeSkin();
    }
    public void OnPowerUpExpired(PowerUpData data)
    {
        switch (data.Type)
        {
            case EPowerUpType.FireRate:
                fireRatio = defaultFireRatio;
                skinList.Remove(fireratePowSkin);
                break;
            case EPowerUpType.Speed:
                speed = defaultSpeed;
                skinList.Remove(speedPowSkin);
                break;
            case EPowerUpType.Invulnerability:
                invulnerabilityOn = false;
                skinList.Remove(invulnerabilityPowSkin);
                break;
            default:
                break;
        }
        ChangeSkin();
    }

    private void CheckAndMoveSkin(GameObject skin){
        if(!skinList.Contains(skin)){
            skinList.Add(skin);
        } else {
            skinList.Remove(skin);
            skinList.Add(skin);
        }
    }

    private void ChangeSkin(){
        currentSkin.SetActive(false);
        currentSkin = skinList.Last();
        currentSkin.SetActive(true);
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
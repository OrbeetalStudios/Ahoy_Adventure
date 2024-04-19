using MEC;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : PlayerMovement
{
    private PlayerControls controls;
    [SerializeField, Range(0f,30f)] 
    private float reloadCannonTime;
    [SerializeField, Range(0f, 10f)] 
    private float fireRatio;
    private int ammoCount=3;
    private bool canFire = true;
    private float reload;
    private bool isLoading = false;
   
  

    private void OnEnable()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Player.Movement.performed += OnMovePerformed;
        displayAmmo.SetActive(false);
        anim.Play("OnIiland");
        reload = reloadCannonTime;
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Movement.performed -= OnMovePerformed;
      
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        controls.Player.Fire.performed += ctx => StartFire();
        controls.Player.Pause.performed += ctx => GameController.Instance.Pause();
        // Velocity of Input
        if (inputVector == Vector2.zero)
        {
            // Se il vettore di input è zero, chiama direttamente OnMoveCanceled per fermare il movimento
            Timing.RunCoroutine(DecelerationCo().CancelWith(gameObject));
        }
        else
        {
            // Altrimenti, imposta la direzione del movimento in base all'input
            SetMovementDirection(new Vector3(inputVector.x, 0f, inputVector.y).normalized);
        }
        
    }


    protected IEnumerator<float> DecelerationCo()
    {
        while (currentSpeed > 0 && inputVector==Vector2.zero)
        {
            // Riduci gradualmente la velocità
            currentSpeed -= deceleration * Time.deltaTime;
            // Attendi il prossimo frame
            yield return Timing.WaitForOneFrame;
        }
        if(currentSpeed < 0)
        {
            currentSpeed = 0f;
            Timing.KillCoroutines("Deceleration");
            SetMovementDirection(Vector3.zero);
        }
        Timing.KillCoroutines("Deceleration");
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
    public void ApplyPowerUp(EPowerUpType type, float value)
    {
        switch (type)
        {
            case EPowerUpType.Speed:
                Speed += value;
                break;
            case EPowerUpType.FireRate:
                fireRatio -= value;
                break;
            case EPowerUpType.PlunderRate:
                break;
            case EPowerUpType.KillScore:
                break;
            default:
                break;
        }
    }
}
using MEC;
using System.Collections.Generic;
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
        controls.Player.Movement.canceled += OnMoveCanceled;
        reload = reloadCannonTime;
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Movement.performed -= OnMovePerformed;
        controls.Player.Movement.canceled -= OnMoveCanceled;
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        controls.Player.Fire.performed += ctx => StartFire();
        controls.Player.Pause.performed += ctx => GameController.Instance.Pause();
        // Velocity of Input
        SetMovementDirection(new Vector3(inputVector.x, 0f, inputVector.y).normalized);
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        //Here the logic for acceleration deceleration
        // When button is Unpressed stopMovement
        SetMovementDirection(Vector3.zero);
    }
    private void StartFire()
    {  
        if (ammoCount > 0 && canFire)
        {
            ammoCount--;
            GameController.Instance.UpdateAmmo(ammoCount);
            
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
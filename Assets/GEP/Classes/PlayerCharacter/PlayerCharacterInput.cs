using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterInput : MonoBehaviour
{
    Inventory inv;
    PlayerRaycast pRaycast;

    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    private void Awake()
    {
        inv = GetComponent<Inventory>();
        pRaycast = GameObject.FindGameObjectWithTag("RaycastStart").GetComponent<PlayerRaycast>();
    }

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void OnPlace(InputValue value)
    {
        if (value.isPressed)
        {
            inv.UseItem();
            //if (!inv.IsSelectedSlotEmpty())
          //  {
        //        ItemDef current_item = inv.GetSelectedSlotsItem();
      //          GameObject b = Instantiate(current_item.prefab);
    //            b.transform.position = pRaycast.GetCast().point;
  //              inv.RemoveItem();
//            }
        }
    }

    public void OnBreak(InputValue value)
    {
        if (value.isPressed)
        {
            RaycastHit hit = pRaycast.GetCast();
            if (hit.collider != null)
            {
                IObject objInterface = hit.collider.gameObject.GetComponent<IObject>();
                if (objInterface != null)
                {
                    objInterface.Break();
                }
            }

        }
    }

    public void OnScroll(InputValue value)
    {
        Vector2 a = value.Get<Vector2>();
        if (a.y > 0)
        {
            inv.IncreaseSelectedSlot();
        }
        else if (a.y < 0)
        {
            inv.DecreaseSelectedSlot();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

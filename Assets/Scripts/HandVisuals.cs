using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HandPoses
{
    Idle,
    SoftGrab = 20,
    MediumGrab,
    HardGrab,
    SoftPinch = 30,
    MediumPinch,
    HardPinch
}

[RequireComponent(typeof(Animator))]
public class HandVisuals : MonoBehaviour
{
    protected Animator animator;


    //TODO use property
    [SerializeField]
    public Transform attachTransform;

    [SerializeField]
    private InputActionProperty flex;

    public void Awake()
    {
  
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //LockPose(HandPoses.MediumGrab);
    }

    public void LockPose(HandPoses newPose)
    {
        animator.SetInteger("LockedPose", (int)newPose);
    }

    private void SetAnimatorInputValue(InputAction action, string parameter)
    {
        if (action != null)
        {
            animator.SetFloat(parameter, action.ReadValue<float>());
        }
    }

    public void Update()
    {
        SetAnimatorInputValue(flex.action, "ControllerSelectValue");
    }
}

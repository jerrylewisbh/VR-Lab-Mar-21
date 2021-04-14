using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public enum RotationAxis
{
    X,
    Y,
    Z
}

public class Door : XRSimpleInteractable
{


    [Header("Transforms")]
    [SerializeField]
    private Transform doorPivot;

    [SerializeField]
    private Transform movablePart;


    [Header("Directions")]
    [SerializeField]
    private Vector3 localStartDirection;

    [SerializeField]
    private Vector3 localEndDirection;

    [SerializeField]
    private RotationAxis doorRotationAxis;

    [SerializeField]
    private bool reverseRotation;

    protected override void Awake()
    {
        onSelectEntered.AddListener(OnInteractionBegin);
        onSelectExited.AddListener(OnInteractionEnd);

        base.Awake();
    }

    private Transform currentInteractor = null;

    private void OnInteractionBegin(XRBaseInteractor interactor)
    {
        currentInteractor = interactor.transform;
        //this is called when we start grabbing the door handle
    }
    private void OnInteractionEnd(XRBaseInteractor interactor)
    {
        currentInteractor = null;
        //this is called when we release the door handle
    }

    private void OnInteracting()
    {
        //called while one interactor is interacting with the door
    }


    // Update is called once per frame
    void Update()
    {
        if(currentInteractor != null)
        {
            OnInteracting();
        }
    }
}

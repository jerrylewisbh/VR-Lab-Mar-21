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

    private float maxLocalAngle;


    private Quaternion initialLocalRotation;

    protected override void Awake()
    {
        onSelectEntered.AddListener(OnInteractionBegin);
        onSelectExited.AddListener(OnInteractionEnd);

        //maximum angle between start and end directions
        maxLocalAngle = Vector3.Angle(localStartDirection, localEndDirection);

        //original rotation of the door relative to the Door Pivot;
        initialLocalRotation = movablePart.transform.localRotation;

        if (reverseRotation)
        {
            maxLocalAngle = maxLocalAngle * -1;
        }

        base.Awake();
    }

    private Transform currentInteractor = null;

    private void OnInteractionBegin(XRBaseInteractor interactor)
    {
        currentInteractor = interactor.transform;

        ProxyHand hand = GetComponentInChildren<ProxyHand>(true);

        if (hand)
        {
            hand.Activate();
            currentInteractor.GetComponent<XRBaseController>().hideControllerModel = true;
        }

       
        //this is called when we start grabbing the door handle
    }
    private void OnInteractionEnd(XRBaseInteractor interactor)
    {
        ProxyHand hand = GetComponentInChildren<ProxyHand>(true);

        if (hand)
        {
            currentInteractor.GetComponent<XRBaseController>().hideControllerModel = false;
            hand.Deactivate();
        }

        currentInteractor = null;
        //this is called when we release the door handle
    }

    private void OnInteracting()
    {
        //called while one interactor is interacting with the door

        Vector3 handDirection = ConvertWorldPointToLocalPoint(currentInteractor.transform.position);
        Vector3 startDirection = doorPivot.TransformDirection(localStartDirection);

        float currentAngle = Vector3.Angle( handDirection, startDirection);

        Vector3 perpendicular = Vector3.Cross(startDirection, handDirection);

        float dot = Vector3.Dot(perpendicular, doorPivot.TransformDirection(AxisDirectionToVector(doorRotationAxis)));


        if(dot < 0)
        {
            currentAngle = currentAngle *= -1;
        }


        if(currentAngle > 0)
        {
            SetPercentageOpen(Mathf.Clamp(currentAngle, 0, maxLocalAngle)/maxLocalAngle);
        }
        else
        {
            SetPercentageOpen(Mathf.Clamp(currentAngle,  maxLocalAngle, 0) / maxLocalAngle);
        }
    }


    private void SetPercentageOpen(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        Vector3 pivotAxis = doorPivot.TransformDirection(AxisDirectionToVector(doorRotationAxis));
        movablePart.localRotation = initialLocalRotation;
        movablePart.RotateAround(doorPivot.position, pivotAxis, percentage * maxLocalAngle);

    }

    private Vector3 ConvertWorldPointToLocalPoint(Vector3 point)
    {

        Vector3 axis = AxisDirectionToVector(doorRotationAxis);
        Vector3 pivotAxis = doorPivot.TransformDirection(axis.normalized);

        Vector3 projectedPoint = Vector3.ProjectOnPlane(point - doorPivot.position, pivotAxis).normalized;

        return projectedPoint;

    }


    private Vector3 AxisDirectionToVector(RotationAxis axis)
    {

        Vector3 returnValue = Vector3.up;

        switch (axis)
        {
            case RotationAxis.X:
                returnValue =  Vector3.right;
                break;
            case RotationAxis.Y:
                returnValue =  Vector3.up;
                break;
            case RotationAxis.Z:
                returnValue =  Vector3.forward;
                break;
        }

        return returnValue;


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

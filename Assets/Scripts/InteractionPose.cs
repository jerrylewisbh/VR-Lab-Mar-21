using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractionPose : MonoBehaviour
{

    [SerializeField]
    private bool hideController = false;

    [SerializeField]
    private HandPoses interactionPose;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }


    // Start is called before the first frame update
    void Start()
    {
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnRelease);
    }

    public void OnGrab(XRBaseInteractor interactor)
    {
        interactor.GetComponent<XRBaseController>().hideControllerModel = hideController;
        ChangePose(interactionPose, interactor);
    }

    public void OnRelease(XRBaseInteractor interactor)
    {
        interactor.GetComponent<XRBaseController>().hideControllerModel = false;
        ChangePose(HandPoses.Idle, interactor);
    }

    private void ChangePose(HandPoses pose, XRBaseInteractor interactor)
    {
        HandVisuals visuals = interactor.GetComponentInChildren<HandVisuals>();


        if (visuals != null)
        {
            visuals.LockPose(pose);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

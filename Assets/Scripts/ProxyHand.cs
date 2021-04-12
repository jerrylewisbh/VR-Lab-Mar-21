using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyHand : HandVisuals
{

    [SerializeField]
    private HandPoses handPose;

    public void Start()
    {
        Deactivate(); 
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        LockPose(handPose);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MenuUITransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;

    public void Start()
    {
        currentCamera.Priority++;
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--;

        currentCamera = target;

        currentCamera.Priority++;
        
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public List<CinemachineVirtualCamera> _listCamera = new();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void AddCameraToList(CinemachineVirtualCamera camera)
    {
        _listCamera.Add(camera);
    }

    public void RemoveCameraFromList(CinemachineVirtualCamera camera)
    {
        _listCamera.Remove(camera);
    }

    public void SwitchingCamera(CinemachineVirtualCamera camera)
    {
        foreach (var cam in _listCamera)
            cam.Priority = (cam != camera) ? 0 : 11;
    }
}

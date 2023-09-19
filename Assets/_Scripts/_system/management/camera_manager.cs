using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.Timeline;

public class camera_manager : OptimizedBehaviour
{
    public List<CinemachineVirtualCamera> Cameras = new List<CinemachineVirtualCamera>();
    public GameObject gui;

    public float Timer;
    public float ChangeTime;
    public int CameraPrio;
    public int CameraIndex;
    public bool ChangeCam;

    private void Start()
    {
        CameraPrio = Cameras[0].Priority;
        for (int i = 0; i < Cameras.Count; i++)
        {
            Cameras[i].Priority = 0;
        }
        Cameras[0].Priority = CameraPrio;
        CameraIndex = 0;
        Timer = ChangeTime;
        ChangeCam = true;
    }
    private void Update()
    {
        UpdateTimer();
    }
    public void ResetCameraTimer(float changeTime)
    {
        Timer = changeTime;
        ChangeCam = true;
    }
    private void UpdateTimer()
    {
        if (Timer <= 0)
        {
            ChangeCamera();
        }
        else
            Timer -= Time.deltaTime;
    }
    private void ChangeCamera()
    {
        if (ChangeCam && CameraIndex <= Cameras.Count)
        {
            CameraIndex++;
            Cameras[CameraIndex].Priority = CameraPrio;
            gui.SetActive(true);
            Cameras[CameraIndex -1].Priority = 0;
            ChangeCam = false;
        }
    }
}

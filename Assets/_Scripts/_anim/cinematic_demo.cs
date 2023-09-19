using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class cinematic_demo : OptimizedBehaviour
{
    public CinemachineVirtualCamera cmCam; 
    public List<OptimizedBehaviour> unitsToAnimate = new List<OptimizedBehaviour>();
    public List<float> journeyLengths = new List<float>();
    public List<Vector3> points = new List<Vector3>();
    public Vector3 startPoint;
    public float speed;
    public float startTime;

    public float Timer01 = 0;
    public float Time01 = 0;


    private void Start()
    {
        for (int i = 0; i < unitsToAnimate.Count; i++)
        {
            unitsToAnimate[i].CachedTransform.position = startPoint;
            journeyLengths.Add(Vector3.Distance(startPoint, points[i]));
        }

        // Calculate the journey length.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color= Color.grey;
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawSphere(points[i], 1f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(startPoint, 1f);
    }

    private void Update()
    {
        UpdateTimer();
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.

        for (int i = 0; i < unitsToAnimate.Count; i++)
        {
            float fractionOfJourney = distCovered / journeyLengths[i];
            unitsToAnimate[i].CachedTransform.position = Vector3.Slerp(startPoint, points[i], fractionOfJourney);
        }
    }

    private void UpdateTimer()
    {
        Timer01 -= Time.deltaTime;
    }
}

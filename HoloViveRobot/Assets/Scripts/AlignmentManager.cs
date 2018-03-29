﻿using UnityEngine;
using UnityEngine.Networking;

public class AlignmentManager : NetworkBehaviour
{
    public enum State
    {
        Normal,
        Aligning,
    }

    public delegate void AlignmentStartedHandler();
    public delegate void AlignmentFinishedHandler(bool success, Vector3 position, float rotationY);
    public delegate void ControllersAvailableHandler();
    public delegate void ControllersUnavailableHandler();

    [SyncEvent]
    public event AlignmentStartedHandler EventAlignmentStarted;

    [SyncEvent]
    public event AlignmentFinishedHandler EventAlignmentFinished;

    [SyncEvent]
    public event ControllersAvailableHandler EventControllersAvailable;

    [SyncEvent]
    public event ControllersUnavailableHandler EventControllersUnavailable;

    public GameObject[] entitiesToAlign;

    private bool hasTarget;
    private Vector3 targetPosition;
    private float targetRotation;

    [SyncVar]
    private State state = State.Normal;

    [SyncVar]
    private int numControllersPresent = 0;

    public bool CanAlign
    {
        get
        {
            return HasControllers && !CurrentlyAligning;
        }
    }

    public bool HasControllers
    {
        get
        {
            return numControllersPresent > 0;
        }
    }

    public bool CurrentlyAligning
    {
        get
        {
            return state == State.Aligning;
        }
    }

    [Client]
    public void ApplyLocalAlignment(Vector3 position, float rotation)
    {
        foreach (var entity in entitiesToAlign)
        {
            entity.transform.position = (Quaternion.Euler(0, rotation, 0) * position);
            entity.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    [Server]
    public void TargetInfo(Vector3 position, float rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
        hasTarget = true;
    }

    [Server]
    public void RequestAlignment()
    {
        if (CanAlign)
        {
            StartAlignment();
        }
    }

    [Server]
    private void StartAlignment()
    {
        hasTarget = false;
        state = State.Aligning;

        Debug.Log("Alignment started.");
        if (EventAlignmentStarted != null) EventAlignmentStarted();
    }

    [Server]
    private void CancelAlignment()
    {
        hasTarget = false;
        state = State.Normal;

        Debug.Log("Alignment canceled.");
        if (EventAlignmentFinished != null) EventAlignmentFinished(false, Vector3.zero, 0);
    }

    [Server]
    private void FinishAlignment(Vector3 position, float rotation)
    {
        if (!hasTarget) return;

        hasTarget = false;
        state = State.Normal;

        var deltaPosition = position - targetPosition;
        var deltaRotation = rotation - targetRotation - 180;

        Debug.Log("Alignment finished. Delta pos: " + deltaPosition + " rotation: " + deltaRotation);
        if (EventAlignmentFinished != null) EventAlignmentFinished(true, -deltaPosition, -deltaRotation);
    }

    [Command]
    public void CmdSetNumControllers(int numControllers)
    {
        bool hadControllers = HasControllers;
        numControllersPresent = numControllers;

        if (HasControllers && !hadControllers)
        {
            if (EventControllersAvailable != null) EventControllersAvailable();
        }
        else if (!HasControllers && hadControllers)
        {
            if (EventControllersUnavailable != null) EventControllersUnavailable();
            if (CurrentlyAligning) CancelAlignment();
        }
    }

    [Command]
    private void CmdControllerClicked(Vector3 position, float rotation)
    {
        if (hasTarget)
        {
            FinishAlignment(position, rotation);
        }
    }

    [Client]
    public void ControllerClicked(Transform transform)
    {
        CmdControllerClicked(transform.position, transform.rotation.eulerAngles.y);
    }
}

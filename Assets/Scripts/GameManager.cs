using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public Material[] materials;
    [SerializeField] public int totalLuggage;

    public Transform target { get; private set; }

    [NonSerialized] public bool acceptingNewTargets = true;

    public enum TripState
    {
        STATIC, PACKING, BOARDING, TRAVELING, ARRIVED, CANCELLED, RETURN, ENGINE_FAILURE
    }

    public TripState currentState = TripState.STATIC;

    public void SetTarget(Transform t)
    {
        if (acceptingNewTargets)
        {
            target = t;
            Debug.Log("New target set!");
            return;
        }

        Debug.Log("Not accepting targets at this time!");
    }
}



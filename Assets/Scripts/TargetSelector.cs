using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    MeshRenderer mr;

    bool isTarget;
    bool isHovering;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        isTarget = GameManager.Instance.target == transform;
        if (isTarget)
            return;
        if (isHovering)
            return;
        mr.material = GameManager.Instance.materials[0];
    }

    private void OnMouseEnter()
    {
        if (isTarget)
            return;
        mr.material = GameManager.Instance.materials[1];
        isHovering = true;
    }

    private void OnMouseExit()
    {
        isHovering = false;
        if (isTarget)
            return;
        mr.material = GameManager.Instance.materials[0];
    }

    private void OnMouseDown()
    {
        GameManager.Instance.SetTarget(transform);
        GameManager.Instance.acceptingNewTargets = false;
        GameManager.Instance.currentState = GameManager.TripState.PACKING;
        mr.material = GameManager.Instance.materials[2];
    }
}

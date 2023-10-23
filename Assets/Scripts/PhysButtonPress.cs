using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PhysButtonPress : MonoBehaviour
{
    public float threshold = .1f;
    public float deadZone = 0.025f;
    public UnityEvent onPressed, onReleased;

    private bool isPressed;
    private Vector3 startPos;
    private ConfigurableJoint joint;

    private float GetValue()
    {
        var value = Vector3.Distance(startPos, transform.localPosition) / joint.linearLimit.limit;

        if(Math.Abs(value) < deadZone)
        {
            value = 0;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPressed && GetValue() + threshold >= 1)
        {
            Pressed();
        }
        if(isPressed && GetValue() - threshold <= 0)
        {
            Released();
        }
    }

    private void Pressed()
    {
        isPressed = true;
        onPressed.Invoke();
    }

    private void Released()
    {
        isPressed = false;
        onReleased.Invoke();
    }
}

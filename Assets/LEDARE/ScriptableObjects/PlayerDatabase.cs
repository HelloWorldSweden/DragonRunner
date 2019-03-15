using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data")]
public class PlayerDatabase : ScriptableObject
{
    [Header("Movement")]
    public float maxSpeed = 10f;
    public float directionDelta = 2f;
    public float accelerationTime = 0.3f;
    public float decelerationTime = 0.2f;
    public float turnAccelerationTime = .1f;
    public float turnDecelerationTime = .05f;
    public float deceleration = 20f;

    [Header("Jumping")]
    public float initialJumpSpeed = 15f;

    public float frameDelay = 0.05f;
    public float jumpHoldTime = 0.5f;
    public float jumpReleasePercentage = 0.55f;
    public float jumpHoldSpeed = 0.4f;
    
    public float jumpHoldDecay = 0.5f;
    public float gravity = 1f;
    public float groundCheckDistance = 1.6f;

    public float terminalVelocity = 50f;
    [Header("Air Control")]
    public float airControlAcceleration = 0.5f;
    public float airControlDeceleration = 0.5f;
    public float passiveAirDeceleration = .1f;
    public float minPercentOfAirControl = 0.4f;
    public float airDotForDeceleration = -.3f;
    [Header("Rotation")]
    public float rotationSpeed = 10f;
    public float atJumpRotationTime = 0.3f;
    public float atJumpRotationSpeed = 50f;
    public float airborneRotationSpeed = 5f;
    [Header("Temp")]
    public float prettyColorDistance = 1.6f;
    public Color prettyColor = Color.magenta;
    public float prettyColorTime;

}


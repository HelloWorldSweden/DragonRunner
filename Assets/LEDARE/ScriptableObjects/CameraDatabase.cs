using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDatabase : ScriptableObject
{

    [Header("Look At")]
    public float accelerationLookAt;
    public float decelertaionLookAt;
    [Tooltip("the speed the camra is rotating at when looking at the player")]
    public float lookAtSpeed = 20f;
    public float airborneLookAtSpeed = 0.1f;
    public Vector3 cameraLookAtOffset = Vector3.zero;

    [Header("Movement")]
    public Vector3 cameraOffset = Vector3.zero;
    public float planarSpeed = 1;
    public float planarRetreatSpeed = .5f;
    public float heightSpeed = 2f;
    public float cameraNearLimit = 1f;

    public float accelerationMove = 1;
    public float decelertaionMove = 1;

    public float rotateAroundSpeed = 1f;

        [Header("Camera Flip")]
    public float flipAngle = 10;
    public float flipTime = 0.5f;


    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Header("Gammalt")]


    public float accelerationTime = 1.5f;
    public float decelerationTime = 0.3f;
    public float rotationSpeed1 = 1;
    public float lookAtMaxOffset = 10;
    public float minDistanceToPlayer = 1f;

    [Tooltip("How fast the camra will rotate towards the player after a freze")]
    public float returnLookAtSpeed = 10f;

    [Tooltip("How far back the camera will move relative to player speed")]
    public float camerActiveFarOffset = 10;
    [Tooltip("How close the camera will move relative to player speed")]
    public float camerActiveCloseOffset = 1;
    public float maxPlanarDistance = 15f;
    public float maxHeightDistance = 15f;
    public float minPlanarDistance = 5f;
}

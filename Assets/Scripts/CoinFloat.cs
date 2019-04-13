using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public class CoinFloat : MonoBehaviour {

	public float rotateSpeed = 10;
	public float floatSpeed = 2;
	[FormerlySerializedAs("floatDist")]
	public float floatAmplitude = 0.5f;

	private Vector3 startPosition;

	void Start() {
		startPosition = transform.position;
	}

	void Update ()
	{
		float deltaY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
		transform.position = new Vector3(startPosition.x, startPosition.y + deltaY, startPosition.z);

		float deltaAngle = rotateSpeed * Time.deltaTime;
		transform.localEulerAngles += new Vector3(0, deltaAngle, deltaAngle);
	}
}

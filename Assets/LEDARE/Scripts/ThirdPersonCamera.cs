using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	public float ClampAngle, MouseSensitivity, IdleCameraSpeed;
	public bool InvertedYAxsis;

	GameObject _player;

	float _rotX, _rotY;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindGameObjectWithTag("Player");
		Cursor.lockState = CursorLockMode.Locked;

		Vector3 euler = transform.eulerAngles;
		_rotX = euler.x;
		_rotY = euler.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = _player.transform.position;
		RotateAround( new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
	}

	void RotateAround(Vector2 input)
	{
		
		if (input.magnitude >= 0)
		{
			float mouseX = input.x;
			float mouseY = InvertedYAxsis ? -input.y : input.y;

			_rotY += mouseX * MouseSensitivity * Time.deltaTime;
			_rotX += mouseY * MouseSensitivity * Time.deltaTime;

			transform.rotation = Quaternion.Euler(_rotX, _rotY, 0.0f);
		}
		else
		{

			transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * IdleCameraSpeed);
		}


		_rotX = Mathf.Clamp(_rotX, -ClampAngle, ClampAngle);
	}

}

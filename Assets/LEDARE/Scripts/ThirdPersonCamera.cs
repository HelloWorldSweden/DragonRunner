using System.Collections;
using System.Collections.Generic;
using System.Reflection;
//using UnityEditor;
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
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position = _player.transform.position;
        RotateAround( new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

   //     LookAtPlayer(_player);
	}

	void RotateAround(Vector2 input)
    {
        
        if (input.magnitude >= 0)
        {
            float mouseX = input.x;
            float mouseY = InvertedYAxsis ? -input.y : input.y;

            _rotY += mouseX * MouseSensitivity * Time.deltaTime;
            _rotX += mouseY * MouseSensitivity * Time.deltaTime;



            Quaternion localRotation = Quaternion.Euler(_rotX, _rotY, 0.0f);


            transform.rotation = localRotation;
        }
        else
        {

            transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * IdleCameraSpeed);

         //   _rotX = transform.rotation.eulerAngles.x;
         //   _rotY = transform.rotation.eulerAngles.y;
        }


        _rotX = Mathf.Clamp(_rotX, -ClampAngle, ClampAngle);
    }

	void LookAtPlayer(GameObject player) {

    }
    
}

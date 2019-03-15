using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class DynamicCamera : MonoBehaviour
{
    //References
	CameraDatabase data;

	Transform player;
	PlayerMovement playerMovement;

    //Lookat
	bool groundedLastFrame = false;

	float lookAtSpeed = 0;
	float groundedY = 0;

    //Movement
	Vector2 playerPlanar;

	Vector2 cameraPlanar;
	float moveSpeed = 0;
	int movementSign = 1;

    //RotateAround
	float rotateAroundSpeed = 50f;

	int rotateAroundSign = 1;
	bool rotating = false;



    void Awake()
    {
        data = Resources.Load<CameraDatabase>("CameraDatabase");
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Start()
    {

    }

    void Update()
    {
        playerPlanar = new Vector2(player.position.x, player.position.z);
        cameraPlanar = new Vector2(transform.position.x, transform.position.z);
        Rotate(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"),0 ));
        LookAt();
        YMove();
        Move();
    }

	void Rotate(Vector3 input)
    {
        if (Vector2.Dot(new Vector2(player.forward.x, player.forward.z).normalized, (cameraPlanar - playerPlanar).normalized) > 0.2f)    //Start
        {
            Debug.Log("Start");
            if (!rotating)
            {
                Vector3 relativeForward = transform.InverseTransformDirection( player.forward);
                rotating = true;
                rotateAroundSign = (int)Mathf.Sign(relativeForward.x);
                rotateAroundSpeed = 50f;
                //Debug.Log(rotateAroundSign);
            }
        }
        else if (Vector2.Dot(new Vector2(player.forward.x, player.forward.z).normalized,(cameraPlanar - playerPlanar).normalized) < -0.7f)
        {
            Debug.Log("Stop");
            rotateAroundSpeed = 0f;
            rotating = false;
        }

        transform.RotateAround(player.position, Vector3.up, rotateAroundSign * rotateAroundSpeed * Time.deltaTime);

    }

	void LookAt()
    {
        if (playerMovement.grounded)
        {
            if (lookAtSpeed != data.lookAtSpeed)
                lookAtSpeed = Mathf.Lerp(lookAtSpeed, data.lookAtSpeed, data.accelerationLookAt * Time.deltaTime);

            Quaternion rot = Quaternion.LookRotation(
                (player.position + data.cameraLookAtOffset - transform.position).normalized, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookAtSpeed * Time.deltaTime);

            //Debug.Log("Looking");
        }
        else
        {       //Airborne
            if (lookAtSpeed != 0.1f)
                lookAtSpeed = Mathf.Lerp(lookAtSpeed, data.airborneLookAtSpeed, data.decelertaionLookAt * Time.deltaTime);

            if (groundedLastFrame)
            {
                groundedY = player.position.y - 1;
            }

            Quaternion rot = Quaternion.LookRotation((new Vector3(player.position.x, groundedY, player.position.z) + data.cameraLookAtOffset - transform.position).normalized, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookAtSpeed * Time.deltaTime);
        }

        groundedLastFrame = playerMovement.grounded;
    }

	void Move()
    {
        if (Vector2.Distance(playerPlanar, cameraPlanar) > data.cameraOffset.z)
        {
            movementSign = 1;
            moveSpeed = Mathf.Lerp(moveSpeed, data.planarSpeed, data.accelerationMove * Time.deltaTime);
        }
        else if(playerMovement.velocity.magnitude == 0)
        {
            movementSign = 1;
            moveSpeed = Mathf.Lerp(moveSpeed, 0, data.decelertaionMove * Time.deltaTime);
        }
        else if(Vector2.Distance(playerPlanar, cameraPlanar) < data.cameraNearLimit)
        {
            movementSign = -1;
            moveSpeed = Mathf.Lerp(moveSpeed, data.planarRetreatSpeed, data.accelerationMove * Time.deltaTime);
        }

        Debug.DrawRay(player.position,(transform.position - new Vector3(movementSign * playerPlanar.x, transform.position.y, movementSign * playerPlanar.y)).normalized * 10f, Color.green );
        transform.position = Vector3.Lerp(transform.position, new Vector3(movementSign * playerPlanar.x, transform.position.y, movementSign * playerPlanar.y), moveSpeed * Time.deltaTime );
    }


	void YMove()
    {
        if (Mathf.Abs(player.position.y - transform.position.y) > data.maxHeightDistance)
        {
            //If player is too high or too low
            Vector3 newPos = new Vector3(transform.position.x, player.position.y + data.cameraOffset.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, data.heightSpeed * Time.deltaTime);
        }
    }

}

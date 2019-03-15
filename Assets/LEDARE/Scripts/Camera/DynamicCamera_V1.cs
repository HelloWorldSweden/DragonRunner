using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera_V1 : MonoBehaviour
{
	CameraDatabase data;

	Transform player;
	float speed;
	float speedRef;
	float cameraDist = 0;
	Vector3 relativeCameraOffset = Vector3.zero;
	Vector3 inputVector = Vector3.zero;

	PlayerMovement playerMove;

	Vector2 planarPosition;
	Vector2 playerPlanarPosition;

    public float off;

	bool lastGrounded = false;
	bool transitioningRotasion = false;

	float flipTimer = 0;

	List<Vector3> wiskerPoints = new List<Vector3>();

    void Awake()
    {
        if (!data)
        {
            data = Resources.Load("CameraDatabase") as CameraDatabase;
        }

        for (int i = 0; i < 5; i++)
        {
            float radian = (30 + i*30) *Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float y = Mathf.Sin(radian);
            Vector3 newPos = new Vector3(x, 0, y);
            wiskerPoints.Add(newPos);
        }
    }

    void Start ()
	{
	    player = GameObject.FindGameObjectWithTag("Player").transform;
	    playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}
	
	void Update ()
	{
	    RotateCamera(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"),0 ));
	    AdjustPath();
        MoveCamera();
	}

	void RotateCamera(Vector3 input)
    {
        off = data.lookAtMaxOffset * Mathf.Clamp01(Vector3.Distance(player.position, transform.position)/data.minDistanceToPlayer);
        inputVector = Vector3.Lerp(inputVector, input * off, data.rotationSpeed1 * Time.deltaTime);

    }

    public void MoveCamera()
    {

        cameraDist = new Vector3(playerMove.velocity.x, 0, playerMove.velocity.z).magnitude * data.camerActiveFarOffset;
        cameraDist = Mathf.Clamp(cameraDist, data.camerActiveCloseOffset, 100);

        planarPosition = new Vector2(transform.position.x,transform.position.z);
        playerPlanarPosition = new Vector2(player.position.x, player.position.z);

        Vector3 newPos = Vector3.zero;
        //float playerY = player.position.y;
        //float myY = transform.position.y;
        CheckForFlip();
        if ((/*playerMove.velocity.magnitude > 0 &&*/ Vector3.Distance(planarPosition, playerPlanarPosition) < data.minPlanarDistance)
            || Vector3.Distance(planarPosition, playerPlanarPosition) > data.maxPlanarDistance)
        {
          //  speed = Mathf.SmoothDamp(speed, data.maxSpeed, ref speedRef, data.accelerationTime);
        }
        else
        {
            speed = Mathf.SmoothDamp(speed, 0, ref speedRef, data.decelerationTime);
        }


        if (playerMove.grounded)
        {
            relativeCameraOffset = player.TransformPoint(new Vector3(data.cameraLookAtOffset.x, data.cameraLookAtOffset.y * cameraDist, data.cameraLookAtOffset.z * cameraDist));
            newPos.x = Mathf.Lerp(transform.position.x, relativeCameraOffset.x, Time.deltaTime * speed);
            newPos.y = Mathf.Lerp(transform.position.y, relativeCameraOffset.y, Time.deltaTime * 5);
            newPos.z = Mathf.Lerp(transform.position.z, relativeCameraOffset.z, Time.deltaTime * speed);

        }
        else
        {
            newPos.x = Mathf.Lerp(transform.position.x, relativeCameraOffset.x, Time.deltaTime * 0.2f);
            newPos.y = Mathf.Lerp(transform.position.y, relativeCameraOffset.y, Time.deltaTime * 1);
            newPos.z = Mathf.Lerp(transform.position.z, relativeCameraOffset.z, Time.deltaTime * 0.2f);
        }
        transform.position = newPos;

        //Check if camera shuld follow
        if (playerMove.grounded && !lastGrounded)
        {
            StartCoroutine(SlowTurn());
        }
        else if(playerMove.grounded && !transitioningRotasion)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((player.position + transform.TransformDirection(inputVector /data.lookAtSpeed) - transform.position).normalized + transform.TransformDirection(inputVector/data.lookAtSpeed)), data.lookAtSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((player.position + transform.TransformDirection(inputVector /0.1f) - transform.position).normalized + transform.TransformDirection(inputVector/0.1f)), 0.1f * Time.deltaTime);
        }


        lastGrounded = playerMove.grounded;
    }

	void AdjustPath()
    {

        foreach (var wiskerPoint in wiskerPoints)
        {
            Debug.DrawRay(player.position,(transform.position + wiskerPoint) - player.position, Color.blue);
        }


    }

	void CheckForFlip()
    {
        Vector3 camDir = transform.position;
        camDir.y = player.transform.position.y;
        camDir = camDir - player.transform.position;
        //Debug.Log(Vector2.Dot(new Vector2(player.transform.forward.x, player.transform.forward.z), new Vector2(camDir.x, camDir.z).normalized));
        if (Vector2.Dot(new Vector2(player.transform.forward.x, player.transform.forward.z), new Vector2(camDir.x, camDir.z).normalized) > 0.9 && playerMove.PlanarDirection.magnitude > 0)
        {
            flipTimer += Time.deltaTime;
            if (flipTimer > data.flipTime)
            {
                //transform.RotateAround(player.position, Vector3.up, 180);
                StartCoroutine(Flip(500, Vector2.Distance(planarPosition, playerPlanarPosition)));
                Debug.Log("It shuld flip");
            }

        }
        else if (flipTimer > 0)
        {
            flipTimer = 0;
        }

    }

	IEnumerator Flip(float speed, float radius)
    {
        float rotation = 90;
        Debug.Log("Flipping");
        //float t = 0;

        while (rotation < 270)
        {
            float radian = rotation*Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float y = Mathf.Sin(radian);
            Vector3 newPos = new Vector3(x, 0, y) * radius;
            newPos = player.TransformPoint(newPos);
            Debug.DrawRay(player.position, newPos - player.position, Color.magenta, 2f);
            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
            rotation += speed * Time.deltaTime;
            yield return 0;
        }

        yield return 0;

    }

	IEnumerator SlowTurn()
    {

        transitioningRotasion = true;
        float dotProd = 0;
        Vector3 cameraDir = Vector3.zero;
        while (dotProd < 0.99)
        {
            cameraDir = (player.position + transform.TransformDirection(inputVector/10) - transform.position).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraDir + transform.TransformDirection(inputVector/data.returnLookAtSpeed)), data.returnLookAtSpeed * Time.deltaTime);
            dotProd = Vector3.Dot(cameraDir, transform.forward);
               Debug.Log(dotProd);
            yield return 0;
        }
        transitioningRotasion = false;
            yield return 0;
    }
}

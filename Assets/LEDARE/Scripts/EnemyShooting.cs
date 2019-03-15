using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.WSA;

public class EnemyShooting : MonoBehaviour
{
    
    //public float walkSpeed = 10;
    //private NavMeshAgent nav;

    //public Transform target1, target2;

    public GameObject bullet;
    public float bulletDelay;

	float bulletTimer;
	
	void Start ()
	{
	    /*nav = GetComponent<NavMeshAgent>();
	    nav.speed = walkSpeed;

	    nav.SetDestination(target1.position);*/
	}
	
	
	void Update ()
	{
        //Movement();
        Shooting();
	}

    /*void Movement()
    {
        if (nav.remainingDistance < 0.1)
        {
            if (nav.destination == new Vector3(target1.position.x, nav.destination.y, target1.position.z))
            {
                nav.SetDestination(target2.position);
            }
            else
            {
                nav.SetDestination(target1.position);
            }
        }
    }*/

    void Shooting()
    {
        bulletTimer += Time.deltaTime;
        if (bulletTimer >= bulletDelay)
        {
            bulletTimer = 0;
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }

}

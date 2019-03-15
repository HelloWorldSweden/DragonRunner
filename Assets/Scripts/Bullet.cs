using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detta är ett script som sätts på en "bullet" (kulan) som fiender skjuter för att skada spelaren.

public class Bullet : MonoBehaviour {
	
    public float speed = 5;    //Kulans hastighet, detta värde går att ändra i Inspektorn.
    public float maxTravelDistance;  // Så långt kulan kan åka. Värdet ändras i Inspektorn.

    Vector3 startPosition;  // Kulans startposition. 


	void Start () {
	    startPosition = transform.position;  //Här sätter vi kulans startposition, som kommer vara fienden (eftersom det är den som skjuter iväg dem)
	}
	
	void Update () {
	    transform.position += transform.forward * Time.deltaTime * speed;   // Gör att kulan rör sig frammåt i den hastighet vi satt i Inspektorn.

	    if (Vector3.Distance(startPosition, transform.position) > maxTravelDistance) { // Förstör kulan om den åker för långt
	        Destroy(gameObject);
	    }
	}

}

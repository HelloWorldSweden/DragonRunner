using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detta script sätts på ett objekt som ska göra att spelaren kan hoppa mycket högre. Till exempel en trampolin.

public class Trampoline : MonoBehaviour {
    
	public float jumpForce = 50; // Detta är hur mycket högra spelaren ska kunna hoppa när den hoppar på detta objekt. Siffran kan ändras i Inspektorn.

    PlayerMovement player; 

    void Start() {
        player = FindObjectOfType<PlayerMovement>(); // Vi sparar ner spelaren så vi vet att det är den som ska interagera med trampolinen. 

    }

    void OnTriggerEnter(Collider other) {
        //Om spelaren rör detta objekt ska metoden ApplyForce köras.
        if (other.CompareTag("Player")) {
            player.ApplyForce(jumpForce); // Denna metod finns i spelarens script, på detta sätt kallar vi en metod från ett attat script. 
                                         // Denna metod gör så att spelarens hopp blir större.
        }
    }
}

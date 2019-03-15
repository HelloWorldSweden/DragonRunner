using UnityEngine;

// Detta är attribut som spelaren har. Detta script sätts på spelaren. 

public class PlayerHealth : MonoBehaviour {

	public float maxLives = 5;  // Spelarens max-liv innan den dör. Siffran kan ändras i Insektorn. 
	public float currentLives;  // Hur många liv spelaren har just nu. 

	void Start() {
		currentLives = maxLives; // När vi startar spelet har spelaren fulliv. Vårt nuvarande liv är alltså vårt maxliv. 
	}

	void Update () {

	}
}

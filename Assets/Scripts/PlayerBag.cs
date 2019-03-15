using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Detta skript håller koll på alla objekt som spelaren kan interagera med.

public class PlayerBag : MonoBehaviour{
	
	public bool hasKey; // true/false om spelaren har plockat upp nyckeln eller inte.
	public int coinCounter; // En mynträknare

	void OnTriggerEnter(Collider other){


	}
}

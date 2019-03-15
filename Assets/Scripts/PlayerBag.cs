using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Detta skript håller koll på alla objekt som spelaren kan interagera med.

public class PlayerBag : MonoBehaviour
{
	
	public bool hasKey; // true/false om spelaren har plockat upp nyckeln eller inte.

	public int coinCounter; // En mynträknare

	void Start()
	{

	}

	void OnTriggerEnter(Collider other)
	{

		// Om vi plockar upp vinnar-myntet...
		if (other.tag == "WinnerCoin")
		{
			coinCounter++;

			// Sätt igång vinn skärmen
			VictoryScreen.Show();
		}
	}
}

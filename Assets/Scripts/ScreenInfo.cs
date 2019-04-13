using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Detta script sätts på Canvas. Detta gör så att du kan se hur mycket liv du har kvar på skärmen samt hur många mynd du plockat upp.

public class ScreenInfo : MonoBehaviour {

	[Header("Player Scripts")]
	//Dessa två håller reda på vilka objekt som ska stå på skärmen. 
	[HideInInspector] public PlayerHealth health;
	[HideInInspector] public PlayerBag bag;

	[Header("Screen Items")]
	//Dessa två håller reda på att det ska stå en text på skärmen när spelet startar.
	public ScoreDisplay healthText;  
	public ScoreDisplay coinText;

	void Awake()
	{
		// Hämta referenser
		health = FindObjectOfType<PlayerHealth>();
		bag = FindObjectOfType<PlayerBag>();
	}

	void Update () {
		healthText.value = health.currentLives;
		coinText.value = bag.coinCounter;
	}

}

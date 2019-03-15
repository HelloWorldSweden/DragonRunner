using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Detta script ska sättas i ett child-objekt under Canvasen som heter "VictoryScreen".

// Denna gör så det kommer upp en meny när spelet är vunnet. 

public class VictoryScreen : MonoBehaviour {
    
    public Text coinText;  // För att kunna skriva ner hur många mynt spelaren plockade upp måste vi ha en text. 
	public GameObject nextLevelButton;

	void OnEnable() { // Från början är Objektet "VictoryScreen" inte aktiverat för att vi bara vill ha den när vi vinner.
        
		Cursor.lockState = CursorLockMode.None;  // Vi vill kunna använda musen fritt
        FindObjectOfType<ThirdPersonCamera>().enabled = false;  // Vi vill inte kunna röra spelaren

		var bag = FindObjectOfType<PlayerBag>();
        coinText.text = "Coins collected: " + bag.coinCounter;

		// Ha bara en "Next level" knapp om det finns en nivå efter denna
		int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
		bool nextValid = SceneManager.GetSceneByBuildIndex(nextIndex).IsValid();
		nextLevelButton.SetActive(nextValid);
	}

	public void Quit() {  // Om man trycker på "Quit" knappen på menyn stängs spelet ner.
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
         Application.Quit();
    #endif
    }

	public void Restart() { // Om man trycker på "Restart" knappen kommer samma bana vi nyss spelade köras igen. 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	public void NextLevel() { // Om man trycker på "Next Level" knappen kommer vi att köra nästa level i spelet (Detta går bara om vi har fler banor). 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

	public static void Show()
	{
		FindObjectOfType<VictoryScreen>().gameObject.SetActive(true);
		
		// Sätt igång partiklar, om det finns
		foreach (var go in GameObject.FindGameObjectsWithTag("WinnerCoin"))
			foreach (var ps in go.GetComponentsInChildren<ParticleSystem>())
				ps.Play();
	}

}


/* I projektet måste man gå in i "File" -> "Build Settings..." och i den tomma rutan dra in alla levels man byggt.
 * Det kommer stå en siffra bredvid levelnamnen i rutan. Den som är på noll är första level, den som är på ett är
 * nästa level osv. Så det är viktigt att sätta dem i rätt ordning.
 * Om detta inte görs kommer vi inte kunna gå vidare till nästa level när vi spelar spelet. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public VictoryScreen winScreen;

	public void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            winScreen.Show();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Detta script ska läggas på ett objekt som spelaren ska röra för att öppna dörren. 

public class OpenDoor : MonoBehaviour {

    Animator animator; // Gör att dörren har en rörelse.

    bool isOpen;  // En bool som håller koll på om dörren är öppen eller inte
    public bool isLocked = true;  // En bool som håller koll på om dörren är låst eller olåst. Från början sätter vi den till sant (för dörren är låst)

    public GameObject player; // Vi hämtar spelaren för att veta vilket objekt som ska kunna interagera med dörren.

    void Start() {
        //Vis start sätter vi igång animationen så dörren kan öppnas när vi ineragerar med objektet. 
        animator = transform.parent.GetComponentInChildren<Animator>();
    }


    void OnTriggerEnter(Collider other) {
        // Om det inte är spelaren som interagerar med objektet ska inget hända.
        if (other.gameObject != player) {
            return;
        }

        // Det nedanstående är det som alltså händer när det är spelaren som rör objektet (triggern som ska göra att dörren öppnas).

        if (isLocked == false || isLocked == true && other.GetComponent<PlayerBag>().hasKey) {
            isOpen = !isOpen;

            if (isOpen == true) {
                animator.SetTrigger("Open");
            }
            else {
                animator.SetTrigger("Close");
            }
        }
    }
}
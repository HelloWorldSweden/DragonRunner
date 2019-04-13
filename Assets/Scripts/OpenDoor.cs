using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detta script ska läggas på ett objekt som spelaren ska röra för att öppna dörren. 

public class OpenDoor : MonoBehaviour {

    Animator animator;

    bool isOpen;
    public bool isLocked = true;

    private static readonly int ANIM_OPEN = Animator.StringToHash("Open");
    private static readonly int ANIM_CLOSE = Animator.StringToHash("Close");

    void OnEnable() {
        animator = transform.parent.GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        // Det nedanstående är det som alltså händer när det är spelaren som rör objektet (triggern som ska göra att dörren öppnas).

        if (!isLocked || other.GetComponent<PlayerBag>().hasKey)
        {
            if (!isOpen)
            {
                isOpen = true;

                animator.SetTrigger(ANIM_OPEN);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!isLocked || other.GetComponent<PlayerBag>().hasKey)
        {
            if (isOpen)
            {
                isOpen = false;
                animator.SetTrigger(ANIM_CLOSE);
            }
        }
    }
}
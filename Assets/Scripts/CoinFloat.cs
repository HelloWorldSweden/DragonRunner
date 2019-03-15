using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinFloat : MonoBehaviour {

    public float rotateSpeed = 10;
    public float floatSpeed = 2;
    public float floatDist = 0.5f;

    private Vector3 startPos;
    private float floatDirection = 1;

    void Start() {
        startPos = transform.position;
    }

	// Update is called once per frame
	void Update () {
        transform.localEulerAngles += new Vector3(0, rotateSpeed, rotateSpeed) * Time.deltaTime;
        transform.position += new Vector3(0, floatSpeed, 0) * Time.deltaTime * floatDirection;

        if(Vector3.Distance(startPos, transform.position) > floatDist) {
            Vector3 temp = transform.position;
            temp.y = startPos.y + floatDist * floatDirection;
            transform.position = temp;
            floatDirection *= -1;
        }
	}
}

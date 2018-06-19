using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour {
	public GameObject target;
	public Vector3 offset;
	public float speed = 1f;
	// Update is called once per frame
	void FixedUpdate () {
		//transform.position = target.transform.position + offset; //It will focus only on player but not the enimes arrival
		transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, Time.deltaTime * speed);
	}
}

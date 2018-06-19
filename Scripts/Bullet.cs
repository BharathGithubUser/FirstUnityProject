using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float speed;
	private float lifeTimer;
	public float lifeDuration = 3f;
	public int damage = 1;
	// Use this for initialization
	void Start () {
		lifeTimer = lifeDuration;
	}

	// Update is called once per frame
	void Update () {
		//collider.isTrigger = false;
		//Time.deltaTime is used for behaving the bullet property in slower or even faster device constantly...
		transform.position += transform.forward * speed * Time.deltaTime;
		lifeTimer -= Time.deltaTime;
		if (lifeTimer <= 0) {
			Destroy (gameObject);
		}
				
	}
	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter");
		other.isTrigger = false;

	}
	public bool Triggered {
		get { return false; }
	}
}

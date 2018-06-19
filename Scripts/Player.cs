using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour {
	[Header("Player Settings")]
	public float playerHeight = 1f;
	public float movementSpeed = 5f;
	public float jumpForce = 100f;
	[Header("Effects")]
	public GameObject[] chargeEffects;
	[Header("Equipment")]
	/// <summary>
	/// This bulletPrefab is assigned with bullet prefab object in unity tools menu...thus it is initiated. 
	/// We need to delete the bullet 3d object from the Unity or else it will move away by default when you start the game.
	/// This will look untidy...So it's better to delete the 3D objects once we have derived that as prefabs
	/// </summary>
	public GameObject[] bulletPrefabs;
	public float chargeShotDuration = 5f;

	private Rigidbody playerRigidBody;
	private bool lookingRight;
	private float chargeShotTimer;

	// Use this for initialization
	void Start () {
		//The reference to "this" is optional...
		lookingRight = true;
		playerRigidBody = this.GetComponent<Rigidbody> (); 
		//Hide Charge effects.
		foreach (GameObject chargeEffect in chargeEffects) {
			chargeEffect.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () {
		//Horizontal Movement
		Vector3 targetVelocity = new Vector3 (0, playerRigidBody.velocity.y, playerRigidBody.velocity.z);
		if(Input.GetKey("right") || Input.GetKey("d")){
			lookingRight = true;
			targetVelocity = new Vector3 (movementSpeed, playerRigidBody.velocity.y, playerRigidBody.velocity.z);
		}
		if(Input.GetKey("left") || Input.GetKey("a")){
			lookingRight = false;
			targetVelocity = new Vector3 (-movementSpeed, playerRigidBody.velocity.y, playerRigidBody.velocity.z);
		}
		playerRigidBody.velocity = targetVelocity;
		//Jump logic...
		if(Input.GetKeyDown("up") || Input.GetKeyDown("w")) {
			//playerRigidBody.AddForce(Vector3.up * jumpForce); //This will work like jetpack jump
			RaycastHit hit;
			if(Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight / 2 + 1.00f)) {
				playerRigidBody.AddForce(Vector3.up * jumpForce);
			}
		}
		if (Input.GetKey ("space")) {
			//Remove the collider for sphere...or else the bullet will not follow Is Trigger.
			//If not removed...It will collide even if you cheked the Is Trigger Option.
			chargeShotTimer += Time.deltaTime;
			float strength = chargeShotTimer / chargeShotDuration;
			if (strength > 0.99f) {
				strength = 0.99f;
			}
			//.Length doesn't works in linux version of unity. So BruteForcing that logic...
			int bulletIndex = Mathf.FloorToInt(Length(bulletPrefabs) * strength);
			//Show the correct charge effect.
			for (int i = 0; i < Length (chargeEffects); i++) {
				chargeEffects [i].SetActive (i == bulletIndex);
			}
		} else {
			if (chargeShotTimer > 0) {
				float strength = chargeShotTimer / chargeShotDuration;
				if (strength > 0.99f) {
					strength = 0.99f;
				}
				//bulletPrefabs.Length doesn't works in linux version of unity. So BruteForcing that logic...
				int bulletIndex = Mathf.FloorToInt(Length(bulletPrefabs) * strength);
				shoot (bulletPrefabs[bulletIndex]);
				//Hide Charge effects.
				foreach (GameObject chargeEffect in chargeEffects) {
					chargeEffect.SetActive (false);
				}
				chargeShotTimer = 0;
			}
		}
	}
	private int Length(GameObject[] gameObject) {
		int Length = 0;
		foreach (GameObject items in bulletPrefabs)
		{
			Length++;
		}
		return Length;
	}
	private void shoot(GameObject bulletPrefab) {
		GameObject bulletObject = Instantiate (bulletPrefab);
		bulletObject.transform.SetParent (transform.parent);
		bulletObject.transform.position = transform.position;
		bulletObject.transform.forward = lookingRight ? Vector3.right : Vector3.left;
	}
}

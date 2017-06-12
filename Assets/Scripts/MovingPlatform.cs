using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	//class MovingPlatform
	/*
	public Vector3 MoveBy;
	Vector3 pointA;
	Vector3 pointB;

	// Use this for initialization
	void Start () {
		this.pointA = this.transform.position;
		this.pointB = this.pointA + MoveBy;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		Vector3 my_pos = this.transform.position;
		Vector3 target;
		if(going_to_a) {
			target = this.pointA;
		} else {
			target = this.pointB;
		}
		Vector3 destination = target - my_pos;
		destination.z = 0;

	}

	bool isArrived(Vector3 pos, Vector3 target) {
		pos.z = 0;
		target.z = 0;
		return Vector3.Distance(pos, target) < 0.02f;
	}

*/
	private Vector3 posA;
	private Vector3 posB;
	private Vector3 nextPos;

	[SerializeField]
	private float speed;



	[SerializeField]
	private Transform childTransform;

	[SerializeField]
	private Transform transformB;

	[SerializeField]
	private float time_to_wait;

	private float waitCounter;


	// Use this for initialization
	void Start () {
		posA = childTransform.localPosition;
		posB = transformB.localPosition;
		nextPos = posB;
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		Move();
	}


	private void Move() { 
		childTransform.localPosition = Vector3.MoveTowards (childTransform.localPosition, nextPos, speed * Time.deltaTime);
		isArrived ();

	}

	private void ChangeDestination() {
		
		nextPos = nextPos != posA ? posA : posB;
	}

	private void isArrived() {
		if (Vector3.Distance (childTransform.localPosition, nextPos) <= 0.1) {
			tToWait();
		}
	}

	private void tToWait() {
		float arrivalT = Time.realtimeSinceStartup;

		if (waitCounter <= time_to_wait) {
			waitCounter +=0.1f;	
		} else {
			waitCounter = 0;
			ChangeDestination();
		}
	}


}

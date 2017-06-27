
using System.Collections;
using System.Collections.Generic;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orc : MonoBehaviour {

	public Vector3 MoveBy;
	public float speed;
	public Collider2D colliderHead;
	public Collider2D colliderBody;

	ModeControler orcController = new ModeControler();
	Vector3 pointA;
	Vector3 pointB;
	Rigidbody2D orcBody;
	Vector3 orcPosition;
	bool moveToB = false;
	Animator orcAnimator;
	bool isDying = false;

	void Start () {
		this.pointA = this.transform.position;
		this.pointB = this.pointA + MoveBy;
		orcBody = this.GetComponent<Rigidbody2D> ();
		orcPosition = this.transform.position;
		orcAnimator = GetComponent<Animator> ();

		orcController.AddMode("moveToB", 1,
			() => {
				orcMove(1, true);
				orcAnimator.SetBool("walk", true);
				orcAnimator.SetBool("run", false);
			},
			() => {
				if( Vector3.Distance(orcPosition, pointB) < 0.2f) {
					moveToB = false;
				}
				return moveToB && !isDying;
			}
		);

		orcController.AddMode("moveToA", 1,
			() => {
				orcMove(-1, false);
				orcAnimator.SetBool("run", false);
				orcAnimator.SetBool("walk", true);
			},
			() => {
				if(Vector3.Distance(orcPosition, pointA) < 0.2f) {
					moveToB = true;
				}
				return !moveToB && !isDying;
			}
		);

		orcController.AddMode("attack", 2,
			() => {
				print("attack");
				Vector3 rabit_pos = HeroRabit.lastRabit.transform.position;
				if(orcPosition.x < rabit_pos.x) {
					orcMove(1, true);
				} else {
					orcMove(-1, false);
				}
				if(Math.Abs(orcPosition.x - rabit_pos.x) < 0.7)
					orcAnimator.SetTrigger("attack");
				orcAnimator.SetBool("walk", false);
				orcAnimator.SetBool("run", true);
			},
			() => {
				return isRabbitNear() && !isDying;
			}
		);
	}

	void Update () {

	}

	void FixedUpdate () {
		orcPosition = this.transform.position;
		if(orcController.getCurrentMode() != null)
			orcController.getCurrentMode().action();
	}

	public bool isRabbitNear () {
		Vector3 rabit_pos = HeroRabit.lastRabit.transform.position;
		if (rabit_pos.x > Mathf.Min (pointA.x, pointB.x)
			&& rabit_pos.x < Mathf.Max (pointA.x, pointB.x)) {
			return true;
		}
		return false;
	}

	void orcMove (int direction, bool flipX) {
		Vector2 vel = orcBody.velocity;
		vel.x = direction * speed;
		//without it orc stuck in a gorund
		vel.y = 0.1f;
		orcBody.velocity = vel;
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.flipX = flipX;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		HeroRabit rabbit = coll.gameObject.GetComponent<HeroRabit>();
		if(rabbit != null) {
			if(colliderBody.IsTouching(coll.collider)) {
				if (rabbit.mushroomMode) {
					rabbit.mushroomMode = false;
					rabbit.transform.localScale = new Vector3 (1.5f, 1.5f, 0);
				} else {
					rabbit.onDeath();
				}
			}
			else {
				orcAnimator.SetBool("walk", false);
				orcAnimator.SetBool("run", false);
				orcAnimator.ResetTrigger("attack1");
				orcAnimator.ResetTrigger("attack2");
				isDying = true;
				orcAnimator.SetTrigger("death");
			}
		}
	}

	public void onAnimationDie () {
		Destroy(this.gameObject);
	}

}

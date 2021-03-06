﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabit : MonoBehaviour {
	public static HeroRabit lastRabit;

	public float speed = 1;
	public float maxSpeed = 10;
	Rigidbody2D myBody = null;

	bool isGrounded = false;
	public bool isGhost = false;
	bool JumpActive = false;
	float JumpTime = 0f;
	public float MaxJumpTime = 2f;
	public float JumpSpeed = 2f;

	float deathAnimTimer = 1f;

	float timer;

	Animator anim;

	Transform heroParent = null;

	public bool mushroomMode = false;
	public bool death = false;

	//bool grounded = false;
	// Use this for initialization
	void Start () {
		
		myBody = this.GetComponent<Rigidbody2D> ();
		LevelController.current.setStartPosition (transform.position);
		this.heroParent = this.transform.parent;

		Vector3 rabit_pos = HeroRabit.lastRabit.transform.position;
	}
	
	// Update is called once per frame

	void Awake() {
		lastRabit = this;
	}

	void Update () {
		



		if (isGhost) {
			timer += Time.deltaTime;
		} else {
			timer = 0;
		}
		if (timer - 4.0f > 0f) {
			isGhost = false;
			GetComponent<SpriteRenderer> ().color = Color.white;
		}

		if (death) {
			deathAnimTimer += Time.deltaTime;
		} else {
			deathAnimTimer = 0;
		}

		if (deathAnimTimer - 1.0f > 0f) {
			death = false;
			LevelController.current.onRabitDeath(this);
		}

		anim = GetComponent<Animator>();

		if(this.isGrounded) {
			anim.SetBool ("jump", false);
		} else {
			anim.SetBool ("jump", true);
		}

		if (this.death) {
			anim.SetBool ("isDead", true);
		} else {
			anim.SetBool ("isDead", false);
		}

	}

	void FixedUpdate() {
		//[-1, 1]
		float value = Input.GetAxis ("Horizontal");

		anim.SetFloat ("speed", Mathf.Abs (value));

		if (Mathf.Abs (value) > 0) {
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
		}


		//class HeroRabit, void FixedUpdate()
		Vector3 from = this.transform.position + Vector3.up * 0.3f;
		Vector3 to = this.transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer ("Ground");
		//Перевіряємо чи проходить лінія через Collider з шаром Ground
		RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);

		if (hit)
		{

			if(hit.transform != null && hit.transform.GetComponent<Object>() != null){
				//Приліпаємо до платформи
				SetNewParent(this.transform, hit.transform);

			} else {
				//Ми в повітрі відліпаємо під платформи
				SetNewParent(this.transform, this.heroParent);
			}
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
			//Намалювати лінію (для розробника)
			Debug.DrawLine (from, to, Color.red);


			//Якщо кнопка тільки що натислась
			if (Input.GetButtonDown ("Jump") && isGrounded) {
				this.JumpActive = true;
			}
			if (this.JumpActive) {
				//meh
				//Якщо кнопку ще тримають
				if (Input.GetButton ("Jump")) {
					this.JumpTime += Time.deltaTime;
					if (this.JumpTime < this.MaxJumpTime) {
						Vector2 vel = myBody.velocity;
						vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
						myBody.velocity = vel;
					}
				} else {
					this.JumpActive = false;
					this.JumpTime = 0;
				}
			}



			SpriteRenderer sr = GetComponent<SpriteRenderer> ();
			if (value < 0) {
				sr.flipX = true;
			} else if (value > 0) {
				sr.flipX = false;
			}


	
		}



	static void SetNewParent(Transform obj, Transform new_parent) {
		if(obj.transform.parent != new_parent) {
			//Засікаємо позицію у Глобальних координатах
			Vector3 pos = obj.transform.position;
			//Встановлюємо нового батька
			obj.transform.parent = new_parent;
			//Після зміни батька координати кролика зміняться
			//Оскільки вони тепер відносно іншого об’єкта
			//повертаємо кролика в ті самі глобальні координати
			obj.transform.position = pos;
		}
	}



	//when mushroom
	public void changeSize(){
		if (!mushroomMode) {
			mushroomMode = true;
			transform.localScale = new Vector3 (2f, 2f, 0);
		}
	}

	public void bang() {
		if (mushroomMode) {
			mushroomMode = false;
			this.isGhost = true;
			transform.localScale = new Vector3 (1.5f, 1.5f, 0);
			GetComponent<SpriteRenderer> ().color = Color.red;
		} else {
			onDeath();

		}
	}

	public void onDeath() {
		death = true;

	}

	public bool IsGhost()
	{
		return this.isGhost;
	}








}

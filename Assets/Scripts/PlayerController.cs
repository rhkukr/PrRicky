using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Renderer renderer;
	public Rigidbody2D rigidbody;
	
	public float speed;
	public ArenaChecker arena;
	public LevelManager level;
	public TrailRenderer trail;
	public ParticleSystem particle;
	
	private bool hasCollided;
	private bool triggeredPasscheck;
	private Vector2 passCheckPos;

	private void Update() {
		trail.enabled = rigidbody.velocity.magnitude >= 12;
		var horizontal = Input.GetAxis("Horizontal");
		var move = horizontal * speed * Time.deltaTime;
		transform.Translate(new Vector3(move,0,0));

		if (!arena.CheckIfInView(renderer.bounds)) {
			if (transform.position.x < arena.leftBorder) {
				transform.position = new Vector3(arena.rightBorder, transform.position.y);
			} else if(transform.position.x > arena.rightBorder){
				transform.position = new Vector3(arena.leftBorder, transform.position.y);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if(hasCollided) return;
		if (other.gameObject.layer == 8) {
			hasCollided = true;
			rigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
			if(other.relativeVelocity.magnitude >= 12) level.triggerPassPlatform();
		}
		if (other.gameObject.layer == 9) {
			if(other.relativeVelocity.magnitude >= 12) level.triggerPassPlatform();
			else {
				hasCollided = true;
				particle.gameObject.transform.position = other.contacts[0].point;
				particle.Emit(10);
				gameObject.SetActive(false);
				level.TriggerDeath();
				triggeredPasscheck = false;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(triggeredPasscheck) return;
		if (other.gameObject.layer == 10) {
			triggeredPasscheck = true;
			passCheckPos = other.gameObject.transform.position;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if(!triggeredPasscheck) return;
		if (passCheckPos.y > gameObject.transform.position.y) {
			level.triggerPassPlatform();
			triggeredPasscheck = false;
		}
		
	}

	private void LateUpdate() {
		hasCollided = false;
	}
}

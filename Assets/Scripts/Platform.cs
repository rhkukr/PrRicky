using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	public ArenaChecker arena;
	public Renderer renderer;
	public Material[] mats;
	public Rigidbody2D rigidbody;
	public Collider2D collider2D;
		
	public bool isTrap;
	public bool isInView;

	public void Init(ArenaChecker _arena) {
		arena = _arena;
	}

	public void Spawn(bool trap, Vector2 position) {
		isTrap = trap;
		if (trap) {
			gameObject.layer = 9;
			renderer.material = mats[1];
		} else {
			gameObject.layer = 8;
			renderer.material = mats[0];
		}
		transform.position = position;
		transform.rotation = Quaternion.identity;
		rigidbody.isKinematic = true;
		collider2D.enabled = true;
		collider2D.isTrigger = false;
		renderer.enabled = true;
		isInView = false;
		gameObject.SetActive(true);
	}

	public void setAsPass() {
		isTrap = false;
		gameObject.layer = 10;
		collider2D.isTrigger = true;
		renderer.enabled = false;
	}
	
	public void DoUpdate() {
		if (isInView) {
			if (!arena.CheckIfInView(renderer.bounds)) gameObject.SetActive(false);
		} else {
			if (arena.CheckIfInView(renderer.bounds)) isInView = true;
		}
	}

	public void TriggerDeath() {
		collider2D.enabled = false;
		rigidbody.isKinematic = false;
		rigidbody.AddTorque(Random.Range(-100f, 100f));
	}
}

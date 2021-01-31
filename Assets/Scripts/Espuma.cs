using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espuma : MonoBehaviour {
	public float lifeTime;
	public float velocity;
	float bornTime;

	void Awake() {
		SoundManager.Play(SoundManager.instance.espuma);
	}

	public void Salir(Vector3 position, Vector3 direction, int espumaLevel) {
		bornTime = Time.time + 0.1f * espumaLevel;
		transform.position = position + direction * 0.5f;
		transform.localScale = Vector3.one + Vector3.one * 0.1f * espumaLevel;
		GetComponent<Rigidbody>().velocity = direction * velocity + Vector3.one * 0.1f * espumaLevel; ;

	}

	void Update() {
		if (Time.time - bornTime > lifeTime) {
			Destroy(gameObject);
		}
	}

	private void OnTriggerStay(Collider other) {
		FireA fire = other.GetComponent<FireA>();
		if (fire != null) {
			fire.Damage(1);
		}
	}



}

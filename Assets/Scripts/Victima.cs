using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victima : MonoBehaviour {

	void Awake() {
		Partida.partida.victimasTotales++;
		Transform randomChild = transform.GetChild(Random.Range(0, transform.childCount));
		randomChild.parent = null;
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
		randomChild.parent = transform;
	}

	void Update() {

	}

	public void Death() {
		SoundManager.Play(SoundManager.instance.victim);
		Partida.partida.victimasMuertas++;
		Partida.partida.analizarPartida();
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider collision) {
		Player player = collision.GetComponent<Player>();
		if (player != null) {
			SoundManager.Play(SoundManager.instance.rescatar);
			Partida.partida.victimasRescatadas++;
			Partida.partida.analizarPartida();
			Destroy(gameObject);
		}
	}
}

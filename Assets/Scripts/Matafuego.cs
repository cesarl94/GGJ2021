using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matafuego : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	private void OnTriggerEnter(Collider collision) {
		Player player = collision.GetComponent<Player>();
		if (player != null) {
			SoundManager.Play(SoundManager.instance.matafuegos);
			player.AddEspumaSize(1);
			Destroy(gameObject);
		}
	}
}

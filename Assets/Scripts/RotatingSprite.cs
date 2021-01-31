using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSprite : MonoBehaviour {
	public float velocity;

	void Update() {
		transform.RotateAround(transform.forward, velocity * Time.deltaTime * 60f);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
	public Transform followObject;

	void Update() {
		if (followObject != null) {
			transform.position = followObject.position + new Vector3(0, 0, -10);
		}

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espumata : MonoBehaviour {
	private void OnTriggerStay(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Default")) {
			Espuma padre = GetComponentInParent<Espuma>();
			if (padre != null) {
				Destroy(padre.gameObject);

			}
		}
	}
}

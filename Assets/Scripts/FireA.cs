using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireA : MonoBehaviour {
	public GameObject fireAPrefab;
	public int healt;
	bool multiplicandose;
	float lastCloneTime;
	List<FireA> childs;
	float spawnTime;
	[HideInInspector] FireA father;


	void Awake() {
		multiplicandose = false;
		childs = new List<FireA>();
		spawnTime = Constants.FIRE_MULTIPLICATION_TIME;
	}

	void Update() {
		if (Partida.partida.endLevel) return;
		if (!multiplicandose) {
			if (Vector3.SqrMagnitude(Player.player.transform.position - transform.position) < 100) {
				float distance = Vector3.Distance(Player.player.transform.position, transform.position);
				if (!Physics.Raycast(new Ray(transform.position, Player.player.transform.position - transform.position), distance * 1.1f, 1 << LayerMask.NameToLayer("Default"))) {
					multiplicandose = true;
					lastCloneTime = Time.time;
				}
				else return;


			}
			else return;
		}

		if (Time.time - lastCloneTime > spawnTime) {
			lastCloneTime = Time.time;
			for (int i = 0; i < 100; i++) {
				FireA childRandom = randomChild();
				Vector2 moving = new Vector2(Random.Range(1f, -1f), Random.Range(-1f, 1f)).normalized;
				Vector3 movedPosition = new Vector3(childRandom.transform.position.x + moving.x, childRandom.transform.position.y + moving.y, childRandom.transform.position.z);
				Vector3 halfExtends = GetComponent<BoxCollider>().size / 2f;
				if (!Physics.CheckBox(movedPosition, halfExtends, Quaternion.identity, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Matafuego"))) {
					GameObject newFire = Instantiate(fireAPrefab, transform.parent);
					FireA newFireA = newFire.GetComponent<FireA>();
					SoundManager.Play(SoundManager.instance.Fire);
					newFireA.fireAPrefab = null;
					newFireA.father = this;
					newFireA.enabled = false;
					newFire.transform.position = movedPosition;
					childs.Add(newFireA);
					newFire.transform.name += childs.Count.ToString();
					spawnTime *= 0.99f;
					if (spawnTime < Constants.FIRE_MULTIPLICATION_TIME * 0.2f) {
						spawnTime = Constants.FIRE_MULTIPLICATION_TIME * 0.2f;
					}
					transform.localScale = new Vector3(transform.localScale.x * 1.01f, transform.localScale.y * 1.01f, transform.localScale.z);
					if (transform.localScale.sqrMagnitude > 10) {
						transform.localScale = Vector3.one * 3.1622f;
					}
					newFireA.transform.localScale = Vector3.one;

					Collider[] collisions = Physics.OverlapBox(movedPosition, halfExtends, Quaternion.identity, 1 << LayerMask.NameToLayer("Victima"));
					foreach (Collider collider in collisions) {
						Victima victima = collider.GetComponent<Victima>();
						if (victima != null) {
							victima.Death();
						}
					}
					break;
				}
			}
		}
	}

	public void Damage(int count) {
		healt -= count;
		if (healt == 0) {
			if (father != null)
				father.childs.Remove(this);
			Destroy(gameObject);
		}
	}

	private FireA randomChild() {
		if (childs.Count == 0) return this;
		return childs[Random.Range(0, childs.Count - 1)];
	}

	private void OnTriggerEnter(Collider collision) {
		Player player = collision.GetComponent<Player>();
		if (player != null) {
			player.Damage(transform);
		}
	}
}

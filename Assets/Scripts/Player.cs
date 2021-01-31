using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour {
	public static Player player;
	[SerializeField] float velocity;
	[SerializeField] float blinkingDuration = 1f;
	[SerializeField] GameObject espumaPrefab;

	Rigidbody rb;
	Light light;

	public float darkness = 0;
	public float lightRadius = 1;
	public float reloadTime;
	public float healt = 10;
	public int delayCountMatafuego;
	public float stopTimeDuration;
	public int maximaDisponibilidadArma;


	private int espumaLevel;
	private Animation animation;
	private string horizontalAnim;
	private string verticalAnim;
	private string anim;
	private float lastBlinking;
	private float lastReload;
	private List<Vector3> lastDirections;
	private float stopTime;
	private float deathTime;
	[HideInInspector] public int disponibilidadActual;






	private void Awake() {
		Player.player = this;
		rb = GetComponent<Rigidbody>();
		animation = GetComponentInChildren<Animation>();
		Camera.main.GetComponent<FollowCamera>().followObject = transform;
		light = GetComponentInChildren<Light>();
		horizontalAnim = "right";
		verticalAnim = "";
		lastBlinking = -800;
		lastDirections = new List<Vector3>();
		espumaLevel = 0;
		darkness = 0.8f;
		lightRadius = 20;
		deathTime = Mathf.Infinity;
	}

	void Update() {

		if (healt <= 0 || Partida.partida.endLevel) {
			disponibilidadActual = Mathf.Max(disponibilidadActual - 1, 0);
			if (healt <= 0) {
				light.range *= 0.99f;
			}
			return;
		}
		Vector3 moveDirection = Vector3.zero;
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) moveDirection.y += 1f;
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) moveDirection.y -= 1f;
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) moveDirection.x -= 1f;
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) moveDirection.x += 1f;

		if (Time.time > stopTime) {
			if (moveDirection == Vector3.zero) {
				anim = "idle";
			}
			else {
				anim = "walk";
				if (Mathf.Abs(moveDirection.x) > 0.5f) {
					horizontalAnim = moveDirection.x > 0f ? "right" : "left";
				}
				else horizontalAnim = "";
				if (Mathf.Abs(moveDirection.y) > 0.5f) {
					verticalAnim = moveDirection.y > 0f ? "up" : "down";
				}
				else verticalAnim = "";
				moveDirection = moveDirection.normalized;
			}



			rb.velocity = moveDirection * velocity;

		}
		animation.setBlinking(Time.time - lastBlinking < blinkingDuration);
		animation.beginAnimation(anim + horizontalAnim + verticalAnim);

		if (Input.GetKey(KeyCode.Space)) {
			if (Time.time - lastReload > reloadTime && disponibilidadActual < maximaDisponibilidadArma) {
				lastReload = Time.time;
				Espuma espuma = Instantiate(espumaPrefab, transform).GetComponent<Espuma>();
				espuma.Salir(transform.position, getViewDirection(), espumaLevel);
				disponibilidadActual++;
			}
		}
		else {
			if (Time.time - lastReload > reloadTime) {
				lastReload = Time.time;
				getViewDirection();
				disponibilidadActual = Mathf.Max(disponibilidadActual - 1, 0);
			}
		}


		RenderSettings.ambientLight = new Color(1 - darkness, 1 - darkness, 1 - darkness);
		light.range = lightRadius;


	}

	public void AddEspumaSize(int count) {
		espumaLevel += count;
		disponibilidadActual = 0;
		maximaDisponibilidadArma += 10 * count;
	}

	public Vector3 getViewDirection() {
		float horizontal = horizontalAnim == "" ? 0f : horizontalAnim == "left" ? -1f : 1f;
		float vertical = verticalAnim == "" ? 0f : verticalAnim == "up" ? 1f : -1f;
		lastDirections.Add(new Vector3(horizontal, vertical, 0).normalized);
		if (lastDirections.Count > delayCountMatafuego) {
			lastDirections.Remove(lastDirections[0]);
		}
		Vector3 total = Vector3.zero;
		foreach (Vector3 direction in lastDirections) {
			total += direction;
		}
		return total / lastDirections.Count;
	}

	public void Damage(Transform origin) {
		lastBlinking = Time.time;
		Vector3 toMe = transform.position - origin.position;
		toMe = toMe.normalized;
		rb.velocity = Vector3.zero;
		rb.MovePosition(transform.position + toMe);
		stopTime = Time.time + stopTimeDuration * 5f;
		healt--;
		if (healt == 0) {
			Death();
		}
		else {
			SoundManager.Play(SoundManager.instance.danio);
		}

	}

	public void EndLevel() {
		animation.setBlinking(false);
		rb.velocity = Vector3.zero;
		animation.beginAnimation("idle" + horizontalAnim + verticalAnim);

	}

	void Death() {
		deathTime = Time.time;
		SoundManager.Play(SoundManager.instance.death);
		animation.beginAnimation("death");
		animation.setBlinking(false);
		rb.velocity = Vector3.zero;
		stopTime = Mathf.Infinity;

	}

	void Attack() {

	}

	private void OnCollisionEnter(Collision collision) {
		stopTime = Time.time + stopTimeDuration;
		rb.velocity = Vector3.zero;
		rb.MovePosition(transform.position + (transform.position - collision.transform.position).normalized * 0.1f);
	}
}

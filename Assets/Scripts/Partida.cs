using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Partida : MonoBehaviour {
	public static Partida partida;
	public int victimasTotales;
	public int victimasRescatadas;
	public int victimasMuertas;
	public bool endLevel;

	public TextMeshProUGUI victimasRestantesText;
	public TextMeshProUGUI disponibilidadArma;
	public TextMeshProUGUI salud;
	public TextMeshProUGUI perdiste;
	public TextMeshProUGUI ganaste;
	public Button reiniciarBoton;

	private void Awake() {
		Partida.partida = this;
		perdiste.gameObject.SetActive(false);
		ganaste.gameObject.SetActive(false);
		reiniciarBoton.gameObject.SetActive(false);

	}

	private void Update() {
		victimasRestantesText.text = "Vidas a salvar: " + (victimasTotales - victimasMuertas - victimasRescatadas).ToString();
		if (Player.player != null) {
			disponibilidadArma.text = "Matafuegos: " + (100 - Mathf.FloorToInt((float)Player.player.disponibilidadActual / (float)Player.player.maximaDisponibilidadArma * 100f)).ToString() + "%";
			salud.text = "Salud: " + (Mathf.Max(Player.player.healt * 10, 0)).ToString() + "%";

		}
		analizarPartida();
	}

	public void analizarPartida() {
		if (endLevel || Player.player == null) return;
		if (victimasMuertas + victimasRescatadas == victimasTotales) {

			ganaste.text = "¡Nivel completado!\n\nPersonas rescatadas:, " + victimasRescatadas.ToString() + "/" + victimasTotales.ToString() + ".";
			ganaste.gameObject.SetActive(true);
			Player.player.EndLevel();
			endLevel = true;
			SoundManager.Play(SoundManager.instance.win);
			reiniciarBoton.gameObject.SetActive(true);
			disponibilidadArma.gameObject.SetActive(false);
			salud.gameObject.SetActive(false);
			return;
		}
		if (Player.player.healt == 0) {
			perdiste.text = "¡Has muerto!\n\nPero no estas solo, " + (victimasTotales - victimasRescatadas).ToString() + " vidas tambien se van contigo.";
			perdiste.gameObject.SetActive(true);
			endLevel = true;
			SoundManager.Play(SoundManager.instance.win);
			reiniciarBoton.gameObject.SetActive(true);
			disponibilidadArma.gameObject.SetActive(false);
			salud.gameObject.SetActive(false);
		}
	}

	public void reiniciarPartida() {
		SceneManager.LoadScene(0);
	}
}

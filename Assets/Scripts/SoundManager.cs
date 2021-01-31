using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	AudioSource source;
	public static SoundManager instance;

	public AudioClip danio;
	public AudioClip death;
	public AudioClip espuma;
	public AudioClip Fire;
	public AudioClip matafuegos;
	public AudioClip rescatar;
	public AudioClip win;
	public AudioClip victim;
	public AudioClip music;

	private AudioSource source2;

	void Awake() {
		source = GetComponent<AudioSource>();
		enabled = false;
		SoundManager.instance = this;
		Play(music);

	}

	public static void Play(AudioClip clip, float volume = 1f) {
		instance.source.PlayOneShot(clip, volume);
	}

	//public static void PlayLoop(AudioClip clip, float volume = 1f) {
	//	instance.source.PlayOneShot(clip, volume);
	//}

	//public static void Stop(AudioClip clip, float volume = 1f) {
	//	instance.source.PlayOneShot(clip, volume);
	//}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Animat {
	public string name;
	public int[] indexs;
}

public class Animation : MonoBehaviour {
	[SerializeField] Animat[] animations;
	[SerializeField] int animationsRows;
	[SerializeField] int animationsColumns;
	[SerializeField] float frameDuration;
	[SerializeField] string initAnimation;
	private Animat currentAnimation;
	private int currentFrameID;
	Dictionary<string, Animat> animations2;
	private float lastAnimationBegin;
	private bool blinking;


	Material material;

	void Awake() {
		material = GetComponent<Renderer>().material;
		animations2 = new Dictionary<string, Animat>();
		foreach (Animat anim in animations) {
			animations2.Add(anim.name, anim);
		}

		material.mainTextureScale = new Vector2(1f / animationsColumns, 1f / animationsRows);
		blinking = false;

		beginAnimation(initAnimation);
	}

	void Update() {
		float timeDiference = Time.time - lastAnimationBegin;
		int floor = Mathf.FloorToInt(timeDiference / frameDuration);
		int frameID = floor % currentAnimation.indexs.Length;
		if (currentAnimation.name == "death" && floor >= currentAnimation.indexs.Length) {
			frameID = currentAnimation.indexs.Length - 1;
		}
		updateFrame(blinking ? floor % 2 == 1 ? 99999999 : currentAnimation.indexs[frameID] : currentAnimation.indexs[frameID]);
	}

	public void beginAnimation(string name) {
		if (!animations2.ContainsKey(name) || name == currentAnimation.name) return;
		if (name == "death") {
			frameDuration *= 3;
		}
		currentAnimation = animations2[name];
		lastAnimationBegin = Time.time;
	}

	public void setBlinking(bool value) {
		blinking = value;
	}

	void updateFrame(int f) {
		int i = f % animationsColumns;
		int j = animationsRows - (f / animationsColumns);
		material.mainTextureOffset = new Vector2(material.mainTextureScale.x * i, material.mainTextureScale.y * (j - 1));
	}
}

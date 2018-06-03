using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSounds : MonoBehaviour {

    public AudioClip[] pewPews;
    private AudioSource myAudioSource;
	// Use this for initialization
	void Start () {
        myAudioSource = GetComponent<AudioSource>();
	}
	
	public void PlayPewPew()
    {
        myAudioSource.Stop();
        myAudioSource.clip = pewPews[Random.Range(0, pewPews.Length)];
        myAudioSource.Play();
    }
}

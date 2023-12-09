using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class rock_free_master : MonoBehaviour {
	
	public AudioMixer rock_mixer;
	
	private Object[] AudioArray_loops;
	
	public float fadeout_speed = 20.0f;
	public float fadein_speed = 100.0f;
	
	private AudioSource audio_loop1;
	private AudioSource audio_loop2;
	private AudioSource audio_loop3;
	
	private float audio_soft_vol;
	private float audio_med_vol;
	private float audio_forte_vol;
	
	private bool soft_isPlaying;
	private bool med_isPlaying;
	private bool forte_isPlaying;
	
	public bool soft;
	public bool med;
	public bool forte;

	
	public float bpm = 135.0F;
	public int beatsPerMeasure = 4;
	private double singleMeasureTime;
	private double delayEvent;
	private bool running = false;
	private int i;
	
	private bool first_run;
	
	// Use this for initialization
	void Start () {
		first_run = false;
		bpm = 135.0F;
		beatsPerMeasure = 4;
		int i = 0;
		singleMeasureTime = AudioSettings.dspTime + 2.0F;
		running = true;
		
		
		audio_loop1 = (AudioSource)gameObject.AddComponent <AudioSource>();
		audio_loop2 = (AudioSource)gameObject.AddComponent <AudioSource>();
		audio_loop3 = (AudioSource)gameObject.AddComponent <AudioSource>();
		
		audio_loop1.outputAudioMixerGroup = rock_mixer.FindMatchingGroups("soft")[0];
		audio_loop2.outputAudioMixerGroup = rock_mixer.FindMatchingGroups("med")[0];
		audio_loop3.outputAudioMixerGroup = rock_mixer.FindMatchingGroups("forte")[0];
		AudioArray_loops = Resources.LoadAll("rock",typeof(AudioClip));
		
		audio_loop1.clip = AudioArray_loops [0] as AudioClip;
		audio_loop2.clip = AudioArray_loops [1] as AudioClip;
		audio_loop3.clip = AudioArray_loops [2] as AudioClip;
		audio_loop1.loop = true;
		audio_loop2.loop = true;
		audio_loop3.loop = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		rock_mixer.SetFloat ("soft", audio_soft_vol);
		rock_mixer.SetFloat ("med", audio_med_vol);
		rock_mixer.SetFloat ("forte", audio_forte_vol);
		
		
		if (!first_run) {
			if (!running)
				return;
			double time = AudioSettings.dspTime;
			
			
			if (i == 0) {			
				if (time + 1.0F > singleMeasureTime) {
					if (soft & !soft_isPlaying){
						audio_soft_vol = 0.0f;
						audio_loop1.Play ();
						
						soft_isPlaying = true;
						med_isPlaying = false;
						forte_isPlaying = false;
						
					}
					if (med & !med_isPlaying){
						audio_med_vol = 0.0f;
						audio_loop2.Play ();
						audio_soft_vol = -80.0f;
						soft_isPlaying = false;
						med_isPlaying = true;
						forte_isPlaying = false;
					}
					if (forte & !forte_isPlaying){
						audio_forte_vol = 0.0f;
						audio_loop3.Play ();
						audio_soft_vol = -80.0f;
						soft_isPlaying = false;
						med_isPlaying = false;
						forte_isPlaying = true;
					}
					
					
				}
			}
			if (!forte) {
				if (med_isPlaying | soft_isPlaying){
					audio_forte_vol -= fadeout_speed * Time.deltaTime;	
					if (audio_forte_vol < -70.0f){
						audio_loop3.Stop ();
					}
				}
				
			}
			if (!med) {
				if (forte_isPlaying | soft_isPlaying){
					audio_med_vol -= 60.0f * Time.deltaTime;	
					if (audio_med_vol < -70.0f){
						audio_loop2.Stop ();
					}
				}
				
			}
			
			//THE most important part of this script: this is the metronome, keeping count of the measures and making sure the audio is in sync
			if (time + 1.0F > singleMeasureTime) {
				i +=1;
				//Debug.Log ("The i int equals  " + i);
				if (i==4){
					i = 0;
				}
				singleMeasureTime += 60.0F / bpm * beatsPerMeasure;
				//Debug.Log("The single measure time is " + singleMeasureTime);
			}
		}
		
		
		if (!soft & !med & !forte) {
			soft_isPlaying = false;
			med_isPlaying = false;
			forte_isPlaying = false;
			if (audio_forte_vol > -80.0f) {
				audio_forte_vol -= fadeout_speed * Time.deltaTime;	
			}
			if (audio_forte_vol < -70.0f) {
				audio_loop3.Stop();
				first_run = true;
			}
			if (audio_med_vol > -80.0f) {
				audio_med_vol -= fadeout_speed * Time.deltaTime;	
			}
			if (audio_med_vol < -70.0f) {
				audio_loop2.Stop();
				first_run = true;
			}
			if (audio_soft_vol > -80.0f) {
				audio_soft_vol -= fadeout_speed * Time.deltaTime;	
			}
			if (audio_soft_vol < -70.0f) {
				audio_loop1.Stop();
				first_run = true;
			}
			
		}
		
		
		
	}
	
	public void Soft_OnClick(){
		soft = true;
		med = false;
		forte = false;
		if (first_run) {
			i = -1;
			if (soft){
				audio_soft_vol = 0.0f;
				audio_med_vol = -80.0f;
				audio_forte_vol = -80.0f;
			}
			if (med){
				audio_soft_vol = -80.0f;
				audio_med_vol = 0.0f;
				audio_forte_vol = -80.0f;
			}
			if (forte){
				audio_forte_vol = 0.0f;
				audio_med_vol = -80.0f;
				audio_soft_vol = -80.0f;
			}
			first_run = false;
		}
		
	}
	
	public void Med_OnClick(){
		soft = false;
		med = true;
		forte = false;
		if (first_run) {
			i = -1;
			if (soft){
				audio_soft_vol = 0.0f;
				audio_med_vol = -80.0f;
				audio_forte_vol = -80.0f;
			}
			if (med){
				audio_soft_vol = -80.0f;
				audio_med_vol = 0.0f;
				audio_forte_vol = -80.0f;
			}
			if (forte){
				audio_forte_vol = 0.0f;
				audio_med_vol = -80.0f;
				audio_soft_vol = -80.0f;
			}
			first_run = false;
		}
		
	}
	
	public void Forte_OnClick(){
		soft = false;
		med = false;
		forte = true;
		if (first_run) {
			i = -1;
			if (soft){
				audio_soft_vol = 0.0f;
				audio_med_vol = -80.0f;
				audio_forte_vol = -80.0f;
			}
			if (med){
				audio_soft_vol = -80.0f;
				audio_med_vol = 0.0f;
				audio_forte_vol = -80.0f;
			}
			if (forte){
				audio_forte_vol = 0.0f;
				audio_med_vol = -80.0f;
				audio_soft_vol = -80.0f;
			}
			first_run = false;
		}
		
	}
	public void Stop_OnClick(){
		soft = false;
		med = false;
		forte = false;
		if (first_run) {
			i = -1;
			if (soft){
				audio_soft_vol = 0.0f;
				audio_med_vol = -80.0f;
				audio_forte_vol = -80.0f;
			}
			if (med){
				audio_soft_vol = -80.0f;
				audio_med_vol = 0.0f;
				audio_forte_vol = -80.0f;
			}
			if (forte){
				audio_forte_vol = 0.0f;
				audio_med_vol = -80.0f;
				audio_soft_vol = -80.0f;
			}
			first_run = false;
		}
		
	}

}

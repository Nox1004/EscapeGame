﻿using UnityEngine;
using System.Collections;

public class fire_wiggle : MonoBehaviour {
	private float t=0f;
	private float wiggle_t=0f;
	public float fire_k=1f;
	private float initial_start_speed;
	private float initial_emission_rate;
	private float initial_lifetime;
	private float initial_size;
	//private Vector3 initial_position;
	private float randomizer=0f;
	// Use this for initialization
	
	void Start () {
		randomizer = Random.Range(.75f,1.25f);//making each flame have burst randomly
		//initial_start_speed=this.GetComponent<ParticleSystem>().startSpeed;//saving initial flame properties
    //initial_emission_rate=this.GetComponent<ParticleSystem>().emissionRate;
		initial_start_speed = ParticleSystemExtension.GetParticleSpeed(this.GetComponent<ParticleSystem>());
	initial_emission_rate =	ParticleSystemExtension.GetEmissionRate(this.GetComponent<ParticleSystem>());
	initial_lifetime = ParticleSystemExtension.GetParticleLifetime(this.GetComponent<ParticleSystem>());
	//initial_lifetime = this.GetComponent<ParticleSystem>().startLifetime;
	//initial_size = this.GetComponent<ParticleSystem>().startSize;
	initial_size = ParticleSystemExtension.GetParticleSize(this.GetComponent<ParticleSystem>());
	//initial_position = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		t+=Time.deltaTime*randomizer;
		wiggle_t+=Time.deltaTime*randomizer;
		
		//creating bursts of fire to make it more physically  realistic-->
		if (t>(2f+(2f-Mathf.Sin(wiggle_t)))){
			
		
		
		//this.GetComponent<ParticleSystem>().emissionRate+=(initial_emission_rate*.4f*fire_k-this.GetComponent<ParticleSystem>().emissionRate)/30f;
			ParticleSystemExtension.SetEmissionRate(this.GetComponent<ParticleSystem>(),ParticleSystemExtension.GetEmissionRate(this.GetComponent<ParticleSystem>()) + (initial_emission_rate*.4f*fire_k-ParticleSystemExtension.GetEmissionRate(this.GetComponent<ParticleSystem>()))/30f );
			ParticleSystemExtension.SetParticleLifetime(this.GetComponent<ParticleSystem>(),ParticleSystemExtension.GetParticleLifetime(this.GetComponent<ParticleSystem>()) + (initial_lifetime*.9f*fire_k-ParticleSystemExtension.GetParticleLifetime(this.GetComponent<ParticleSystem>()))/30f );

			//this.GetComponent<ParticleSystem>().startLifetime+=(initial_lifetime*.9f*fire_k-this.GetComponent<ParticleSystem>().startLifetime)/30f;


			if (ParticleSystemExtension.GetEmissionRate(this.GetComponent<ParticleSystem>())<initial_emission_rate*.42f*fire_k){
				
				ParticleSystemExtension.SetEmissionRate(this.GetComponent<ParticleSystem>(), initial_emission_rate*1.1f*fire_k);
				ParticleSystemExtension.SetParticleLifetime(this.GetComponent<ParticleSystem>(),initial_lifetime*1.1f*fire_k);
				ParticleSystemExtension.SetParticleSpeed(this.GetComponent<ParticleSystem>(),initial_start_speed*.7f*fire_k);
				ParticleSystemExtension.SetParticleSize(this.GetComponent<ParticleSystem>(), initial_size*1.1f*fire_k);
				randomizer = Random.Range(.75f,1.25f);
				t=0f;
			}
		} else{

			ParticleSystemExtension.SetEmissionRate(this.GetComponent<ParticleSystem>(), ParticleSystemExtension.GetEmissionRate(this.GetComponent<ParticleSystem>())+ (initial_emission_rate-ParticleSystemExtension.GetEmissionRate(this.GetComponent<ParticleSystem>()))/30f);
			ParticleSystemExtension.SetParticleLifetime(this.GetComponent<ParticleSystem>(),ParticleSystemExtension.GetParticleLifetime(this.GetComponent<ParticleSystem>())+ (initial_lifetime-ParticleSystemExtension.GetParticleLifetime(this.GetComponent<ParticleSystem>()))/100f);
			ParticleSystemExtension.SetParticleSpeed(this.GetComponent<ParticleSystem>(), ParticleSystemExtension.GetParticleSpeed(this.GetComponent<ParticleSystem>()) + (initial_start_speed-ParticleSystemExtension.GetParticleSpeed(this.GetComponent<ParticleSystem>()))/30f);
			ParticleSystemExtension.SetParticleSize(this.GetComponent<ParticleSystem>(),ParticleSystemExtension.GetParticleSize(this.GetComponent<ParticleSystem>()) +(initial_size-ParticleSystemExtension.GetParticleSize(this.GetComponent<ParticleSystem>()))/30f);


		}


	/*
			if (this.GetComponent<ParticleSystem>().emissionRate<initial_emission_rate*.42f*fire_k){
				this.GetComponent<ParticleSystem>().emissionRate = initial_emission_rate*1.1f*fire_k;
					this.GetComponent<ParticleSystem>().startLifetime=initial_lifetime*1.1f*fire_k;
				this.GetComponent<ParticleSystem>().startSpeed=initial_start_speed*.7f*fire_k;
					this.GetComponent<ParticleSystem>().startSize= initial_size*1.1f*fire_k;
				randomizer = Random.Range(.75f,1.25f);
				t=0f;
			}
		} else{
		this.GetComponent<ParticleSystem>().emissionRate+=(initial_emission_rate-this.GetComponent<ParticleSystem>().emissionRate)/30f;
			this.GetComponent<ParticleSystem>().startLifetime+=(initial_lifetime-this.GetComponent<ParticleSystem>().startLifetime)/100f;
			this.GetComponent<ParticleSystem>().startSpeed+=(initial_start_speed-this.GetComponent<ParticleSystem>().startSpeed)/30f;
			this.GetComponent<ParticleSystem>().startSize+=(initial_size-this.GetComponent<ParticleSystem>().startSize)/30f;
				
			
		}
		*/
	}
}

﻿using UnityEngine;
using System.Collections;

public class SinWaveMover : MonoBehaviour {
	public bool isStart = false;
	void Start()
	{
		m_centerPosition = transform.position;
		Invoke ("StartMoving",Random.Range(1,5)*0.75f);
	}
	void StartMoving(){
		isStart = true;
	}
	void Update()
	{
		if (!isStart) {
			return;
		}
		float deltaTime = Time.deltaTime;

		// Move center along x axis
		//m_centerPosition.x += deltaTime * m_speed;

		// Update degrees
		float degreesPerSecond = 360.0f / m_period;
		m_degrees = Mathf.Repeat(m_degrees + (deltaTime * degreesPerSecond), 360.0f);
		float radians = m_degrees * Mathf.Deg2Rad;

		// Offset by sin wave
		Vector3 offset = new Vector3(0.0f, m_amplitude * Mathf.Sin(radians), 0.0f);
		transform.position = m_centerPosition + offset;
	}

	Vector3 m_centerPosition;
	float m_degrees;

	[SerializeField]
	float m_speed = 1.0f;

	[SerializeField]
	float m_amplitude = 1.0f;

	[SerializeField]
	float m_period = 1.0f;
}

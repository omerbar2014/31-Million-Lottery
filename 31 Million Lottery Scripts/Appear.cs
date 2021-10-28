using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appear : MonoBehaviour
{
	public Material material;

	public bool isAppear = false;
	float fade = 0f;

	void Start()
	{
		// Get a reference to the material
		material = GetComponent<SpriteRenderer>().material;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			isAppear = true;
		}

		if (isAppear)
		{
			fade += Time.deltaTime;

			if (fade >= 1f)
			{
				fade = 1f;
				isAppear = false;
			}

			// Set the property
			material.SetFloat("_Fade", fade);
		}
		else fade = 0f;
	}
}
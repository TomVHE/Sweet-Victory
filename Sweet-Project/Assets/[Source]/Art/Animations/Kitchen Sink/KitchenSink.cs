using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenSink : MonoBehaviour
{
	public GameObject particle;
	public float speed = 0.1f;

	private Animator _animator;
	private bool _up;
	private float _blend;

	private void Start()
	{
		_animator = GetComponent<Animator>();

		_up = true;
		_animator.SetFloat("Blend", 1f);
	}
	
	private void Update()
	{
		float _time = (_up) ? Time.deltaTime : -Time.deltaTime;
		_blend = Mathf.Clamp01(_animator.GetFloat("Blend") + (_time * speed));
		if(_blend > 0.99f && _up)
		{
			particle.SetActive(false);
		}
		_animator.SetFloat("Blend", _blend);

		if(_blend > 0.99f)
		{
			ChangeHeight(false);
		}
		else if (_blend < 0.01f)
		{
			ChangeHeight(true);
		}
	}

	private void ChangeHeight(bool _goUp)
	{
		_up = _goUp;
		if(particle != null)
		{
			particle.SetActive(_up);
		}
	}
}

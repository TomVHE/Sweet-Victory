using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimer : Singleton<RoundTimer> 
{


	//connect with gamemanager
	
	//scripts
	public UIManager uimanager;

	//timer
	public int minutes;	//can edit this in inspector
	private float second;
	private int minute;
	public bool timerActive;

	


	// Use this for initialization
	void Start () 
	{
		StartTimer();
	}
	void Update () 
	{
		if(timerActive)
		{
			Timer();
		}
	}
	//sets timer
	public void StartTimer ()
	{
		second = 0;
		minute = minutes;
	}
	//does the timing
	public void Timer ()
	{
        second -= Time.deltaTime;
		
		if(second < 0)
		{
			if(minute > 0)
			{
				minute--;
				second = 59;
			}
			if(minute <= 0 && second <= 0) //Tom
			{
				EndMatch();
			}
		}
		SendInfo();
	}
	//when the match ends
	public void EndMatch () 
	{
		Debug.Log("Stop match");
	}
	//sends info to uimanager and updates it
	public void SendInfo ()
	{
		uimanager.UpdateTimer(minute,second);
	}
}

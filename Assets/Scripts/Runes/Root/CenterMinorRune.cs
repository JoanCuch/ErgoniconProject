using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMinorRune : MinorRune
{

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		SetWorkable(true);

		
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		ChangeWorkState(true);
	}


	private void OnDestroy()
	{
		ChangeWorkState(false);
	}

	private void ChangeWorkState(bool _newState)
	{

		List<RuneSorted> list = GetMajorRune().GetAllMinorRunes();

		foreach(RuneSorted rune in list)
		{
			rune.runeScript.SetWorkable(_newState);
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorRune : MonoBehaviour
{

	public enum RuneTypes { source, transformation, complement, basic}

	[SerializeField]private RuneTypes runeType;

	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public virtual void DoYourAction()
	{
		Debug.LogWarning("This minor rune is doing nothing");
	}

	public RuneTypes GetRuneType()
	{
		return runeType;
	}


}

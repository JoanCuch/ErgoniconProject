using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : EnergyInteractable
{

	private List<MajorRune> attachedRunes;

    // Start is called before the first frame update
    void Start()
    {
		attachedRunes = new List<MajorRune>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public bool hasRune()
	{
		bool _hasRune = false;

		if (attachedRunes.Count != 0)
			_hasRune = true;

		return _hasRune;
	}


	public void AttachMajorRune(Transform _newRune)
	{
		_newRune.SetParent(transform);
		_newRune.position = transform.position + Vector3.up;
		attachedRunes.Add(_newRune.GetComponent<MajorRune>());
	}
	public void RemoveMajorRune(Transform _oldRune)
	{
		attachedRunes.Remove(_oldRune.GetComponent<MajorRune>());
		Destroy(_oldRune.gameObject);

	}
}

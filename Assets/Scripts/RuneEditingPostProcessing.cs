using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class RuneEditingPostProcessing : MonoBehaviour
{

	[SerializeField] private PostProcessVolume volume;
	private ColorGrading gray = null;

    // Start is called before the first frame update
    void Start()
    {
		volume.profile.TryGetSettings<ColorGrading>(out gray);
		gray.active = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void RuneEditingEffectSetActive(bool _active)
	{
		gray.active = _active;
	}
}

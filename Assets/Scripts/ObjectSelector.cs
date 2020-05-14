using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
	[SerializeField] private Transform target;

	[SerializeField] private string leftIndexFingerTag;
	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {	
    }

	public (Transform transform, RaycastHit hit) SelectTarget(GameObject _indexFinger)
	{
		Transform hitObject = null;
		Transform indexFinger = _indexFinger.transform;

		RaycastHit hit;
		Ray ray;


		Vector3 direction = indexFinger.right;

		if (_indexFinger.tag == leftIndexFingerTag) direction *= -1;

		ray = new Ray(indexFinger.position, Vector3.Normalize(direction));

		


		if (Physics.Raycast(ray, out hit))
		{
			hitObject = hit.collider.transform;
			Debug.Log("Detected object: " + hitObject.name);
		}

		return (hitObject, hit);
	}



}

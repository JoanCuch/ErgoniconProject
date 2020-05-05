using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHighlight : MonoBehaviour
{
	//transform postion
	//transform rotation
	[SerializeField] private MeshRenderer meshRender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetParent(Transform _newParent)
	{
		this.transform.parent = _newParent;
		transform.position = transform.parent.position;
		transform.rotation = transform.parent.rotation;
		transform.localScale = Vector3.one;

		transform.parent.GetComponent<MeshRenderer>();


	}



}

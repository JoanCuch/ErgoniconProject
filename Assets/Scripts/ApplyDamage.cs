using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.SendMessageUpwards("ApplyDamage", SendMessageOptions.DontRequireReceiver);
	}
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class JoystickController : MonoBehaviour
{
	public float MaxDistance;
	public float MaxDuration;

	private RawImage sJoystickHead;
	private EasingFunction.EasingFunc fEasingFunc;

	private void Awake()
	{
		this.sJoystickHead = this.gameObject.GetComponent<RawImage>();
		this.fEasingFunc = EasingFunction.Linear;
	}

	private void OnMouseDown()
	{

	}

	private void OnMouseUp()
	{
		this.StartCoroutine(this.smoothReset());
	}

	private IEnumerator smoothReset()
	{
		
	}
}
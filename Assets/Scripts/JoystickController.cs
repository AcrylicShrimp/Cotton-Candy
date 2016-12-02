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
		if(this.transform.localPosition.x != 0f || this.transform.localPosition.y != 0f)
			this.StartCoroutine(this.smoothReset());
	}

	private IEnumerator smoothReset()
	{
		Vector2 sCurrentPos = this.transform.localPosition;
		float nDuration = sCurrentPos.magnitude / this.MaxDistance;
		float nElapsed = 0f;

		while((nElapsed += Time.deltaTime) <= nDuration)
		{
			sCurrentPos.x = this.fEasingFunc(sCurrentPos.x, 0f, nElapsed / nDuration);
			sCurrentPos.y = this.fEasingFunc(sCurrentPos.y, 0f, nElapsed / nDuration);

			this.transform.localPosition = sCurrentPos;

			yield return null;
		}

		this.transform.localPosition = Vector2.zero;
	}
}
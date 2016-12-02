using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class JoystickController : MonoBehaviour
{
	public Vector2 Velocity { get { return this.transform.localPosition / this.MaxDistance; } }

	public float MaxDistance;
	public float MaxDuration;
	public CanvasScaler UICanvasScaler;
	
	private EasingFunction.EasingFunc fEasingFunc;
	private Vector2 sLastMousePosition;
	private float nScaleFactor;

	private void Awake()
	{
		this.fEasingFunc = EasingFunction.Quintic.Out;
		this.nScaleFactor = this.UICanvasScaler.referenceResolution.x / Screen.width;
	}

	public void OnMouseDown()
	{
		this.StopAllCoroutines();
		this.sLastMousePosition = Input.mousePosition;
	}

	public void OnMouseDrag()
	{
		Vector2 sMousePosition = Input.mousePosition;
		Vector2 sDeltaPosition = (sMousePosition - this.sLastMousePosition) * this.nScaleFactor;
		Vector2 sCurrentPosition = this.transform.localPosition;

		sCurrentPosition += sDeltaPosition;

		if (sCurrentPosition.magnitude >= this.MaxDistance)
		{
			sCurrentPosition.Normalize();
			sCurrentPosition *= this.MaxDistance;
		}

		this.transform.localPosition = sCurrentPosition;
		this.sLastMousePosition = sMousePosition;
	}

	public void OnMouseUp()
	{
		if (this.transform.localPosition.x != 0f || this.transform.localPosition.y != 0f)
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
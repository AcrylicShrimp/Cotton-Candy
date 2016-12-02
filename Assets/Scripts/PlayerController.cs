using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	public float MaxSpeed;
	public Sprite IdleSprite;
	public JoystickController Joystick;

	[Header("Attack")]
	public float AttackDelay;
	public float AttackDelaySPF;
	public Sprite[] AttackDelayAnimation;

	public float AttackSPF;
	public Sprite[] AttackAnimation;

	public float AttackRange;
	public Sprite AttackSprite;

	private RawImage sRawImage;
	private bool bAttacking = false;

	private void Awake()
	{
		this.sRawImage = this.gameObject.GetComponent<RawImage>();
	}

	private void Update()
	{
		Vector2 sMovement = this.transform.localPosition;
		sMovement += this.Joystick.Velocity * this.MaxSpeed * Time.deltaTime;

		this.transform.localPosition = sMovement;
	}

	public void OnAttack()
	{
		/*
		if(!this.bAttacking)
		{
			this.bAttacking = true;
			this.StartCoroutine(this.DoAttack());
		}
		*/
		var vCollider = Physics2D.OverlapCircleAll(this.transform.localPosition, this.AttackRange);

		foreach (var sCollider in vCollider)
			if (sCollider.tag == "Monster")
				GameObject.Destroy(sCollider.gameObject);
	}

	private IEnumerator DoAttack()
	{
		float nElapsed = 0f;
		int nSpriteIndex = 0;
		var sWaitFrame = new WaitForSeconds(this.AttackDelaySPF);

		while ((nElapsed += Time.deltaTime) <= this.AttackDelay)
		{
			this.sRawImage.material.mainTexture = this.AttackDelayAnimation[nSpriteIndex++].texture;
			this.sRawImage.SetMaterialDirty();

			if (nSpriteIndex >= this.AttackDelayAnimation.Length)
				nSpriteIndex = 0;

			yield return sWaitFrame;
		}

		//TODO : Do attack here.

		nSpriteIndex = 0;
		sWaitFrame = new WaitForSeconds(this.AttackSPF);

		while (nSpriteIndex < this.AttackAnimation.Length)
		{
			this.sRawImage.material.mainTexture = this.AttackAnimation[nSpriteIndex++].texture;
			this.sRawImage.SetMaterialDirty();

			yield return sWaitFrame;
		}

		this.sRawImage.material.mainTexture = this.IdleSprite.texture;
		this.sRawImage.SetMaterialDirty();

		this.bAttacking = false;
	}
}
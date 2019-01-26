using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Revealable : MonoBehaviour
{
	private IEnumerable<SpriteRenderer> _renderers;

	public void Reveal()
	{
		SetAllRendererColors(Color.white);
	}

	protected virtual void Start()
	{
		GetComponent<Collider>().isTrigger = true;
		_renderers = GetComponentsInParent<SpriteRenderer>().Concat(GetComponentsInChildren<SpriteRenderer>());
		SetAllRendererColors(Color.black);
	}

	private void SetAllRendererColors(Color color)
	{
		foreach (var renderer in _renderers) renderer.color = color;
	}
}
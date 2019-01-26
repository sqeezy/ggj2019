using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
	private IEnumerable<SpriteRenderer> _renderers;

	public bool Walkable;
	public int X;

	public int Y;

	public override string ToString()
	{
		return $"({X} | {Y})";
	}

	public void Reveal()
	{
		SetAllRendererColors(Color.white);
	}

	private void Start()
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
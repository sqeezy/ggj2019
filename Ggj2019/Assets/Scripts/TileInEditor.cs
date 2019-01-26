using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TileInEditor : MonoBehaviour
{
	private void Start()
	{
		Selection.selectionChanged += UpdateSelection;
	}

	private void UpdateSelection()
	{
	}

	private void Update()
	{
		//Debug.Log("Update");
	}

	private void OnInspectorGUI()
	{
		Debug.Log("Update");
	}
}
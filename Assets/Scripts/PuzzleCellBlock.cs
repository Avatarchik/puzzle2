using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class PuzzleCellBlock : MonoBehaviour, IPointerClickHandler
{
	public List<PuzzleCellBlockType> cells = new List<PuzzleCellBlockType> ();

	public struct PuzzleCellBlockType
	{
		public PuzzleCell.CellType type;
		public RectTransform transform;

		public PuzzleCellBlockType (PuzzleCell.CellType t, RectTransform tr)
		{
			type = t;
			transform = tr;
		}
	}

	public void RotateBlock ()
	{
		Vector3 rot = transform.eulerAngles;
		rot.z += 90f;
		transform.eulerAngles = rot;
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
			PuzzleGame.instance.OnItmClick (this);
	}
}

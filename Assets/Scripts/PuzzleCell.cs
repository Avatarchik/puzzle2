using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleCell : MonoBehaviour, IPointerClickHandler
{
	[HideInInspector]
	public RectTransform rectTransform;
	public Image img;
	public CellType typeCell;
	int _posX;
	int _posY;
	bool _isActive = true;

	public enum CellType
	{
		part1,
		part2,
		part3,
		none
	}

	public enum ReverseType
	{
		notReverse,
		reverse,
		allDirection
	}

	public int PosX {
		get{ return _posX;}
		set{ _posX = value;}
	}

	public int PosY {
		get{ return _posY;}
		set{ _posY = value;}
	}

	public bool IsActive {
		get{ return _isActive;}
		set{ _isActive = value;}
	}

	void Start ()
	{
		rectTransform = gameObject.GetComponent<RectTransform> ();
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (/*IsActive &&*/ eventData.button == PointerEventData.InputButton.Left) {
			PuzzleGame.instance.OnTargetClick ();
		}
	}

	public void SetType (CellType t)
	{
		typeCell = t;
		img.sprite = PuzzleGame.instance.sprites.Find (o => o.name.Equals (t.ToString ()));
	}
}

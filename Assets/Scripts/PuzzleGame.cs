using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class PuzzleGame : MonoBehaviour
{

	public static PuzzleGame instance;
	public List<Sprite> sprites;
	public GameObject cellPrefab;
	public GameObject cellBlockPrefab;
	public RectTransform canvas;
	public RectTransform cellBlockContainer;
	public RectTransform targetContainer;
	public float cellSize;
	const int xSize = 7;
	const int ySize = 7;
	List<PuzzleCell> cells = new List<PuzzleCell> ();
	Color32 cellColor = new Color32 (232, 102, 82, 255);
	RectTransform currentBlock;
	Dictionary<PuzzleCell.CellType,PuzzleCell.ReverseType> reverseTypes = new Dictionary<PuzzleCell.CellType, PuzzleCell.ReverseType> ()
	{
		{PuzzleCell.CellType.part1, PuzzleCell.ReverseType.notReverse},
		{PuzzleCell.CellType.part2, PuzzleCell.ReverseType.allDirection},
		{PuzzleCell.CellType.part3, PuzzleCell.ReverseType.reverse}
	};
	//Подобный масив можно создать после парсинга файла, где елементом масива будет число, например 4180, где 4 ето тип, а 180 поворот
	LevelPart[,] levelMass = new LevelPart[ySize,xSize]
	{
		{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f),new LevelPart(PuzzleCell.CellType.part1,0f), 
			new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1,90f), new LevelPart(PuzzleCell.CellType.none,0f),
			new LevelPart(PuzzleCell.CellType.none,0f)},

		{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part2,0f), new LevelPart(PuzzleCell.CellType.none,0f),
			new LevelPart(PuzzleCell.CellType.part3,90f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part2,0f),
			new LevelPart(PuzzleCell.CellType.none,0f)},

		{new LevelPart(PuzzleCell.CellType.part1,180f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1, 270f), 
			new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1,180f), new LevelPart(PuzzleCell.CellType.none,0f),
			new LevelPart(PuzzleCell.CellType.part1,270f)},

		{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part3,0f), new LevelPart(PuzzleCell.CellType.none,0f),
			new LevelPart(PuzzleCell.CellType.none,0f),new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part3,0f), new LevelPart(PuzzleCell.CellType.none,0f)},

		{new LevelPart(PuzzleCell.CellType.part1,90f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1,0f),
			new LevelPart(PuzzleCell.CellType.none,0f),new LevelPart(PuzzleCell.CellType.part1,90f), new LevelPart(PuzzleCell.CellType.none,0f),
			new LevelPart(PuzzleCell.CellType.part1,0f)},

		{new LevelPart(PuzzleCell.CellType.none,0f),new LevelPart(PuzzleCell.CellType.part2,0f), new LevelPart(PuzzleCell.CellType.none,0f),
			new LevelPart(PuzzleCell.CellType.part3,90f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part2,0f),
			new LevelPart(PuzzleCell.CellType.none,0f)},

		{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1, 270f), 
			new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1, 180f), new LevelPart(PuzzleCell.CellType.none,0f), 
			new LevelPart(PuzzleCell.CellType.none,0f)}
	};

	//Подобный масив можно создать после парсинга файла, где елементом масива будет число, например 4180, где 4 ето тип, а 180 поворот
	List<LevelPart[,]> levelMassItems =  new List<LevelPart[,]>()
	{
		new LevelPart[3,3] {
			{new LevelPart(PuzzleCell.CellType.part1,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1,90f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part3,90f), new LevelPart(PuzzleCell.CellType.none,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f)}
		},

		new LevelPart[3,3] {
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part2,0f), new LevelPart(PuzzleCell.CellType.none,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f)}
		},

		new LevelPart[3,3] {
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part3,0f), new LevelPart(PuzzleCell.CellType.none,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f)}
		},

		new LevelPart[3,3] {
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part1,0f), new LevelPart(PuzzleCell.CellType.none,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f)}
		},

		new LevelPart[3,3] {
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.part2,0f), new LevelPart(PuzzleCell.CellType.none,0f)},
			{new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f), new LevelPart(PuzzleCell.CellType.none,0f)}
		}
	};

	struct LevelPart
	{
		public PuzzleCell.CellType type;
		public float angle;

		public LevelPart(PuzzleCell.CellType t, float a)
		{
			type = t;
			angle = a;
		}
	}

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		parseLevelMass ();
		//parseLevelFile ();
	}
	
	void Update ()
	{
		if (currentBlock != null) {
			Vector3 p = Camera.main.ScreenToViewportPoint (Input.mousePosition);
			p.x *= canvas.rect.width;
			p.y *= canvas.rect.height;
			currentBlock.anchoredPosition = p;

			if (Input.GetMouseButtonDown (1)) {
				currentBlock.GetComponent<PuzzleCellBlock> ().RotateBlock ();
			}
		}
	}

	void parseLevelMass()
	{
		float xPos = 0f;
		float yPos = 0f;
		float offset = cellPrefab.GetComponent<RectTransform>().rect.width;

		for(int i = 0; i < ySize;i++)
		{
			for(int j = 0; j < xSize; j++)
			{
				xPos += offset/4f;
				PuzzleCell.CellType  cellType = levelMass[i,j].type;
				if(cellType ==  PuzzleCell.CellType.none) 
					continue;

				float rot = levelMass[i,j].angle;
				GameObject obj = Instantiate (cellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				obj.transform.SetParent (targetContainer);
				obj.transform.localScale = Vector3.one;
				RectTransform reTrans = obj.GetComponent<RectTransform> ();
				reTrans.anchoredPosition = new Vector2 (xPos, yPos);
				reTrans.eulerAngles = new Vector3 (0f, 0f, rot);
				PuzzleCell pc = obj.GetComponent<PuzzleCell> ();
				pc.SetType (cellType);
				cells.Add (pc);
			}
			xPos = 0f;
			yPos -= offset/4f;
		}
		targetContainer.anchoredPosition = new Vector2 (targetContainer.anchoredPosition.x - (xSize * offset / 8f), targetContainer.anchoredPosition.y + (ySize * offset / 8f));

		for(int i =0; i < levelMassItems.Count; i++)
		{
			GameObject obj = Instantiate (cellBlockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			obj.transform.SetParent (cellBlockContainer);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			xPos = -((offset/4f)*2f);
			yPos = offset/4f;
			for(int k =0; k < 3; k++)
			{
				for(int d = 0; d < 3; d++)
				{
					xPos += offset/4f;
					if(levelMassItems[i][k,d].type == PuzzleCell.CellType.none)
						continue;
					GameObject obj1 = Instantiate (cellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					obj1.transform.SetParent (obj.transform);
					obj1.transform.localScale = Vector3.one;
					obj1.name = k.ToString ()+d.ToString();
					RectTransform rcTrans = obj1.GetComponent<RectTransform> ();
					rcTrans.anchoredPosition = new Vector2 (xPos,yPos);
					rcTrans.eulerAngles = new Vector3 (0f, 0f, levelMassItems[i][k,d].angle);
					PuzzleCell pc = obj1.GetComponent<PuzzleCell> ();
					pc.img.color = cellColor;
					pc.SetType (levelMassItems[i][k,d].type);
					obj.GetComponent<PuzzleCellBlock> ().cells.Add (new PuzzleCellBlock.PuzzleCellBlockType (levelMassItems[i][k,d].type, obj1.GetComponent<RectTransform> ()));
					pc.enabled = false;
				}
				xPos = -((offset/4f)*2f);
				yPos -= offset/4f;
			}
		}
	}

	void parseLevelFile ()
	{
		StreamReader reader = File.OpenText (Application.dataPath + "/Data/1.txt");
		JSONObject j = new JSONObject (reader.ReadToEnd ());
		JSONObject target = j.GetField ("game");
		foreach (JSONObject jsonObj in target.list) {
			GameObject obj = Instantiate (cellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			obj.transform.SetParent (targetContainer);
			obj.transform.localScale = Vector3.one;
			RectTransform reTrans = obj.GetComponent<RectTransform> ();
			reTrans.anchoredPosition = new Vector2 (jsonObj.GetField ("x").f, jsonObj.GetField ("y").f);
			reTrans.eulerAngles = new Vector3 (0f, 0f, jsonObj.GetField ("rot").f);
			PuzzleCell pc = obj.GetComponent<PuzzleCell> ();
			pc.SetType ((PuzzleCell.CellType)Enum.Parse (typeof(PuzzleCell.CellType), jsonObj.GetField ("type").str));
			cells.Add (pc);
		}

		JSONObject objects = j.GetField ("objects");
		foreach (JSONObject jsonObj in objects.list) {
			GameObject obj = Instantiate (cellBlockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			obj.transform.SetParent (cellBlockContainer);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;

			JSONObject tmp = jsonObj.GetField ("obj");
			int i = 0;
			foreach (JSONObject jsonObjFigure in tmp.list) {
				GameObject obj1 = Instantiate (cellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				obj1.transform.SetParent (obj.transform);
				obj1.transform.localScale = Vector3.one;
				obj1.name = i.ToString ();
				RectTransform rcTrans = obj1.GetComponent<RectTransform> ();
				rcTrans.anchoredPosition = new Vector2 (jsonObjFigure.GetField ("x").f, jsonObjFigure.GetField ("y").f);
				rcTrans.eulerAngles = new Vector3 (0f, 0f, jsonObjFigure.GetField ("rot").f);
				PuzzleCell pc = obj1.GetComponent<PuzzleCell> ();
				pc.img.color = cellColor;
				PuzzleCell.CellType t = (PuzzleCell.CellType)Enum.Parse (typeof(PuzzleCell.CellType), jsonObjFigure.GetField ("type").str);
				pc.SetType (t);
				obj.GetComponent<PuzzleCellBlock> ().cells.Add (new PuzzleCellBlock.PuzzleCellBlockType (t, obj1.GetComponent<RectTransform> ()));
				pc.enabled = false;
				i++;
			}
		}
	}

	public void OnItmClick (PuzzleCellBlock objBlock)
	{
		if (currentBlock != null)
			Destroy (currentBlock.gameObject);

		GameObject obj = Instantiate (objBlock.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		obj.transform.SetParent (transform);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		PuzzleCellBlock pzCellBlock = obj.GetComponent<PuzzleCellBlock> ();
		foreach (Transform child in obj.transform) {
			PuzzleCell puzzleCell = child.gameObject.GetComponent<PuzzleCell> ();
			PuzzleCell.CellType cellType = objBlock.cells.Find (k => k.transform.name == child.name).type;
			pzCellBlock.cells.Add (new PuzzleCellBlock.PuzzleCellBlockType (cellType, child.GetComponent<RectTransform> ()));
		}
		CanvasGroup canvasGroup = obj.AddComponent<CanvasGroup> ();
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		currentBlock = obj.GetComponent<RectTransform> ();
	}

	public void OnTargetClick ()
	{
		if (currentBlock == null)
			return;
		PuzzleCellBlock block = currentBlock.GetComponent<PuzzleCellBlock> ();
		List<PuzzleCell> cellsOnTarget = new List<PuzzleCell> ();
		foreach (PuzzleCellBlock.PuzzleCellBlockType bCell in block.cells) {
			float dist = Mathf.Infinity;
			PuzzleCell findedCell = null;
			foreach (PuzzleCell puzzleCell in cells) {
				if (cellsOnTarget.Contains (puzzleCell))
					continue;
				float val = Vector2.Distance (bCell.transform.position, puzzleCell.rectTransform.position);
				if (val < dist) {
					dist = val;
					findedCell = puzzleCell;
				}
			}

			if (findedCell != null && findedCell.typeCell == bCell.type && findedCell.IsActive && 
				allowInsert (findedCell.rectTransform.eulerAngles.z, bCell.transform.eulerAngles.z, reverseTypes [findedCell.typeCell])) {
				cellsOnTarget.Add (findedCell);
			} else {
				Debug.Log ("Inappropriate block");
				return;
			}
		}

		for (int i = 0; i < cellsOnTarget.Count; i++) {
			cellsOnTarget [i].GetComponent<Image> ().color = cellColor;
			cellsOnTarget [i].IsActive = false;
		}
		Destroy (currentBlock.gameObject);
	}

	bool allowInsert (float angle1, float angle2, PuzzleCell.ReverseType reverseType)
	{
		if (reverseType == PuzzleCell.ReverseType.allDirection)
			return true;
		if (reverseType == PuzzleCell.ReverseType.notReverse)
			return Mathf.Abs (angle1 - angle2) < 1f;
		if (reverseType == PuzzleCell.ReverseType.reverse) {
			angle1 = angle1 % 360f;
			angle2 = angle2 % 360f;
			return Mathf.Abs ((angle1 % 180f) - (angle2 % 180f)) < 1f;
		}
		return false;
	}

}
		
﻿using System.Collections.Generic;
using BlockClasses;
using ManagerClasses;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class ControllerMenu : MonoBehaviour
{
	[SerializeField] Transform _menuObject;
	[SerializeField] Text _levelText;

	[Header ("Level Preview ")]
	[SerializeField] Transform _previewHolder;
	[SerializeField] Transform _previewCube;
	[SerializeField] Color[] _previewColorArray;
	[SerializeField] float _posDiv = 100.0f;

	int _levelIndex;
	List<string> _savedLevels;

	private void Start ()
	{
		GetComponent<VRTK_ControllerEvents> ().ButtonTwoPressed += new ControllerInteractionEventHandler (DoMenuOn);
		GetComponent<VRTK_ControllerEvents> ().ButtonTwoReleased += new ControllerInteractionEventHandler (DoMenuOff);
		_savedLevels = LevelLayoutManager.instance.GetSavedLevels ();
		_menuObject.gameObject.SetActive (false);
	}

	private void DoMenuOn (object sender, ControllerInteractionEventArgs e)
	{
		_menuObject.gameObject.SetActive (true);
		_levelText.text = LevelLayoutManager.instance._GetLevelName;
		GenerateLevelPreview (LevelLayoutManager.instance.GetLevelData (_levelText.text));
		UpdateIndex ();
	}

	private void UpdateIndex ()
	{
		for (int i = 0; i < _savedLevels.Count; i++)
			if (_levelText.text == _savedLevels[i])
			{
				_levelIndex = i;
				break;
			}
		else _levelIndex = 0;
	}

	private void DoMenuOff (object sender, ControllerInteractionEventArgs e)
	{
		_menuObject.gameObject.SetActive (false);
	}

	public void NextLevel ()
	{
		UpdateIndex ();
		int temp = _levelIndex + 1;
		if (temp < _savedLevels.Count && _savedLevels[temp] != null)
		{
			_levelText.text = _savedLevels[temp];
			GenerateLevelPreview (LevelLayoutManager.instance.GetLevelData (_levelText.text));
		}
	}

	public void PrevLevel ()
	{
		UpdateIndex ();
		int temp = _levelIndex - 1;
		if (temp >= 0 && _savedLevels[temp] != null)
		{
			_levelText.text = _savedLevels[temp];
			GenerateLevelPreview (LevelLayoutManager.instance.GetLevelData (_levelText.text));
		}
	}

	void GenerateLevelPreview (List<LevelData> levelDataList_)
	{
		foreach (Transform child in _previewHolder)
			if (child != null) Destroy (child.gameObject);

		if (levelDataList_ != null)
			foreach (LevelData ld in levelDataList_)
			{
				Vector3 pos = LevelLayoutManager.instance.GetVectorFromData (ld);
				// Vector3 rot = new Vector3 (0, ld._rotationY, 0);
				Transform clone = Instantiate (_previewCube, _previewHolder.transform.position + (pos / _posDiv), Quaternion.identity, _previewHolder);
				clone.GetComponent<MeshRenderer> ().material.color = _previewColorArray[(int) ld._blockType + 1];
			}
	}

	public void RestartLevel ()
	{
		LevelLayoutManager.instance.LoadLevel (_levelText.text);
	}

	public void LoadLevel ()
	{
		LevelLayoutManager.instance.LoadLevel (_levelText.text);
	}

}
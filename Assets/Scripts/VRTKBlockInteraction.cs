﻿using System.Collections;
using BlockClasses;
using UnityEngine;
using VRTK;

public class VRTKBlockInteraction : MonoBehaviour
{

	[SerializeField] LayerMask _blocksLM;
	[SerializeField] LayerMask _ghostBlockLM;
	[SerializeField] Transform _selectionBlock;
	[SerializeField] BlockSelection _blockSelection;
	[SerializeField] bool _debug;

	Block _currentBlock;

	public bool _IsHolding { get; private set; }

	VRTK_ControllerEvents _controllerEvents;
	Collider[] _colliders = new Collider[10];

	bool _isInPlaceablePosition;

	private void Awake ()
	{
		_controllerEvents = GetComponent<VRTK_ControllerEvents> ();
	}

	private void Start ()
	{
		_controllerEvents.TriggerPressed += new ControllerInteractionEventHandler (ControllerGrab);
		_controllerEvents.TriggerReleased += new ControllerInteractionEventHandler (ControllerPlace);
		_controllerEvents.GripPressed += new ControllerInteractionEventHandler (ControllerCancel);
	}

	void ControllerGrab (object sender, ControllerInteractionEventArgs e)
	{
		if (!_IsHolding) Grab ();
	}

	void ControllerPlace (object sender, ControllerInteractionEventArgs e)
	{
		if (_IsHolding) Place ();
	}

	void ControllerCancel (object sender, ControllerInteractionEventArgs e)
	{
		if (_IsHolding) Cancel ();
	}

	void Grab ()
	{
		if (_debug) Debug.Log ("Attempting to Grab");

		_currentBlock = _blockSelection._CurrentlySelectedBlock;

		if (_currentBlock != null)
		{
			_isInPlaceablePosition = true;
			_IsHolding = true;
			_currentBlock.OnGrabbed ();
			StartCoroutine (UpdateGhostBlock ());
		}
		else if (_debug) Debug.Log ("Unable to grab!");
	}

	IEnumerator UpdateGhostBlock ()
	{
		int numCols = 0;
		while (_IsHolding)
		{
			numCols = 0;
			Vector3 pos = transform.position.ToInt ();
			numCols = Physics.OverlapSphereNonAlloc (pos, 0.2f, _colliders, _blocksLM);
			numCols += Physics.OverlapSphereNonAlloc (pos, 0.2f, _colliders, _ghostBlockLM);
			if (numCols == 0) _currentBlock._BlockGhostMesh.transform.position = pos;
			if (!_currentBlock._BlockGhostMesh._BlockCollisions.HasAdjacentBlock (true, _currentBlock.transform.position))
				_isInPlaceablePosition = false;
			else _isInPlaceablePosition = true;
			yield return new WaitForSeconds (0.05f);
		}
	}

	void Place ()
	{
		if (_debug) Debug.Log ("Placing.");
		if (!_isInPlaceablePosition) Cancel ();
		_IsHolding = false;
		// Don't ask, it just works -.- 
		_currentBlock.transform.position = _currentBlock._BlockGhostMesh.transform.position;
		_currentBlock._BlockGhostMesh.transform.position = _currentBlock.transform.position;
		BlockManager.instance.CheckForRoot (_currentBlock);
		_currentBlock.OnPlaced ();
	}

	void Cancel ()
	{
		if (_debug) Debug.Log ("Cancelling");
		_IsHolding = false;
		_currentBlock._BlockGhostMesh.transform.position = _currentBlock.transform.position;
		_currentBlock.OnPlaced (true);
	}

}

// Might add this as an option later so I'll just keep it in for now.
// Allows for input via Ray instead of grabbing blocks.

// [SerializeField] LayerMask _blocksLM;
// public bool _IsHolding { get; private set; }
// Block _currentBlock;

// [Header ("VR Mode")]
// [SerializeField] Transform _controller;
// [SerializeField] Transform _leftController;
// [SerializeField] bool _VRMode;

// VRTK_ControllerEvents _controllerEvents;

// Ray ray = new Ray ();

// static VRTKBlockInteraction _instance;
// public static VRTKBlockInteraction instance
// {
// 	get
// 	{
// 		if (!_instance)
// 			_instance = FindObjectOfType<VRTKBlockInteraction> ();
// 		return _instance;
// 	}
// }

// private void Awake ()
// {
// 	_controllerEvents = _controller.GetComponent<VRTK_ControllerEvents> ();
// }

// private void Start ()
// {
// 	_controllerEvents.TriggerPressed += new ControllerInteractionEventHandler (ControllerGrab);
// 	_controllerEvents.TriggerReleased += new ControllerInteractionEventHandler (ControllerPlace);
// 	_controllerEvents.GripPressed += new ControllerInteractionEventHandler (ControllerCancel);
// }

// private void Update ()
// {
// 	if (Input.GetButtonDown ("Fire1") || Input.GetKeyDown (KeyCode.E))
// 	{
// 		if (!_IsHolding) Grab ();
// 		else
// 		if (_IsHolding) Place ();
// 	}
// }

// void ControllerGrab (object sender, ControllerInteractionEventArgs e)
// {
// 	if (!_IsHolding) Grab ();
// }

// void ControllerPlace (object sender, ControllerInteractionEventArgs e)
// {
// 	if (_IsHolding) Place ();
// }

// void ControllerCancel (object sender, ControllerInteractionEventArgs e)
// {
// 	if (_IsHolding) Cancel ();
// }

// void Grab ()
// {
// 	if(_debug) Debug.Log ("Attempting to Grab");

// 	if (_VRMode)
// 	{
// 		ray.origin = _controller.position;
// 		ray.direction = _controller.forward;
// 	}
// 	else ray = Camera.main.ScreenPointToRay (Input.mousePosition);

// 	RaycastHit hit;

// 	if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
// 	{
// 		Block block = hit.transform.GetComponent<Block> ();
// 		if (block != null)
// 		{
// 			if(_debug) Debug.Log ("Grabbing.");
// 			_IsHolding = true;
// 			_currentBlock = block;
// 			_currentBlock.GetGhostBlock.gameObject.SetActive (true);
// 			StartCoroutine (UpdateGhostBlock ());
// 		}
// 		else
// 			if(_debug) Debug.Log ("You cannot grab this!");
// 	}
// }

// IEnumerator UpdateGhostBlock ()
// {
// 	while (_IsHolding)
// 	{
// 		if (_VRMode)
// 		{
// 			ray.origin = _controller.position;
// 			ray.direction = _controller.forward;
// 		}
// 		else ray = Camera.main.ScreenPointToRay (Input.mousePosition);

// 		RaycastHit hit;

// 		if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
// 		{
// 			Vector3 newPos = hit.transform.position + hit.normal;
// 			_currentBlock.GetGhostBlock.position = newPos;
// 		}

// 		yield return new WaitForSeconds (0.1f);
// 	}
// }

// void Place ()
// {
// 	if(_debug) Debug.Log ("Placing.");
// 	_IsHolding = false;
// 	StopCoroutine (UpdateGhostBlock ());
// 	_currentBlock.PlaceBlock (_currentBlock.GetGhostBlock.position);
// 	_currentBlock.GetGhostBlock.gameObject.SetActive (false);
// }

// void Cancel ()
// {
// 	if(_debug) Debug.Log ("Cancelling");
// 	_IsHolding = false;
// 	StopCoroutine (UpdateGhostBlock ());
// 	_currentBlock.GetGhostBlock.position = transform.position;
// 	_currentBlock.GetGhostBlock.gameObject.SetActive (false);
// }
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuild : Block, IPlaceable
{
	// [SerializeField] string _defaultParentTag = "BlockBuildsHolder";

	[Space (1.0f)]

	[SerializeField] List<Vector3> _colliders;
	[SerializeField] Transform _blockGhost;
	[SerializeField] LayerMask _blocksLM;

	// List<IPlaceable> pList = new List<IPlaceable> ();

	// public List<IPlaceable> GetConnectedBlocks ()
	// {
	// 	_colliders = GetAdjacentSpaces (transform);
	// 	pList.Clear ();

	// 	for (int i = 0; i < _colliders.Count; i++)
	// 	{
	// 		Collider[] cols = Physics.OverlapSphere (_colliders[i], 0.2f, _blocksLM);
	// 		if (cols.Length != 0)
	// 		{
	// 			IPlaceable temp = cols[0].transform.GetComponent<IPlaceable> ();
	// 			if (temp != null) pList.Add (temp);
	// 		}
	// 	}

	// 	return pList;
	// }

	public Transform GetGhostBlock ()
	{
		return _blockGhost;
	}

	public void PlaceBlock (Vector3 pos_)
	{
		transform.position = pos_;
	}

}
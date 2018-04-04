﻿using System.Collections.Generic;
using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public abstract class Block : MonoBehaviour
	{
		[Space (10.0f)]
		[Header ("Block: ")]
		[SerializeField] bool _randomizeXRotation;
		protected Transform _blockGhost;

		protected virtual void Awake ()
		{
			if (GetComponent<IPlaceable> () != null) SetupGhostMesh ();
		}

		protected virtual void Start ()
		{
			if (_randomizeXRotation) RandomXRotation ();
		}

		void SetupGhostMesh ()
		{
			_blockGhost = transform.Find ("GhostMesh");
			if (_blockGhost == null) Debug.LogError ("No Ghost Block on a IPlaceable GameObject!");
			_blockGhost.GetComponent<MeshFilter> ().mesh = GetComponent<MeshFilter> ().mesh;
		}

		public virtual bool BlockEffect (IBotMovement bot_)
		{
			// Default no effect takes place.
			return false;
		}

		protected void RandomXRotation ()
		{
			int x = Random.Range (0, 4);
			transform.rotation = Quaternion.AngleAxis (x * 90, Vector3.up);
		}

		protected static List<Vector3> GetAdjacentSpaces (Transform vec_)
		{
			List<Vector3> VecList = new List<Vector3> ();
			VecList.Add (vec_.up);
			VecList.Add (vec_.right * -1);
			VecList.Add (vec_.right);
			VecList.Add (vec_.forward);
			VecList.Add (vec_.forward * -1);
			return VecList;
		}

	}

	public interface IPlaceable
	{
		Transform GetGhostBlock { get; }
		// Transform GetGhostBlock ();
		void PlaceBlock (Vector3 pos_);
	}
}
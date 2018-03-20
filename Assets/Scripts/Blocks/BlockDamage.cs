﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDamage : Block
{
    // [SerializeField] string _defaultParentTag = "BlockBuildsHolder";

    [SerializeField] List<Vector3> _colliders;
    [SerializeField] Transform _blockGhost;
    [SerializeField] LayerMask _blocksLM;

    public override bool BlockEffect (Transform bot_)
    {
        Debug.Log ("Damage Effect.");
        bot_.GetComponent<Health> ().Damage (int.MaxValue);
        return true;
    }

    public Transform GetGhostBlock ()
    {
        return _blockGhost;
    }

    public void PlaceBlock (Vector3 pos_)
    {
        transform.position = pos_;
    }

}
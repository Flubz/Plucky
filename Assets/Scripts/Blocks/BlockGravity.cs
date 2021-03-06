﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BlockClasses
{
    public class BlockGravity : Block
    {
        [Header ("Gravity Block Attributes")]
        [SerializeField] LayerMask _blocksLM;
        [SerializeField] List<Transform> _gravityEffects;

        [SerializeField] int _maxHeight = 3;
        [SerializeField] float _checkTimer = 1.0f;
        float _heightReachable;

        public override void InitializeILevelObject (float spawnEffectTime_)
        {
            base.InitializeILevelObject (spawnEffectTime_);
            StartCoroutine (CalculateDistanceEnumerator ());
        }

        public override bool BlockEffect (IBot bot_)
        {
            CalculateDistance ();
            bot_.BotDestination (transform.position + transform.up * _heightReachable);
            return true;
        }

        private void CalculateDistance ()
        {
            _heightReachable = _maxHeight;

            for (int i = 0; i < _maxHeight; i++)
            {
                Collider[] cols = Physics.OverlapSphere (transform.position + transform.up * (i + 1), 0.2f, _blocksLM);
                if (cols.Length != 0)
                {
                    _heightReachable = i;
                    break;
                }
            }

            for (int i = 0; i < _maxHeight; i++)
            {
                if (i < _heightReachable)
                {
                    _gravityEffects[i].gameObject.SetActive (true);
                    _gravityEffects[i].transform.position = transform.position + transform.up * (i + 1);
                }
                else
                    _gravityEffects[i].gameObject.SetActive (false);
            }
        }

        IEnumerator CalculateDistanceEnumerator ()
        {
            while (true)
            {
                CalculateDistance ();
                yield return new WaitForSeconds (_checkTimer);
            }
        }

        public void PlaceBlock (Vector3 pos_)
        {
            CalculateDistance ();
            transform.position = pos_;
        }

        public override void DeathEffect (float deathEffectTime_)
        {
            base.DeathEffect (deathEffectTime_);
            foreach (Transform effect in _gravityEffects) effect.DOScale (Vector3.zero, deathEffectTime_ / 0.3f);
        }

    }
}
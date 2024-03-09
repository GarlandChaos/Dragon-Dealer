using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Game.Gameplay
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Transform body = null;
        [SerializeField] private float moveDuration = 0.5f;
        private Vector3 initialPosition = Vector3.zero;

        public void SetInitialPosition(Vector3 position)
        {
            initialPosition = position;
        }

        public void Move(Vector3 positionToMove, Action onCompleteCallback = null)
        {
            body.DOMove(positionToMove, moveDuration)
                .OnComplete(() =>
                {
                    onCompleteCallback?.Invoke();
                });
        }

        public void MoveToInitialPosition(Action onCompleteCallback = null)
        {
            Move(initialPosition, onCompleteCallback);
        }
    }
}
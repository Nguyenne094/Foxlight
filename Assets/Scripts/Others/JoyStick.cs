using System;
using Bap.DependencyInjection;
using Bap.Service_Locator;
using UnityEngine;
using UnityEngine.EventSystems;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace PlatformingGame.Controller
{
    public class JoyStick : MonoBehaviour
    {
        [SerializeField] private RectTransform _knob;
        [SerializeField] private float _radius;
        [SerializeField ,Range(0, 1) ,Tooltip("Horizontal value must larger than this value for Player takes input")] 
        private float xThreshold;

        private RectTransform _rect;
        private ETouch.Finger _movementFinger;
        private bool _isTouching = false;

        [Provide] private JoyStick Construct() => this;
        
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            ETouch.EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += HandleFingerDown;
            ETouch.Touch.onFingerMove += HandleFingerMove;
            ETouch.Touch.onFingerUp += HandleFingerUp;
        }

        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= HandleFingerDown;
            ETouch.Touch.onFingerMove -= HandleFingerMove;
            ETouch.Touch.onFingerUp -= HandleFingerUp;
            ETouch.EnhancedTouchSupport.Disable();
        }

        private void HandleFingerDown(ETouch.Finger touchedFinger)
        {
            if (_movementFinger == null && touchedFinger.currentTouch.startScreenPosition.x <= Screen.width/2)
            {
                _movementFinger = touchedFinger;
                _isTouching = true;
                ClampKnobPosition(_movementFinger.currentTouch.screenPosition);
            }
        }

        private void HandleFingerMove(ETouch.Finger movedFinger)
        {
            if (movedFinger == _movementFinger)
            {
                ClampKnobPosition(_movementFinger.currentTouch.screenPosition);
            }
        }

        private void HandleFingerUp(ETouch.Finger lostFinger)
        {
            if (lostFinger == _movementFinger)
            {
                _knob.anchoredPosition = Vector2.zero;
                _movementFinger = null;
                _isTouching = false;
            }
        }

        private void ClampKnobPosition(Vector2 touchPosition)
        {
            if (Vector2.Distance(_rect.anchoredPosition, touchPosition) > _radius)
            {
                _knob.anchoredPosition = (touchPosition - _rect.anchoredPosition).normalized * _radius;
            }
            else
            {
                _knob.anchoredPosition = touchPosition - _rect.anchoredPosition;
            }
        }

        public int GetNormalizedHorizontalMovement()
        {
            //Player is touching and horizontal input value is larger than threshold
            if (_isTouching && Mathf.Abs(_knob.anchoredPosition.x) > xThreshold * _radius)
            {
                return (int)Mathf.Sign(_knob.anchoredPosition.x);
            }

            return 0;
        }
    }
}
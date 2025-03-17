using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Dialogue
{
    /// <summary>
    /// Just for NPC interact with Player
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        enum TriggerMethod{Collider, UserTap}

        [SerializeField] private TriggerMethod _triggerMethod = TriggerMethod.Collider;
        [SerializeField] private bool _repeat = false;
        [SerializeField] private DialogueDataScriptableObject _dialogueDataSO;
        [SerializeField] private float _normalCharWaitTime;
        [SerializeField] private float _fastCharWaitTime;
        
        [SerializeField] private RectTransform _rect;
        
        //TODO: Random Speech

        private ETouch.Touch _currentTouch;
        private TMP_Text _text;

        private bool _tapProcessed = false;
        private bool _inDialogue;
        private bool _finishCurrentSpeech;

        private int _index = 0;
        private float _currentCharWaitTime;
        private int _repeatTimes = 0;
        
        private void Awake()
        {
            _text = _rect.gameObject.GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            _currentCharWaitTime = _normalCharWaitTime;
        }

        private void OnEnable()
        {
            ETouch.EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += OnFingerDown;
        }

        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= OnFingerDown;
            ETouch.EnhancedTouchSupport.Disable();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_repeat || (!_repeat && _repeatTimes < 1))
            {
                switch (_triggerMethod)
                {
                    case TriggerMethod.Collider:
                        if (_triggerMethod == TriggerMethod.Collider)
                        {
                            if (other.gameObject.CompareTag("Player") && _dialogueDataSO.TalkTo == DialogueDataScriptableObject.TalkToWho.Player)
                            {
                                _inDialogue = true;
                                _rect.gameObject.SetActive(true);
                                _index = 0;
                                PlayDialogue();
                            }
                        }
                        break;
                    case TriggerMethod.UserTap:
                        break;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_triggerMethod == TriggerMethod.Collider)
            {
                if (other.gameObject.CompareTag("Player") && _dialogueDataSO.TalkTo == DialogueDataScriptableObject.TalkToWho.Player)
                {
                    FinishDialogue();
                }
            }
        }

        public void OnFingerDown(ETouch.Finger finger)
        {
            if (!_finishCurrentSpeech && _inDialogue && _currentCharWaitTime == _normalCharWaitTime) _currentCharWaitTime = _fastCharWaitTime;
            
            if (CanGoToNextSpeech(finger))
            {
                _finishCurrentSpeech = false;
                _currentCharWaitTime = _normalCharWaitTime;
                
                _index++;
                if (_index == _dialogueDataSO.Speech.Count)
                {
                    FinishDialogue();
                    _repeatTimes++;
                    return;
                }

                char[] charArray = _dialogueDataSO.Speech[_index].ToCharArray();
                StartSpeech(charArray);
            }
        }

        private async Task StartSpeech(char[] chars)
        {
            _text.text = string.Empty;
            await Task.Yield();
            foreach (var c in chars)
            {
                _text.text += c;
                await Task.Delay((int)(_currentCharWaitTime * 1000));
            }
            
            _finishCurrentSpeech = true;
            await Task.Yield();
        }
        
        private void PlayDialogue()
        {
            char[] charArray = _dialogueDataSO.Speech[_index].ToCharArray();
            StartSpeech(charArray);
        }

        private bool CanGoToNextSpeech(ETouch.Finger finger)
        {
            return finger.currentTouch.startScreenPosition.x >= Screen.width/2 && 
                   _inDialogue &&
                   _finishCurrentSpeech;
        }

        public void FinishDialogue()
        {
            _rect.gameObject.SetActive(false);
            _inDialogue = false;
            _finishCurrentSpeech = false;
            _currentCharWaitTime = _normalCharWaitTime;
        }
    }
}
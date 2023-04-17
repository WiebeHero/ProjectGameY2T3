using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using User.Wiebe.Scripts.States;
using User.Wiebe.Scripts.States.StateManager;

namespace Main._Scripts.Main
{
    public class DragonCommands : MonoBehaviour, SpeechRecognizerPlugin.ISpeechRecognizerPlugin
    {
        [SerializeField] private AnimationStateManager _stateManager;
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TextMeshProUGUI _updateText;
        
        private Dictionary<CommandDisplay, Type> commands;
        private Animator _animator;
        
        private bool _recordingInput;
        private int _timesRecorded;
        private List<string> _spokenWords;
        private bool _spoken;
        private SpeechRecognizerPlugin speechRecognizer = null;

        private void Start()
        {
            _spokenWords = new List<string>();
            
            commands = new Dictionary<CommandDisplay, Type>
            {
                { new CommandDisplay("Dance", "dance", "dance"), typeof(DanceState) },
                { new CommandDisplay("Stop", "stop", "stop"), typeof(IdleState) },
                { new CommandDisplay("Sit", "sit", "sit"), typeof(SitState) },
                { new CommandDisplay("Lie down", "lie down", "lie down"), typeof(LieDownState) },
                { new CommandDisplay("Roll", "roll", "roll"), typeof(RollOverState) },
                { new CommandDisplay("Get up", "get up", "get up"), typeof(IdleState) },
            };

            if (_dropdown == null)
                throw new Exception("CubeCommand: No dropdown was coupled to this object!");

            if (_updateText == null)
                throw new Exception("CubeCommand: No update text was coupled to this object!");

            _updateText.text = "Not recording...";
            
            foreach (KeyValuePair<CommandDisplay, Type> pair in commands)
            {
                Debug.Log("Jajajaja");
                _dropdown.options.Add(new TMP_Dropdown.OptionData(pair.Key.Display));
            }

            Transform obj = transform.GetChild(0);
            _animator = obj.GetComponent<Animator>();

            if (_animator == null)
                throw new Exception("Dragon: There is no animator component!");
            
            Debug.Log("Starting voice listener!");
            speechRecognizer = SpeechRecognizerPlugin.GetPlatformPluginVersion(gameObject.name);
            //The old listening configuration
            //speechRecognizer.SetContinuousListening(true);
            //speechRecognizer.StartListening();
            Debug.Log("Voice listener started!");
        }

        private void OnDisable()
        {
            speechRecognizer.StopListening();
            speechRecognizer = null;
        }

        public void OnResult(string recognizedResult)
        {
            
            char[] delimiterChars = { '~' };
            string[] results = recognizedResult.Split(delimiterChars);
            Debug.Log("Were in here.");
            foreach (string t in results)
            {
                Debug.Log("Result: " + t);
                if(!_recordingInput)
                    PossiblyExecuteCommand(t);
                else
                    RecordInput(t);
            }
            if(_recordingInput)
                _timesRecorded++;
            if (_timesRecorded >= 3)
                FinalOutput();
        }

        private void PossiblyExecuteCommand(string result)
        {
            Debug.Log("Voice result: " + result);
            bool recognised = true;
            foreach (KeyValuePair<CommandDisplay, Type> pair in commands)
            {
                result = result.ToLower();
                if (result.Contains(pair.Key.Command))
                {
                    Debug.Log("Voice command: " + pair.Key.Command);
                    AnimationStateManager.SetState(pair.Value);
                    recognised = false;
                }
            }
            Debug.Log("Recorded");
            if (recognised)
            {
                _animator.Play("what the fuck___");
                Debug.Log("Recording done here");
            }
        }

        public void StartRecordingCommand()
        {
            speechRecognizer.StartListening(true);
        }

        public void StopRecordingCommand()
        {
            speechRecognizer.StopListening();
        }
        
        public void RecordChosenInput()
        {
            _recordingInput = true;
            _updateText.text = "Recording...";
        }

        public void CancelRecording()
        {
            _recordingInput = false;
            _updateText.text = "Not Recording...";
            _spokenWords.Clear();
            _spoken = false;
            _timesRecorded = 0;
        }

        private void RecordInput(string result)
        {
            string[] splitted = result.Split(' ');
            _spokenWords.AddRange(splitted);
            if (!_spoken)
            {
                _updateText.text = "Recorded!";
                StartCoroutine(UpdateText());
                _spoken = true;
            }
        }

        private void FinalOutput()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (string word in _spokenWords)
            {
                if (dictionary.ContainsKey(word))
                    dictionary[word] += 1;
                else
                    dictionary.Add(word, 1);
            }
            Debug.Log("Dictionary size: " + dictionary.Count);
            Dictionary<string, int> orderedDictionary = dictionary.OrderBy(keyValuePair => keyValuePair.Value).ToDictionary(x => x.Key, x => x.Value);
            Debug.Log("ordered size: " + orderedDictionary.Count);
            Dictionary<string, int> reversed = orderedDictionary.Reverse().ToDictionary(x => x.Key, x => x.Value);
            string thisWord = "";
            foreach (KeyValuePair<string, int> pair in reversed)
            {
                Debug.Log("Executed!");
                thisWord = pair.Key;
                Debug.Log("This word: " + thisWord);
                break;
            }
            string dropDown = _dropdown.options[_dropdown.value].text;
            KeyValuePair<CommandDisplay, Type>[] itemsToRemove = commands.Where(entry => entry.Key.Original.Equals(dropDown.ToLower())).ToArray();
            Debug.Log("Items to remove size: " + itemsToRemove.Length);
            KeyValuePair<CommandDisplay, Type> oldCommand = default;
            foreach (KeyValuePair<CommandDisplay, Type> item in itemsToRemove)
            {
                Debug.Log("Current command: " + item.Key.Command);
                oldCommand = item;
                commands.Remove(item.Key);
            }
            Debug.Log("New command: " + thisWord);
            commands.Add(new CommandDisplay(oldCommand.Key.Display, thisWord, oldCommand.Key.Original), oldCommand.Value);
            _recordingInput = false;
            _timesRecorded = 0;
            _spokenWords.Clear();
            _spoken = false;
            _updateText.text = "Not Recording...";
        }

        public void OnError(string recognizedError)
        {
            SpeechRecognizerPlugin.ERROR error = (SpeechRecognizerPlugin.ERROR)int.Parse(recognizedError);
            switch (error)
            {
                case SpeechRecognizerPlugin.ERROR.UNKNOWN:
                    Debug.Log("<b>ERROR: </b> Unknown");
                    break;
                case SpeechRecognizerPlugin.ERROR.INVALID_LANGUAGE_FORMAT:
                    Debug.Log("<b>ERROR: </b> Language format is not valid");
                    break;
            }
        }

        private IEnumerator StopAnim(string command)
        {
            yield return new WaitForSeconds(1f);
            _animator.SetBool(command, false);
        }

        public bool RecordingInput
        {
            get => _recordingInput;
            set => _recordingInput = value;
        }

        IEnumerator UpdateText()
        {
            yield return new WaitForSeconds(1f);
            _updateText.text = "Recording...";
            _spoken = false;
            if (_timesRecorded >= 3)
                _updateText.text = "Not recording...";
        }
    }
}

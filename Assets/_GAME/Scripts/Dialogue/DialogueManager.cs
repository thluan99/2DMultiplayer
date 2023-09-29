using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private const string SPEAKER_TAG = "speaker";
    private const string AVATAR_TAG = "avatar";

    [Header("Params")]
    [SerializeField] private float _typingSpeed = 0.04f;

    [Header("Load globalJson")]
    [SerializeField] private TextAsset _loadGlobalJson;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _displayNameText;
    [SerializeField] private GameObject _continueIcon;
    [SerializeField] private Animator _avatarAnimator;
    [SerializeField] private GameObject[] _choices;
    private TextMeshProUGUI[] _choiceTexts;
    private Story _currentStory;
    private bool _canContinueToNextLine = false;
    private DialogueVariables _dialogueVariables;
    public bool IsDialoguePlaying { get; private set; }
    private static DialogueManager _instance;
    public static DialogueManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene!");
        }
        _instance = this;

        _dialogueVariables = new DialogueVariables(_loadGlobalJson);
    }

    private void Start()
    {
        IsDialoguePlaying = false;
        _dialoguePanel.SetActive(false);
        _choiceTexts = new TextMeshProUGUI[_choices.Length];

        Observable.EveryUpdate()
            .Where(_ => HasContinueStoryInput() && _canContinueToNextLine &&
                IsDialoguePlaying && _currentStory.currentChoices.Count == 0)
            .Subscribe(_ => ContinueStory())
            .AddTo(gameObject);

        SetupChoiceUI();
    }

    private bool HasContinueStoryInput()
    {
        return Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Return);
    }

    private void SetupChoiceUI()
    {
        for (int i = 0; i < _choices.Length; i++)
        {
            _choiceTexts[i] = _choices[i].GetComponentInChildren<TextMeshProUGUI>();
            int temp = i;
            _choices[i].GetComponent<Button>().onClick.AddListener(() => OnMakeChoice(temp));
        }
    }

    public void EnterDialogueMode(TextAsset inkJson, Animator emoteAnimator)
    {
        _currentStory = new Story(inkJson.text);
        IsDialoguePlaying = true;
        _dialoguePanel.SetActive(true);

        _dialogueVariables.StartListening(_currentStory);
        _currentStory.BindExternalFunction("playEmote", (string emoteName) => {
            Debug.Log(emoteName);
            if (emoteAnimator != null)
            {
                emoteAnimator.Play(emoteName);
            }
        });

        _displayNameText.SetText("???");
        _avatarAnimator.Play("default");

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            if (_displayLineCoroutine != null)
                StopCoroutine(_displayLineCoroutine);

            _displayLineCoroutine = StartCoroutine(DisplayLine(_currentStory.Continue()));

            HandleTags(_currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    Coroutine _displayLineCoroutine;
    private IEnumerator DisplayLine(string line)
    {
        _dialogueText.SetText(line);
        _dialogueText.maxVisibleCharacters = 0;
        _canContinueToNextLine = false;
        _continueIcon.SetActive(false);
        HideChoices();

        bool isEndOfLine = false;
        IDisposable trackingInput = Observable.EveryUpdate()
            .First(_ => HasContinueStoryInput())
            .Subscribe(_ =>
            {
                _dialogueText.maxVisibleCharacters = line.Length;
                isEndOfLine = true;
            }).AddTo(gameObject);

        foreach (char letter in line.ToCharArray())
        {
            if (isEndOfLine)
                break;

            _dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(_typingSpeed);
        }
        trackingInput.Dispose();

        DisplayChoices();
        _canContinueToNextLine = true;
        _continueIcon.SetActive(true);

    }

    private void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag error: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    _displayNameText.SetText(tagValue);
                    break;
                case AVATAR_TAG:
                    _avatarAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag key not handle! " + tagKey);
                    break;
            }
        }
    }

    private void ExitDialogueMode()
    {
        _dialogueVariables.StopListening(_currentStory);
        _currentStory.UnbindExternalFunction("playEmote");

        IsDialoguePlaying = false;
        _dialoguePanel.SetActive(false);
        _dialogueText.SetText("");
    }

    private void HideChoices()
    {
        foreach (GameObject choice in _choices)
        {
            choice.SetActive(false);
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;

        if (currentChoices.Count > _choices.Length)
        {
            Debug.LogError("More than choices given!");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            _choices[index].gameObject.SetActive(true);
            _choiceTexts[index].SetText(choice.text);
            index++;
        }

        for (int i = index; i < _choices.Length; i++)
        {
            _choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private void OnMakeChoice(int index)
    {
        if (_canContinueToNextLine)
            _currentStory.ChooseChoiceIndex(index);
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choices[0].gameObject);
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        _dialogueVariables.Variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning(variableName + " was found to be null");
        }
        return variableValue;
    }

    private void OnApplicationQuit() 
    {
        _dialogueVariables.SaveVariable();
    }
}

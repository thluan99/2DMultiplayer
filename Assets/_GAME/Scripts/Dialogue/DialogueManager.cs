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
    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject[] _choices;
    private TextMeshProUGUI[] _choiceTexts;
    private Story _currentStory;
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
    }

    private void Start() 
    {
        IsDialoguePlaying = false;
        _dialoguePanel.SetActive(false);
        _choiceTexts = new TextMeshProUGUI[_choices.Length];

        Observable.EveryUpdate()
            .Where(_ => HasContinueStoryInput() && 
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

    public void EnterDialogueMode(TextAsset inkJson)
    {
        _currentStory = new Story(inkJson.text);
        IsDialoguePlaying = true;
        _dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            _dialogueText.SetText(_currentStory.Continue());

            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void ExitDialogueMode()
    {
        IsDialoguePlaying = false;
        _dialoguePanel.SetActive(false);
        _dialogueText.SetText("");
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
        _currentStory.ChooseChoiceIndex(index);
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choices[0].gameObject);
    }
}

using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> Variables { get; private set; }

    private Story _globalVariablesStory;
    private const string SAVE_VARIABLES_KEY = "SaveVariableKey";

    public DialogueVariables(TextAsset loadGlobalJson)
    {
        _globalVariablesStory = new Story(loadGlobalJson.text);

        if (PlayerPrefs.HasKey(SAVE_VARIABLES_KEY))
        {
            string jsonState = PlayerPrefs.GetString(SAVE_VARIABLES_KEY);
            _globalVariablesStory.state.LoadJson(jsonState);
        }

        Variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in _globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = _globalVariablesStory.variablesState.GetVariableWithName(name);
            Variables.Add(name, value);
            Debug.Log("Init global dialogue variable! " + name + " = " + value );
        }
    }

    public void SaveVariable()
    {
        if (_globalVariablesStory != null)
        {
            VariablesToStory(_globalVariablesStory);
            PlayerPrefs.SetString(SAVE_VARIABLES_KEY, _globalVariablesStory.state.ToJson());
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (Variables.ContainsKey(name))
        {
            Variables.Remove(name);
            Variables.Add(name, value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in Variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}

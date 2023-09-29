using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UniRx;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset _inkJson;

    [Header("Emote Animator")]
    [SerializeField] private Animator _emoteAnimator;

    [Header("Visual Cube")]
    [SerializeField] private GameObject _visualCube;
    private ReactiveProperty<bool> _interactable;

    public void SetInteractable(bool value) => _interactable.Value = value;

    private void Awake() 
    {
        _interactable = new ReactiveProperty<bool>(false);
        _visualCube.SetActive(false);
    }

    private void Start() 
    {
        _interactable
            .Subscribe(value => _visualCube.SetActive(value))
            .AddTo(gameObject);
    }

    public void ShowInteract()
    {
        DialogueManager.Instance.EnterDialogueMode(_inkJson, _emoteAnimator);
        SetInteractable(false);
    }
}

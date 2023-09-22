using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private const float INTERACT_RANGE = 2F;
    private List<Collider2D> _interactableArray;
    private bool _hasInteractableObject => _interactableArray.Count > 0;
    private DialogueTrigger _currentInteractableSelect;

    private void Awake() 
    {
        _interactableArray = new List<Collider2D>();
    }

    private void Start() 
    {
        Observable.EveryUpdate()
            .Where(_ => !DialogueManager.Instance.IsDialoguePlaying &&
                    _hasInteractableObject &&
                    Input.GetKeyDown(KeyCode.F))
            .Subscribe(_ => Interact())
            .AddTo(gameObject);
    }

    private void Interact()
    {
        _currentInteractableSelect?.ShowInteract();
    }

    private void SelectInteractObject(List<Collider2D> colliders)
    {
        for (int i = 0; i < colliders.Count - 1; i++)
        {
            colliders[i].GetComponent<DialogueTrigger>().SetInteractable(false);
        }

        _currentInteractableSelect = colliders[colliders.Count - 1].GetComponent<DialogueTrigger>();
        _currentInteractableSelect.SetInteractable(true);
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!DialogueManager.Instance.IsDialoguePlaying && other.CompareTag("NPC"))
        {
            _interactableArray.Add(other);
            DisplayCanInteract();
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (!DialogueManager.Instance.IsDialoguePlaying && other.CompareTag("NPC"))
        {
            _interactableArray.Remove(other);
            other.GetComponent<DialogueTrigger>().SetInteractable(false);
        }
    }


    private void DisplayCanInteract()
    {
        if (_hasInteractableObject)
        {
            SelectInteractObject(_interactableArray);
        }
    }
}

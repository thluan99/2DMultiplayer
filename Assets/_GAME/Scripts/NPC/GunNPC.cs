using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GunNPC : MonoBehaviour
{
    [SerializeField] private Sprite _defaultGun;
    [SerializeField] private Sprite _akGun;
    [SerializeField] private Sprite _thompsonGun;
    [SerializeField] private Sprite _awpGun;

    private SpriteRenderer _spriteRenderer;

    private void Awake() 
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        string gun_name = ((Ink.Runtime.StringValue)DialogueManager.Instance.GetVariableState("type_gun_name")).value;

        switch (gun_name)
        {
            case "":
                _spriteRenderer.sprite = _defaultGun;
                break;
            case "AK-47":
                _spriteRenderer.sprite = _akGun;
                break;
            case "Thompson":
                _spriteRenderer.sprite = _thompsonGun;
                break;
            case "AWP":
                _spriteRenderer.sprite = _awpGun;
                break;
            default:
                Debug.LogWarning("Gun name not handled ! " + gun_name);
                break;
        }
    }
}

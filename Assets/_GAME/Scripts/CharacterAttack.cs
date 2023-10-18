using System.Collections;
using UniRx;
using UnityEngine;
using Mirror;
using UniRx.Triggers;

public class CharacterAttack : NetworkBehaviour
{
    [SerializeField] private GameObject _normalAttackSkill;
    [SerializeField] private GameObject _swordRainAttackSkill;
    private const string ATTACK_ANIM = "Attack";
    private const float TIME_TO_ATTACK = 0.3F;
    private PlayerObservable _playerObservable;
    private SpriteRenderer _spriteRenderer;
    private void Awake() 
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerObservable = GetComponent<PlayerObservable>();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.C) && !_playerObservable.isAttacking)
        {
            StartCoroutine(HandleAttack(_normalAttackSkill));
        }
        else if (Input.GetKeyDown(KeyCode.V) && !_playerObservable.isAttacking)
        {
            StartCoroutine(HandleAttack(_swordRainAttackSkill));
        }
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

    private IEnumerator HandleAttack(GameObject attackPrefab)
    {
        _playerObservable.AnimNeedPlay.OnNext(ATTACK_ANIM);;
        _playerObservable.isAttacking = true;

        SpawnAttackCommand(attackPrefab);

        yield return new WaitForSeconds(TIME_TO_ATTACK);
        AttackCompleted();
        _playerObservable.isAttacking = false;
    }

    GameObject attackObject;
    [Command]
    private void SpawnAttackCommand(GameObject attackPrefab)
    {
        attackObject = Instantiate(_swordRainAttackSkill);
        NetworkServer.Spawn(attackObject, connectionToClient);
        attackObject.transform.localScale = new Vector3(_spriteRenderer.flipX == false ? 1 : -1, 1, 1);
        attackObject.transform.position = transform.position;
        attackObject.GetComponent<AttackSkill>().ObjectId = connectionToClient.connectionId;
    }

    [Command]
    private void AttackCompleted()
    {
        NetworkServer.Destroy(attackObject);
        attackObject = null;
    }
}
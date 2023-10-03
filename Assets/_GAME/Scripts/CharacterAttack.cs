using System.Collections;
using UniRx;
using UnityEngine;
using Mirror;
using UniRx.Triggers;

public class CharacterAttack : NetworkBehaviour
{
    [SerializeField] private GameObject _normalAttackSkill;
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
            StartCoroutine(HandleAttack());
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

    private IEnumerator HandleAttack()
    {
        _playerObservable.AnimNeedPlay.OnNext(ATTACK_ANIM);;
        _playerObservable.isAttacking = true;
        
        SpawnAttackCommand();

        yield return new WaitForSeconds(TIME_TO_ATTACK);
        AttackCompleted();
        _playerObservable.isAttacking = false;
    }

    GameObject _normalAttack;
    [Command]
    private void SpawnAttackCommand()
    {
        _normalAttack = Instantiate(_normalAttackSkill);
        NetworkServer.Spawn(_normalAttack, connectionToClient);
        _normalAttack.transform.localScale = new Vector3(_spriteRenderer.flipX == false ? 1 : -1, 1, 1);
        _normalAttack.transform.position = transform.position;
        _normalAttack.GetComponent<NormalAttackSkill>().ObjectId = connectionToClient.connectionId;
    }

    [Command]
    private void AttackCompleted()
    {
        NetworkServer.Destroy(_normalAttack);
        _normalAttack = null;
    }
}
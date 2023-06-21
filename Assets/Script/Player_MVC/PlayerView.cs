using KrakenGamingTest.Player;
using KrakenGamingTest.ScriptableObjects.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject playerRagdoll;
    [SerializeField] private UIEvent uiEvent;
    [SerializeField] private GameObject ragdollPrefab;

    private Transform _playerVisual;
    private PlayerData _playerData;
    private GameObject _lastRagdollCreated;

    private bool _playerClimbing;

    private static readonly string VELOCITY_X = "VelocityX";
    private static readonly string VELOCITY_Y = "VelocityY";
    private static readonly string ON_GROUND = "OnGround";
    private static readonly string ON_STAIRS = "OnStairs";
    private static readonly string ON_CROUCH = "OnCrouch";
    private static readonly string JUMP_ANIMATION = "Jump";
    private static readonly string CROUCH_ANIMATION = "Crouch";
    private static readonly string ATTACK_ANIMATION = "Attack";

    private static readonly WaitForSeconds DELAY_DEAD = new WaitForSeconds(2);

    public void GetSubscriptionsEvents(PlayerModel player)
    {
        player.OnMove += MovementHandler;
        player.OnClimb += SetPlayerOnClimb;
        player.OnJump += TriggerJumpAnimation;
        player.OnGround += OnGroundHnadler;
        player.OnGetDamage += PlayerGetDamageHandler;
        player.OnRespawn += RespawnPlayerHandler;
        player.OnCrouch += CrouchAnimationHandler;
        player.UseSword += PlayerAttackHandler;
        _playerVisual = animator.transform;
        _playerData = player.GetPlayerData();
        SetPlayersHearts(player.GetPlayerData().playerLifes);
    }

    private void MovementHandler(Vector3 moveVector) 
    {
        if (!_playerClimbing)
        {
            animator.SetFloat(VELOCITY_X, moveVector.x);
            animator.SetFloat(VELOCITY_Y, 0);
            animator.SetBool(ON_STAIRS, false);
            animator.speed = 1;
        }

        if(_playerClimbing && moveVector == Vector3.zero)
        {
            animator.speed = 0;
            return;
        }

        if (_playerClimbing && (moveVector.y != 0 || moveVector.x !=0))
        {
            animator.SetFloat(VELOCITY_X, moveVector.x);
            animator.SetFloat(VELOCITY_Y, moveVector.y);
            animator.SetBool(ON_STAIRS, true);
            _playerVisual.rotation = Quaternion.Euler(0, 0, 0);
            animator.speed = 1;
            return;
        }

        if (moveVector.x > 0.1f)
        {
            _playerVisual.rotation = Quaternion.Euler(0, 90, 0);
            return;
        }            
        if(moveVector.x < -0.1f)
            _playerVisual.rotation = Quaternion.Euler(0, -90, 0);
    }

    private void PlayerAttackHandler()
    {
        animator.CrossFade(ATTACK_ANIMATION, 0.2f);
    }

    private void SetPlayerOnClimb(bool onClimb)
    {
        _playerClimbing = onClimb;
        animator.SetFloat(VELOCITY_Y, _playerClimbing? 1:0);
    }

    private void CrouchAnimationHandler(bool state)
    {
        if (state)
        {
            animator.CrossFade(CROUCH_ANIMATION, 0.2f);
            animator.SetBool(ON_CROUCH, state);
            return;
        }
        animator.SetBool(ON_CROUCH, state);
    }

    private void TriggerJumpAnimation()
    {
        animator.CrossFade(JUMP_ANIMATION, 0.2f);
    }

    private void OnGroundHnadler(bool state)
    {
        animator.SetBool(ON_GROUND, state);
    }

    private void PlayerGetDamageHandler(int lifes)
    {
        animator.gameObject.SetActive(false);
        var newRagdoll = Instantiate(ragdollPrefab);
        newRagdoll.transform.position = animator.transform.position;
        newRagdoll.transform.rotation = animator.transform.rotation;
        _lastRagdollCreated = newRagdoll;

        foreach (var ragdoll in newRagdoll.GetComponentsInChildren<Rigidbody>())
        {
            ragdoll.AddForce((-animator.transform.forward + Vector3.up) * _playerData.playerDeathForce, ForceMode.Impulse);
        }

        uiEvent.Raise(new UIParameters()
        {
            Command = UICommands.SET_PLAYERS_HEARTS,
            Value = lifes
        });
        StartCoroutine(DelayDead());
    }

    private IEnumerator DelayDead()
    {
        yield return DELAY_DEAD;
        uiEvent.Raise(new UIParameters()
        {
            Command = UICommands.FADE_SCREEN_IN,
            Value = 2
        });
    }

    private void SetPlayersHearts(int lifes)
    {       
        uiEvent.Raise(new UIParameters()
        {
            Command = UICommands.SET_PLAYERS_HEARTS,
            Value = lifes
        });
    }

    private void RespawnPlayerHandler()
    {
        animator.gameObject.SetActive(true);
        _playerVisual.rotation = Quaternion.Euler(0, -90, 0);
        Destroy(_lastRagdollCreated);
    }
}

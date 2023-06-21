using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GamePointsData gamePointsData;
    [SerializeField] private UIEvent uiEvent;
    [SerializeField] private PlayerModel playerModel;

    private int _currentScore;
    private int _bonusScore;
    private int _barrelsJumpedConsecutively;
    private float _barrelsJumpedConsecutivelyTimer;
    private bool _barrelsJumped;

    private void Start()
    {
        Initialize();
    }

    public void BarrelJumped()
    {
        _barrelsJumpedConsecutively++;
        _barrelsJumpedConsecutivelyTimer = gamePointsData.barrelsJumpedConsecutivelyTimer;

        if (_barrelsJumped)
            return;

        StartCoroutine(JumpBarrelsConsecutively());
        _barrelsJumped = true;
    }

    public void ShurikenEvade()
    {
        _currentScore += gamePointsData.evadeShurikenPoint;
        UpdateScoreUi();
    }

    private void Initialize()
    {
        _bonusScore = gamePointsData.bonusScore;
        UpdateBonusScoreUi();
        playerModel.OnGetDamage += PlayerLost;
        LevelEventsHandler.Instance.SubscribeToFadeOutEvent(StartGame);
        LevelEventsHandler.Instance.SubscribeToFadeInEvent(SetInitialBonusScore);
        LevelEventsHandler.Instance.PlayerWin += GetFinalScore;
    }

    private void PlayerLost(int value)
    {
        StopAllCoroutines();       
    }

    private void StartGame()
    {       
        StartCoroutine(SubstractBonusScore());
    }

    private void SetInitialBonusScore()
    {
        _bonusScore = gamePointsData.bonusScore;
        UpdateBonusScoreUi();
    }

    private void GetFinalScore()
    {
        _currentScore += _bonusScore;
        _bonusScore = 0;
        UpdateBonusScoreUi();
        UpdateScoreUi();
    }

    private IEnumerator SubstractBonusScore()
    {
        float timer = 1;
        UpdateBonusScoreUi();
        while (_bonusScore > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                _bonusScore -= gamePointsData.pointToSubstractPerSecond;
                UpdateBonusScoreUi();
                timer = 1;
            }
            yield return null;
        }
    }

    private IEnumerator JumpBarrelsConsecutively()
    {
        while (_barrelsJumpedConsecutivelyTimer > 0)
        {
            _barrelsJumpedConsecutivelyTimer -= Time.deltaTime;
            yield return null;
        }

        _currentScore += (gamePointsData.jumpBarrelPoint * _barrelsJumpedConsecutively) * _barrelsJumpedConsecutively;
        _barrelsJumpedConsecutively = 0;
        _barrelsJumped = false;
        UpdateScoreUi();
    }

    private void UpdateScoreUi()
    {
        uiEvent.Raise(new UIParameters()
        {
            Command = UICommands.CHANGE_SCORE,
            Value = _currentScore
        });
    }

    private void UpdateBonusScoreUi()
    {
        uiEvent.Raise(new UIParameters()
        {
            Command = UICommands.CHANGE_BONUS_SCORE,
            Value = _bonusScore
        });
    }
}

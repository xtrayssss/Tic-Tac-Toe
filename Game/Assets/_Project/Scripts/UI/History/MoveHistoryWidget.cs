using System.Collections.Generic;
using _Project.Scripts.Core.Services.Config;
using _Project.Scripts.Gameplay.Battle;
using _Project.Scripts.Gameplay.Battle.States;
using _Project.Scripts.UI.Elements;
using PrimeTween;
using UnityEngine;

namespace _Project.Scripts.UI.History
{
    public class MoveHistoryWidget : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        private MoveHistoryService _moveHistory;
        private readonly List<MoveHistoryElement> _history = new List<MoveHistoryElement>();
        private BattleStateMachine _battleStateMachine;
        private ConfigService _configService;

        private Tween _currentElementAnimation;
        private Tween _currentDestroyAnimation;

        public void Construct(
            MoveHistoryService moveHistory,
            BattleStateMachine gameStateMachine,
            ConfigService configService)
        {
            _configService = configService;
            _battleStateMachine = gameStateMachine;
            _moveHistory = moveHistory;
        }

        public void Initialize()
        {
            _moveHistory.OnMoveAdded += CreateRevertMoveElement;
            _moveHistory.OnMoveReverted += DestroyRevertMoveElement;

            ClearBoardButton clearBoardButton = Instantiate(_configService.ClearBoardButtonPrefab).Construct();
            
            MoveHistoryElement element = Instantiate(_configService.MoveHistoryElementPrefab, _content);
            element.Construct(clearBoardButton.Button);
            element.NumberedElement.Set(clearBoardButton, number: _moveHistory.History.Count + 1);
            _history.Add(element);
            
            clearBoardButton.Button.onClick.AddListener(() =>
            {
                if (_currentElementAnimation.isAlive)
                    return;

                _battleStateMachine
                    .Fire(BattleStateMachine.BattleEvent.CLEAR_BOARD);
            });
        }

        private void OnDisable()
        {
            _moveHistory.OnMoveAdded -= CreateRevertMoveElement;
            _moveHistory.OnMoveReverted -= DestroyRevertMoveElement;

            foreach (MoveHistoryElement element in _history)
                element.Button.onClick.RemoveAllListeners();
        }

        private async void CreateRevertMoveElement(MoveHistoryService.MoveInfo move)
        {
            await _currentDestroyAnimation;

            RevertMoveButton revertMoveButton =
                Instantiate(_configService.RevertRevertMoveButtonPrefab)
                    .Construct()
                    .SetText("MOVE #" + move.Index)
                    .AddListener(() => RevertToMove(move.Index));
            
            MoveHistoryElement element = Instantiate(_configService.MoveHistoryElementPrefab, _content);
            element.Construct(revertMoveButton.Button);

            element.NumberedElement.Set(
                revertMoveButton,
                number: move.Index + 1);

            _currentElementAnimation = element.CreationTween = Tween
                .Scale(
                    target: element.NumberedElement.Body.RectTransform,
                    startValue: Vector3.zero,
                    endValue: Vector3.one,
                    duration: 0.2f,
                    ease: Ease.OutBack);

            _history.Add(element);
        }

        private void DestroyRevertMoveElement(int moveIndex)
        {
            MoveHistoryElement element = _history[moveIndex + 1];
            element.CreationTween.Stop();

            _currentDestroyAnimation = _currentElementAnimation = Tween
                .Scale(
                    element.NumberedElement.Body.RectTransform,
                    endValue: Vector3.zero,
                    duration: 0.2f,
                    ease: Ease.InBack)
                .OnComplete(
                    element.gameObject,
                    static go => Destroy(go));
            
            _history.RemoveAt(moveIndex + 1);
        }

        private void RevertToMove(int moveIndex)
        {
            if (_currentElementAnimation.isAlive)
                return;

            _battleStateMachine
                .Fire(BattleStateMachine.BattleEvent.REVERT_MOVE,
                    new MoveRevertingState.Context(_battleStateMachine.Ctx.Token, moveIndex));
        }
    }
}
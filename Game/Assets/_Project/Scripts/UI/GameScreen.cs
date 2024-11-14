using _Project.Scripts.Core.Services;
using _Project.Scripts.Core.Services.Config;
using _Project.Scripts.Gameplay.Battle;
using _Project.Scripts.Gameplay.Players;
using _Project.Scripts.UI.Elements;
using _Project.Scripts.UI.History;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class GameScreen : MonoBehaviour, IService
    {
        [field: SerializeField] public RectTransform Board { get; private set; }
        [field: SerializeField] public MoveHistoryWidget MoveHistoryWidget { get; set; }
        [SerializeField] private SwapSymbolButton _swapSymbolButton;
        [SerializeField] private BattleStateWidget _battleStateWidget;
        [SerializeField] private GameTitle _gameTitle;
        public WinLineView WinLineView { get; set; }
        public bool IsPlayLikeButtonHided { get; private set; }
        private Canvas _canvas;
        private BattleStateMachine _battleStateMachine;
        private IPlayersService _playersService;

        public GameScreen Construct(
            Camera uiCamera,
            MoveHistoryService moveHistoryService,
            BattleStateMachine gameStateMachine,
            IPlayersService playersService,
            ConfigService configService)
        {
            _playersService = playersService;
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = uiCamera;
            _battleStateMachine = gameStateMachine;
            _gameTitle.Construct();
            
            MoveHistoryWidget.Construct(
                moveHistoryService, 
                _battleStateMachine,
                configService);
            
            _battleStateWidget.Construct();
            _swapSymbolButton.Construct();
            _battleStateWidget.Construct();

            return this;
        }

        public GameScreen Initialize()
        {
            MoveHistoryWidget.Initialize();
            
            return this;
        }

        public void OnEnable()
        {
            char player1Symbol = _playersService.Player1.SymbolSettings.Symbol;
            char player2Symbol = _playersService.Player2.SymbolSettings.Symbol;

            _swapSymbolButton.Text.text = $"\u27f2 {player1Symbol} \u27fa {player2Symbol}";

            _swapSymbolButton.Button.onClick.AddListener(SwapSymbols);
        }

        private void OnDisable() =>
            _swapSymbolButton.RemoveListeners();

        public void HideSwapSymbolsButton()
        {
            IsPlayLikeButtonHided = true;
            _swapSymbolButton.Disable();
            _swapSymbolButton.Fade(isFadingIn: false);
        }

        public void DisplayPlayLikeButton()
        {
            IsPlayLikeButtonHided = false;
            _swapSymbolButton.Enable();
            _swapSymbolButton.Fade(isFadingIn: true);
        }

        private void SwapSymbols()
        {
            IPlayersService playersService = AllServices.Instance.Get<IPlayersService>();

            playersService.SwapSymbols();
            HideSwapSymbolsButton();
        }

        public void PlayTitleBounceInDown() =>
            _gameTitle.PlayBounceInDown();

        public void DisplayMove(string text)
        {
            _battleStateWidget.SetText(text);
            _battleStateWidget.PlayAnimation();
        }

        public void DisplayBattleResult(string text)
        {
            _battleStateWidget.SetText(text);
            _battleStateWidget.StopAnimation();
            _battleStateWidget.PlayAnimation();
        }
    }
}
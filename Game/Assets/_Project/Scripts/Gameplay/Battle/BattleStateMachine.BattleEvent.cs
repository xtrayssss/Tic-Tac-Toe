namespace _Project.Scripts.Gameplay.Battle
{
    public partial class BattleStateMachine
    {
        public enum BattleEvent
        {
            MAKE_MOVE,
            REVERT_MOVE,
            END_BATTLE,
            CLEAR_BOARD,
            END_GAME
        }
    }
}
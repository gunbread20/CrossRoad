public enum GameState
{
    INIT, // 초기화 게임 처음 시작시 가장 먼저 한번 호출 
    SHOP,
    STANDBY, // Init 호출 후 게임 시작 화면, 혹은 게임 오버 후 다시 게임 시작 화면으로 돌아가면
    RUNNING, // 실제로 Tap 을 눌러 시작 했을 때
    OVER, // 게임 오버
    APPQUIT
    // Ex) GameState state = GameState.Init;
    // Game Cycle { init (only one time) - ( standby - running - over ) (loop) }
}
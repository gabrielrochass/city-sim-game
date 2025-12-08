namespace CitySim.Core
{
    /// <summary>
    /// Enumeração dos estados possíveis do jogo.
    /// Define o fluxo de estados do jogo durante sua execução.
    /// </summary>
    public enum GameState
    {
        /// <summary>Menu inicial do jogo</summary>
        MainMenu,
        
        /// <summary>Tela de instruções</summary>
        Instructions,
        
        /// <summary>Jogo em execução</summary>
        Playing,
        
        /// <summary>Jogo pausado</summary>
        Paused,
        
        /// <summary>Tela de game over (vitória)</summary>
        GameOverWin,
        
        /// <summary>Tela de game over (derrota)</summary>
        GameOverLose,
        
        /// <summary>Tela de configurações</summary>
        Settings
    }
}

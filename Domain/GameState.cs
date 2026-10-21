using System;

namespace Domain;

public class GameState
{

    private bool isGameOver = false;

    public int RequiredCardsToPlay { get; set; } = 2;
    public bool IsGameOver => isGameOver;
    public List<Move> MovesThisTurn { get; set; } = new List<Move>();

    public List<Player> Players { get; set; } = new List<Player>();
    public Deck Deck { get; set; } = new Deck();

    public Pile AscendingPile1 { get; set; } = new Pile();
    public Pile DescendingPile1 { get; set; } = new Pile();

    public Pile AscendingPile2 { get; set; } = new Pile();
    public Pile DescendingPile2 { get; set; } = new Pile();
    public Player CurrentPlayer { get; set; }
}

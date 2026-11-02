using System;
using Domain.Enums;

namespace Domain;

public class GameState
{

    public GameState()
    {
        AscendingPile1 = new Pile { TopCardValue = 1, Direction = PileDirection.Ascending };
        DescendingPile1 = new Pile { TopCardValue = 100, Direction = PileDirection.Descending };
        AscendingPile2 = new Pile { TopCardValue = 1, Direction = PileDirection.Ascending };
        DescendingPile2 = new Pile { TopCardValue = 100, Direction = PileDirection.Descending };
    }


    public int RequiredCardsToPlay { get; set; } = 2;
    public bool IsGameOver { get; set; } = false;
    public List<Move> MovesThisTurn { get; set; } = new List<Move>();

    public List<Player> Players { get; set; } = new List<Player>();
    public Deck Deck { get; set; } = new Deck();

    public Pile AscendingPile1 { get; set; } = new Pile();
    public Pile DescendingPile1 { get; set; } = new Pile();

    public Pile AscendingPile2 { get; set; } = new Pile();
    public Pile DescendingPile2 { get; set; } = new Pile();
    public Player CurrentPlayer { get; set; }
}

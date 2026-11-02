using System;
using Application;
using Domain;
using Domain.Enums;

namespace Tests;

public class GameEngineTests
{
    private readonly GameEngine _gameEngine = new(new MoveService());

    [Fact]
    public void ApplyMove_PlaysCardRemovesItFromHandAndRecordsMove()
    {
        var player = new Player();
        var card = new Card { ValueCard = 42 };
        player.Hand.Add(card);

        var pile = new Pile { TopCardValue = 10 };
        var move = new Move { Player = player, Card = card, TargetPile = pile };
        var gameState = new GameState { CurrentPlayer = player };

        _gameEngine.ApplyMove(gameState, move);

        Assert.Equal(42, pile.TopCardValue);
        Assert.DoesNotContain(card, player.Hand);
        Assert.Collection(gameState.MovesThisTurn, recordedMove => Assert.Same(move, recordedMove));
    }

    [Fact]
    public void ValidateMove_ReturnsValidForValidMove()
    {
        var player = new Player();
        var card = new Card { ValueCard = 15 };
        var pile = new Pile { TopCardValue = 10, Direction = PileDirection.Ascending };

        var gameState = new GameState { CurrentPlayer = player };
        var move = new Move { Player = player, Card = card, TargetPile = pile };
        var result = _gameEngine.ValidateMove(move);

        Assert.True(result.IsValid);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void ValidateMove_ReturnsInvalidForInvalidMove()
    {
        var player = new Player();
        var card = new Card { ValueCard = 15 };
        var pile = new Pile { TopCardValue = 10, Direction = PileDirection.Descending };

        var gameState = new GameState { CurrentPlayer = player };
        var move = new Move { Player = player, Card = card, TargetPile = pile };
        var result = _gameEngine.ValidateMove(move);

        Assert.False(result.IsValid);
        Assert.Equal("Invalid move.", result.ErrorMessage);
    }

    [Fact]
    public void ValidateMove_ReturnsValidForReverseMove()
    {
        var player = new Player();
        var card = new Card { ValueCard = 20 };
        var pile = new Pile { TopCardValue = 10, Direction = PileDirection.Descending };

        var gameState = new GameState { CurrentPlayer = player };
        var move = new Move { Player = player, Card = card, TargetPile = pile };
        var result = _gameEngine.ValidateMove(move);

        Assert.True(result.IsValid);

        card = new Card { ValueCard = 10 };
        pile = new Pile { TopCardValue = 20, Direction = PileDirection.Ascending };
        move = new Move { Player = player, Card = card, TargetPile = pile };
        result = _gameEngine.ValidateMove(move);

        Assert.True(result.IsValid);
    }
    [Fact]
    public void ValidateMove_ReturnsInValidForReverseMove()
    {
        var player = new Player();
        var card = new Card { ValueCard = 40 };
        var pile = new Pile { TopCardValue = 10, Direction = PileDirection.Descending };

        var gameState = new GameState { CurrentPlayer = player };
        var move = new Move { Player = player, Card = card, TargetPile = pile };
        var result = _gameEngine.ValidateMove(move);

        Assert.False(result.IsValid);

        card = new Card { ValueCard = 10 };
        pile = new Pile { TopCardValue = 30, Direction = PileDirection.Ascending };
        move = new Move { Player = player, Card = card, TargetPile = pile };
        result = _gameEngine.ValidateMove(move);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void StartTurn()
    {
        var player = new Player();
        var gameState = new GameState() { CurrentPlayer = player, Players = new List<Player>() { player } };
        _gameEngine.StartTurn(gameState);

        Assert.Equal(2, gameState.RequiredCardsToPlay);
        Assert.Empty(gameState.MovesThisTurn);
        Assert.Equal(gameState.CurrentPlayer, gameState.Players.FirstOrDefault());
    }

    [Fact]
    public void EndTurn_PlayerPlayedEnoughCards()
    {
        var player = new Player()
        {
            Hand = new List<Card>()
            {
                new Card(),
                new Card(),
                new Card(),
                new Card(),
                new Card(),
            }
        };
        var gameState = new GameState() { CurrentPlayer = player, Players = new List<Player>() { player } };
        gameState.MovesThisTurn.Add(new Move());
        gameState.MovesThisTurn.Add(new Move());

        var result = _gameEngine.EndTurn(gameState);

        Assert.True(result.IsValid);
        Assert.Equal(gameState.CurrentPlayer, gameState.Players.FirstOrDefault());
        Assert.Equal(7, gameState.CurrentPlayer.Hand.Count);
    }

    [Fact]
    public void EndTurn_PlayerDidntPlayEnoughMoves_ReturnsInvalidResult()
    {
        var player = new Player()
        {
            Hand = new List<Card>()
            {
                new Card(),
                new Card(),
                new Card(),
                new Card(),
                new Card(),
            }
        };
        var gameState = new GameState() { CurrentPlayer = player, Players = new List<Player>() { player } };
        gameState.MovesThisTurn.Add(new Move());

        var result = _gameEngine.EndTurn(gameState);

        Assert.False(result.IsValid);
        Assert.Equal("Not enough moves played this turn.", result.ErrorMessage);
        Assert.Equal(5, gameState.CurrentPlayer.Hand.Count);
    }

    [Fact]
    public void HasValidMoves()
    {
        var currentPlayer = new Player()
        {
            Hand = new List<Card>()
                {
                    new Card() { ValueCard = 10 },
                    new Card() { ValueCard = 20 },
                    new Card() { ValueCard = 30 },
                }
        };

        var gameState = new GameState()
        {
            CurrentPlayer = currentPlayer,
            Players = new List<Player>() { currentPlayer },
            AscendingPile1 = new Pile() { TopCardValue = 5, Direction = PileDirection.Ascending },
            DescendingPile1 = new Pile() { TopCardValue = 35, Direction = PileDirection.Descending },
            AscendingPile2 = new Pile() { TopCardValue = 15, Direction = PileDirection.Ascending },
            DescendingPile2 = new Pile() { TopCardValue = 25, Direction = PileDirection.Descending }
        };

        var validmoves = _gameEngine.HasValidMove(gameState);

        Assert.True(validmoves);
    }

    [Fact]
    public void HasNoValidMoves()
    {
        var currentPlayer = new Player()
        {
            Hand = new List<Card>()
                {
                    new Card() { ValueCard = 10 },
                    new Card() { ValueCard = 20 },
                    new Card() { ValueCard = 30 },
                }
        };

        var gameState = new GameState()
        {
            CurrentPlayer = currentPlayer,
            Players = new List<Player>() { currentPlayer },
            AscendingPile1 = new Pile() { TopCardValue = 99, Direction = PileDirection.Ascending },
            DescendingPile1 = new Pile() { TopCardValue = 5, Direction = PileDirection.Descending },
            AscendingPile2 = new Pile() { TopCardValue = 99, Direction = PileDirection.Ascending },
            DescendingPile2 = new Pile() { TopCardValue = 5, Direction = PileDirection.Descending }
        };

        var validmoves = _gameEngine.HasValidMove(gameState);

        Assert.False(validmoves);
    }


}

using System;
using Application;
using Domain;
using Domain.Enums;
using Domain.Exceptions;

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

        Assert.Equal(MoveResult.Valid, result);
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

        Assert.Equal(MoveResult.IsInvalid, result);
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

        Assert.Equal(MoveResult.Valid, result);

        card = new Card { ValueCard = 10 };
        pile = new Pile { TopCardValue = 20, Direction = PileDirection.Ascending };
        move = new Move { Player = player, Card = card, TargetPile = pile };
        result = _gameEngine.ValidateMove(move);

        Assert.Equal(MoveResult.Valid, result);
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

        Assert.Equal(MoveResult.IsInvalid, result);

        card = new Card { ValueCard = 10 };
        pile = new Pile { TopCardValue = 30, Direction = PileDirection.Ascending };
        move = new Move { Player = player, Card = card, TargetPile = pile };
        result = _gameEngine.ValidateMove(move);

        Assert.Equal(MoveResult.IsInvalid, result);
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

        _gameEngine.EndTurn(gameState);

        Assert.Equal(gameState.CurrentPlayer, gameState.Players.FirstOrDefault());
        Assert.Equal(7, gameState.CurrentPlayer.Hand.Count);
    }

    [Fact]
    public void EndTurn_PlayerDidntPlayEnoughMoves_ThrowsException()
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

        Assert.Throws<InvalidMoveException>(() => _gameEngine.EndTurn(gameState));
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

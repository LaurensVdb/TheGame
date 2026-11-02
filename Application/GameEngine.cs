using System;
using Domain;
using Domain.Enums;

namespace Application;

public interface IGameEngine
{
    void ApplyMove(GameState gameState, Move move);
    bool CheckGameOver(GameState state);
    ValidationResult EndTurn(GameState state);
    bool HasValidMove(GameState state);
    void StartTurn(GameState state);
    ValidationResult ValidateMove(Move move);
}

public class GameEngine : IGameEngine
{
    private readonly MoveService _moveService;

    public GameEngine(MoveService moveService)
    {
        _moveService = moveService;
    }
    public ValidationResult ValidateMove(Move move)
    {
        return _moveService.ValidateMove(move);
    }

    public void ApplyMove(GameState gameState, Move move)
    {
        move.TargetPile.Play(move.Card);

        gameState.CurrentPlayer.Hand.Remove(move.Card);
        gameState.MovesThisTurn.Add(move);
    }
    public bool HasValidMove(GameState state)
    {
        return _moveService.GetAllValidMoves(state).Any();
    }

    public void StartTurn(GameState state)
    {
        state.MovesThisTurn.Clear();
        state.RequiredCardsToPlay = 2;
        NextPlayer(state);
    }

    public ValidationResult EndTurn(GameState state)
    {
        var result = CheckIfRequiredMovesPlayed(state);
        if (!result.IsValid)
        {
            return result;
        }

        CheckPlayerHand(state);
        return result;
    }

    private void NextPlayer(GameState state)
    {
        var currentPlayerIndex = state.Players.IndexOf(state.CurrentPlayer);
        var nextPlayerIndex = (currentPlayerIndex + 1) % state.Players.Count;
        state.CurrentPlayer = state.Players[nextPlayerIndex];
    }

    public ValidationResult CheckIfRequiredMovesPlayed(GameState state)
    {
        if (state.MovesThisTurn.Count < state.RequiredCardsToPlay)
        {
            return new ValidationResult(false, "Not enough moves played this turn.");
        }

        return new ValidationResult(true);
    }
    public void CheckPlayerHand(GameState state)
    {
        while (state.CurrentPlayer.Hand.Count < 7)
        {
            var card = state.Deck.Draw();
            if (card == null)
            {
                break;
            }

            state.CurrentPlayer.Hand.Add(card);
        }
    }

    public bool CheckGameOver(GameState state)
    {
        return !HasValidMove(state);



    }
}

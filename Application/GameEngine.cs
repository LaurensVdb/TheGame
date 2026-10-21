using System;
using Domain;
using Domain.Enums;
using Domain.Exceptions;

namespace Application;

public interface IGameEngine
{
    void ApplyMove(GameState gameState, Move move);
    bool CheckGameOver(GameState state);
    void EndTurn(GameState state);
    bool HasValidMove(GameState state);
    void StartTurn(GameState state);
    MoveResult ValidateMove(Move move);
}

public class GameEngine : IGameEngine
{
    private readonly MoveService _moveService;

    public GameEngine(MoveService moveService)
    {
        _moveService = moveService;
    }
    public MoveResult ValidateMove(Move move)
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

    public void EndTurn(GameState state)
    {
        CheckIfRequiredMovesPlayed(state);
        CheckPlayerHand(state);

    }

    private void NextPlayer(GameState state)
    {
        var currentPlayerIndex = state.Players.IndexOf(state.CurrentPlayer);
        var nextPlayerIndex = (currentPlayerIndex + 1) % state.Players.Count;
        state.CurrentPlayer = state.Players[nextPlayerIndex];
    }

    private void CheckIfRequiredMovesPlayed(GameState state)
    {
        if (state.MovesThisTurn.Count < state.RequiredCardsToPlay)
        {
            throw new InvalidMoveException("Not enough moves played this turn.");
        }
    }
    private void CheckPlayerHand(GameState state)
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

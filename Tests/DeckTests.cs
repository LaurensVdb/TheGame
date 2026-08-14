using System;
using Domain;

namespace Tests;

public class DeckTests
{
    [Fact]
    public void ShuffleDeck()
    {
        var deck = new Deck();
        deck.Shuffle();

        Assert.Equal(98, deck.Cards.Count);

        var hasDuplicates = deck.Cards.GroupBy(x => x.ValueCard).Any(x => x.Count() > 1);
        Assert.False(hasDuplicates);
    }

    [Fact]
    public void DrawCardFromDeck()
    {
        var deck = new Deck();
        var card = deck.Draw();

        Assert.NotNull(card);
        Assert.Equal(97, deck.Cards.Count);
    }
}

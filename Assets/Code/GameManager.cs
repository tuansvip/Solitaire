using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject deck;
    public GameObject cardPrefabs;
    public DrawZone drawZone;
    public List<Bottom> bottom;
    public GameObject draggingCard;
    public List<GameObject> listDraggingBot;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 140;
        GenerateCards();
        SuffleDeck();
        deck.GetComponent<Deck>().setOffset();
        FirstDeal();
    }

    private void GenerateCards()
    {
        for(int i = 1; i <= 13; i++)
        {
            for (int suit = 1; suit <= 4; suit++)
            {
                GameObject newCard = Instantiate(cardPrefabs, deck.transform);
                newCard.GetComponent<Card>().cardValue = i;
                switch(suit)
                {
                    case 1:
                        newCard.GetComponent<Card>().suit = Card.Suit.Clubs;
                        newCard.name = i + "Clubs";
                        break;
                    case 2:
                        newCard.GetComponent<Card>().suit = Card.Suit.Diamonds;
                        newCard.name = i + "Diamonds";
                        break;
                    case 3:
                        newCard.GetComponent<Card>().suit = Card.Suit.Hearts;
                        newCard.name = i + "Hearts";
                        break;
                    case 4:
                        newCard.GetComponent<Card>().suit = Card.Suit.Spades;
                        newCard.name = i + "Spades";
                        break;
                    default:
                        break;
                }
                newCard.GetComponent<Card>().isInDeck = true;
                newCard.GetComponent<Card>().isFaceUp = false;
                newCard.GetComponent<Card>().isOnTop = false;
                newCard.GetComponent<Card>().isOnBottom = false;
                deck.GetComponent<Deck>().cards.Add(newCard);
            }
        }
    }
    public void SuffleDeck()
    {
        List<GameObject> listCard = deck.GetComponent<Deck>().cards;
        for (int i = 0; i < listCard.Count; i++)
        {
            GameObject temp = listCard[i];
            int randomIndex = UnityEngine.Random.Range(i, listCard.Count);
            listCard[i] = listCard[randomIndex];
            listCard[randomIndex] = temp;
        }
    }

    private void FirstDeal()
    {
        int count = 7;
        do
        {
            for (int i = 1; i <= count; i++)
            {

                bottom[i - 1].cards.Add(deck.GetComponent<Deck>().cards[deck.GetComponent<Deck>().cards.Count - 1]);
                bottom[i - 1].cards[bottom[i - 1].cards.Count - 1].transform.SetParent(bottom[i - 1].transform);
                deck.GetComponent<Deck>().cards.Remove(deck.GetComponent<Deck>().cards[deck.GetComponent<Deck>().cards.Count - 1]);
            }
            count--;
        }
        while (count > 0);

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

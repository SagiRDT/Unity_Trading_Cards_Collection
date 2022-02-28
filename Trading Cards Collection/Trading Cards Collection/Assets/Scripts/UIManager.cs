/*
 *  UIManager
 *  Handling the card UI functionality.
*/

using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    //=============================
    // Consts
    //=============================
    private const int CARDS_PER_PAGE = 12;

    //=============================
    // Serialize Fields
    //=============================
    // Serialize Field of the cards panel.
    [SerializeField] RectTransform cardsPanel = null;

    // Serialize Field of the search by name input field.
    [SerializeField] TMP_InputField searchByNameInputField = null;

    // Serialize Field of the page number text, for updating the page number.
    [SerializeField] TextMeshProUGUI pageNumberText = null;

    //=============================
    // Cached component references
    //=============================
    // An array with all the cards that are in our cards panel, for quick access to each card in our panel.
    private Card[] cardsList;

    // An list that will hold all the cards that that we got via searching a cost, a name etc.
    private List<Card> searchResultCardsList = new List<Card>();

    // Will hold the max pages for the panel when all the cards are loaded.
    private int maxPage;

    // Will hold the current page of the collection.
    private int curPage = 1;

    // Will hold the stat of the paging, search on/off.
    private bool isSearch = false;

    // Will hold the total cards we got while filttering by cost or name.
    private int curTotalCards;

    // Start is called before the first frame update
    void Start()
    {
        // Keep a list of all the cards that are in our card panel
        cardsList = cardsPanel.GetComponentsInChildren<Card>();

        // get the max page
        maxPage = Mathf.CeilToInt((float)cardsList.Length / CARDS_PER_PAGE);

        // Init the paging in page 1 with no filtters
        InitPaging();
    }

    // Enable/Disable cards according to a cost we got as param.
    // -1 will enable everything while 5 will enable all the 5+ cards.
    public void SearchByCost(int cost)
    {
        isSearch = true;
        curTotalCards = 0;  // will hold the total cards we found for this cost
        searchResultCardsList.Clear();  // Clear the search result list
        curPage = 1;    // Init the current page number, search result will always starts at page 1

        // if we didnt loaded any cards - do nothing
        if (cardsList == null) return;

        // if the cost is -1 init the paging at page 1 with no fillters
        if (cost == -1)
        {
            InitPaging();
            return;
        }

        // Loop over the cards that are in the cards panel, enable the cards that got the right cost and disable the rest
        foreach (Card card in cardsList)
        {
            if(card.GetCost() == cost || (cost == 5 && card.GetCost() >= 5))
            {
                //card.gameObject.SetActive(true);
                curTotalCards++;
                searchResultCardsList.Add(card);    // Add the card to the search result list
            }
            else
            {
                card.gameObject.SetActive(false);
            }
        }

        // Update the cards we view on the page
        UpdatePageCards();
        // Update the pages text
        UpdatePageNumberText();
    }

    // Enable only the card that got the card name that we got as param. Disable the rest.
    public void SearchByName()
    {
        isSearch = true;
        curTotalCards = 0;  // will hold the total cards we found for the name we're searching (will be 1 or 0)
        searchResultCardsList.Clear();  // Clear the search result list
        curPage = 1;    // Init the current page number, search result will always starts at page 1

        // if we didnt loaded any cards - do nothing
        if (cardsList == null) return;

        // get the searched name from the search by name input field
        string name = searchByNameInputField.text;

        // if the name is empty init the paging at page 1 with no fillters
        if (name == "")
        {
            InitPaging();
            return;
        }

        // Loop over the cards that are in the cards panel, enable the cards that got the right cost and disable the rest
        foreach (Card card in cardsList)
        {
            if (card.GetName().ToUpper() == name.ToUpper())
            {
                //card.gameObject.SetActive(true);
                curTotalCards++;
                searchResultCardsList.Add(card);    // Add the card to the search result list
            }
            else
            {
                card.gameObject.SetActive(false);
            }
        }

        // Update the cards we view on the page
        UpdatePageCards();
        // Update the pages text
        UpdatePageNumberText();
    }

    // Init the pages from page 1 and no searching fillters.
    public void InitPaging()
    {
        isSearch = false;
        curPage = 1;

        // Update the cards that are shown and the pages text
        UpdatePageCards();
        UpdatePageNumberText();
    }

    // Disable the cards in the card's panel.
    public void DisableAllCards()
    {
        foreach (Card card in cardsList)
        {
            card.gameObject.SetActive(false);
        }
    }

    // Go to the next page.
    public void NextPage()
    {
        int curMaxPage;

        if (isSearch)
        {
            curMaxPage = GetMaxPagePerSearch();
        }
        else
        {
            curMaxPage = maxPage;
        }

        if (curPage + 1 <= curMaxPage)
        {
            curPage++;
        }
        else
        {
            return;
        }
        
        // Update the cards that are shown and the pages text
        UpdatePageCards();
        UpdatePageNumberText();
    }

    // Go to the previous page.
    public void PreviousPage()
    {
        if (curPage - 1 >= 1)
        {
            curPage--;
        }
        else
        {
            return;
        }

        // Update the cards that are shown and the pages text
        UpdatePageCards();
        UpdatePageNumberText();
    }

    // Update the the pages text according to the search mode.
    // If the search mode is on the max page will be calc with the curTotalCards that we counted while filttering, 
    // else the max page is the regulat maxPage we calc for the entire collection.
    public void UpdatePageNumberText()
    {
        if(isSearch)
        {
            pageNumberText.text = curPage + "/" + GetMaxPagePerSearch();
        }
        else
        {
            pageNumberText.text = curPage + "/" + maxPage;
        }
    }

    // Update the cards that are shown on the panel according to the cur page.
    public void UpdatePageCards()
    {
        // Calc the start/end indexes of the cards that are in the cur page
        int startIndex = (curPage - 1) * CARDS_PER_PAGE;
        int endIndex = (curPage * CARDS_PER_PAGE) -1;

        // Select which cards list to scan according to isSearch bool - the list of all the cards or the list of the last search result
        Card[] listToScan;
        if (isSearch)
        {
            listToScan = searchResultCardsList.ToArray();
        }
        else
        {
            listToScan = cardsList;
        }

        // Loop over all the cards in the cards list(if isSearch is on we will loop over the search result list), 
        // enable the cards of the cur page and disable the rest
        for(int cardsListIndex = 0; cardsListIndex < listToScan.Length; cardsListIndex++)
        {
            if (cardsListIndex >= startIndex && cardsListIndex <= endIndex)
            {
                listToScan[cardsListIndex].gameObject.SetActive(true);
            }
            else
            {
                listToScan[cardsListIndex].gameObject.SetActive(false);
            }
        }
    }

    // Return the max page number for the curTotalCards we got.
    // curTotalCards value is updated in any of the search or reseting the view.
    public int GetMaxPagePerSearch()
    {
        int maxPagePerSearch = Mathf.CeilToInt((float)curTotalCards / CARDS_PER_PAGE);
        if (maxPagePerSearch == 0) return (maxPagePerSearch + 1);

        return maxPagePerSearch;
    }
}

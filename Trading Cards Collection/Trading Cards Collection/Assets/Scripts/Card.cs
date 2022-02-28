/*
 *  Card
 *  Handling the card UI representation.
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public enum CardType { UNIT, SPELL }

    //=============================
    // Serialize Fields
    //=============================
    [Header("Card info")]
    [SerializeField] int cardCost = 0;
    [SerializeField] string cardName = null;
    [SerializeField] string cardDescription = null;
    [SerializeField] CardType cardType = CardType.UNIT;
    [SerializeField] int health = 0;
    [SerializeField] int attackDamage = 0;
    [SerializeField] int spellPower = 0;

    [Header("References to card's objs")]
    // Serialize Field of the Image cost image, for enabling/disabling it and also getting a reference to its text.
    [SerializeField] Image costImage = null;

    // Serialize Field of the Image attack image, for enabling/disabling it and also getting a reference to its text.
    [SerializeField] Image attackImage = null;

    // Serialize Field of the Image health image, for enabling/disabling it and also getting a reference to its text.
    [SerializeField] Image healthImage = null;

    // Serialize Field of the Image spell power image, for enabling/disabling it and also getting a reference to its text.
    [SerializeField] Image spellPowerImage = null;

    // Serialize Field of the Image description image, for enabling/disabling it and also getting a reference to its text.
    [SerializeField] Image descriptionImage = null;

    //=============================
    // Cached component references
    //=============================
    // A reference to costText for changing the cost text of the card.
    private TextMeshProUGUI costText = null;

    // A reference to attackText for changing the attack text of the card.
    private TextMeshProUGUI attackText = null;

    // A reference to healthText for changing the health text of the card.
    private TextMeshProUGUI healthText = null;

    // A reference to spellPowerText for changing the spell power text of the card.
    private TextMeshProUGUI spellPowerText = null;

    // A reference to descriptionText for changing the description text of the card.
    private TextMeshProUGUI descriptionText = null;


    // Start is called before the first frame update
    void Start()
    {
        // Init the references
        InitReferences();

        // Init the card object with assests we assign to it in the editor
        InitCard();
    }

    public int GetCost() { return cardCost; }
    public string GetName() { return cardName; }

    // Init the class references
    private void InitReferences()
    {
        costText = costImage.GetComponentInChildren<TextMeshProUGUI>();
        attackText = attackImage.GetComponentInChildren<TextMeshProUGUI>();
        healthText = healthImage.GetComponentInChildren<TextMeshProUGUI>();
        spellPowerText = spellPowerImage.GetComponentInChildren<TextMeshProUGUI>();
        descriptionText = descriptionImage.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Init the card object with assests we assign to it in the editor
    public void InitCard()
    {
        // Card's Cost
        costText.text = cardCost.ToString();

        // Card's Description - will hold the name, and if its a spell card we will add it the spell effect
        descriptionText.text = cardName + "\n" + cardDescription;

        // Handle the extra data of the card according to the card's type
        switch (cardType)
        {
            case CardType.UNIT:
                // Card's Attack Damage
                attackText.text = attackDamage.ToString();
                // Card's Health
                healthText.text = health.ToString();
                // Disable the Spell Effect Amount Image
                spellPowerImage.gameObject.SetActive(false);
                break;

            case CardType.SPELL:
                // Card's Spell Power (if the spell power is 0 hide it)
                if(spellPower > 0)
                {
                    spellPowerText.text = spellPower.ToString();
                }
                else
                {
                    // Disable the Spell Effect Amount Image
                    spellPowerImage.gameObject.SetActive(false);
                }

                // Disable the Attack Damage and Health Images
                attackImage.gameObject.SetActive(false);
                healthImage.gameObject.SetActive(false);
                break;
        }
    }
    

}

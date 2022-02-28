/*
 *  HighlightObject
 *  Handling the functionality when the mouse is over/exiting the object.
 *  Inheriting from IPointerEnterHandler, IPointerExitHandler
*/

using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //=============================
    // Serialize Fields
    //=============================
    // Serialize Field of scaling factor.
    [Range(0,2)][SerializeField] float scaleFactor = 1.2f;

    //=============================
    // Cached component references
    //=============================
    // Will hold the original object local scale values, in order to return the object to his original size.
    private Vector3 cachedScale;

    // Start is called before the first frame update
    private void Start()
    {
        // keep the object original scale values
        cachedScale = transform.localScale;
    }

    // Get called when the mouse is over the object.
    // Will enlarge the object by scaleFactor size and play a sound.
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        GetComponent<AudioSource>().Play();
    }

    // Get called when the mouse is exiting the object.
    // Will return the object to his original size.
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = cachedScale;
    }
    
}

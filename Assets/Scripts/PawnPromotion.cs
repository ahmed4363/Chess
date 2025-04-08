using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnPromotion : MonoBehaviour
{
    public GameObject reference;
    public Canvas Canvas;
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }
    public void OnClick()
    {
        reference.name = this.name;
        reference.GetComponent<SpriteRenderer>().sprite = this.GetComponent<Image>().sprite;

        Destroy(GameObject.FindGameObjectWithTag("promo").transform.gameObject);
    }
}

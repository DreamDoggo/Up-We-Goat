using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketManager : MonoBehaviour
{
    [Tooltip("How long will the collectable take to reach the bottom of the basket?")]
    [SerializeField] float AnimationDuration;
    //[Tooltip("How small will the collectable get as it moves towards the basket?")]
    //[SerializeField] float ShrinkModifier;
    [SerializeField] Transform EndLocation;
    [SerializeField] AudioSource DropInBasketAudio;
    public Image image;
    public GameObject RefCollectable;
    private Vector2 StartPosition;
    private Color Transparent;
    private Color Opaque;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start()
    {
        StartPosition = transform.position;
        Transparent = new Color(image.color.r, image.color.g, image.color.b, 0);
        Opaque = new Color(image.color.r, image.color.g, image.color.b, 1);
        ResetAnimation();
    }

    public IEnumerator DropInBasket() 
    {
        ResetAnimation();

        float time = 0;
        //float startValue = ShrinkModifier;
        StartPosition = transform.position;
        //Vector2 startScale = transform.localScale;
        transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        image.color = Opaque;

        while (time < AnimationDuration)
        {
            transform.position = Vector2.Lerp(StartPosition, EndLocation.position, (time * 1.1f) / AnimationDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = EndLocation.position;
        DropInBasketAudio.Play();
        ResetAnimation();
    }

    void ResetAnimation() 
    {
        image.color = Transparent;
        transform.position = StartPosition;
        transform.rotation = Quaternion.identity;
    }

    public void ConvertCollectableToImage(GameObject collectable) 
    {
        RefCollectable = collectable;
        image.sprite = RefCollectable.GetComponent<SpriteRenderer>().sprite;
    }


}

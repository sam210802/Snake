using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicSizeSliderManager : MonoBehaviour
{
    [SerializeField]
    RectTransform mainTransform;
    [SerializeField]
    RectTransform textTransform;    
    [SerializeField]
    RectTransform sliderTransform;

    [SerializeField]
    int sliderMinWidth = 250;
    [SerializeField]
    VerticalLayoutGroup layoutGroup;

    [SerializeField]
    List<Slider> sliders;

    void Start() {
        updateSize();
    }

    void Update() {
        if (mainTransform.hasChanged) {
            // Debug.Log("Main Transform Changed");
            updateSize();
            mainTransform.hasChanged = false;
        } else if (textTransform.hasChanged) {
            // Debug.Log("Text Transform Changed");
            updateSize();
            textTransform.hasChanged = false;
        } else if (sliderTransform.hasChanged) {
            // Debug.Log("Slider Transform Changed");
            updateSize();
            sliderTransform.hasChanged = false;
        }
    }

    void updateSize() {
        if (textTransform.rect.width > sliderMinWidth) {
            sliderTransform.sizeDelta = new Vector2(textTransform.rect.width, (textTransform.rect.width/sliderTransform.rect.width)*sliderTransform.rect.height);
            mainTransform.sizeDelta = new Vector2(textTransform.rect.width, (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));

            if (sliders != null) normaliseSlider();

            Debug.Log("Text bigger than " + sliderMinWidth + "px");
        } else {
            sliderTransform.sizeDelta = new Vector2(sliderMinWidth, (sliderMinWidth/sliderTransform.rect.width)*sliderTransform.rect.height);
            Debug.Log("Text smaller than " + sliderMinWidth + "px");
        }
        Debug.Log("Updated Size");
    }

    // sets slider size to same as largest slider
    void normaliseSlider() {
        foreach (Slider slider in sliders) {
            float sliderWidth = slider.GetComponent<RectTransform>().rect.width;
            float sliderHeight = slider.GetComponent<RectTransform>().rect.height;
            if (sliderWidth > sliderTransform.rect.width) {
                sliderTransform.sizeDelta = new Vector2(sliderWidth, sliderHeight);
                mainTransform.sizeDelta = new Vector2(Mathf.Max(sliderWidth, textTransform.rect.width), (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
                Debug.Log("Normalised");
            }
        }
    }
}

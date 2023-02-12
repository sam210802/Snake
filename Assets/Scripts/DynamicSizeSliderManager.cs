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
    RectTransform topParent;

    [SerializeField]
    int sliderMinWidth = 250;
    [SerializeField]
    VerticalLayoutGroup layoutGroup;

    [SerializeField]
    List<Slider> sliders;

    private float time = 0.0f;
    private float updateTime = 1.0f;

    void Start() {
        updateSize();
    }

    void Update() {
        time += Time.deltaTime;
        if (time < updateTime) return; // code below only exectued when updateTime seconds passed
        
        time -= updateTime;

        if (mainTransform.hasChanged) {
            updateSize();
            mainTransform.hasChanged = false;
        } else if (textTransform.hasChanged) {
            updateSize();
            textTransform.hasChanged = false;
        } else if (sliderTransform.hasChanged) {
            updateSize();
            sliderTransform.hasChanged = false;
        }
    }

    void updateSize() {
        if (textTransform.rect.width > sliderMinWidth) {
            sliderTransform.sizeDelta = new Vector2(textTransform.rect.width, (textTransform.rect.width/sliderTransform.rect.width)*sliderTransform.rect.height);
            mainTransform.sizeDelta = new Vector2(textTransform.rect.width, (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));

            if (sliders != null) normaliseSlider();
        } else {
            sliderTransform.sizeDelta = new Vector2(sliderMinWidth, (sliderMinWidth/sliderTransform.rect.width)*sliderTransform.rect.height);
            mainTransform.sizeDelta = new Vector2(sliderMinWidth, (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(topParent);
    }

    // sets slider size to same as largest slider
    void normaliseSlider() {
        foreach (Slider slider in sliders) {
            float sliderWidth = slider.GetComponent<RectTransform>().rect.width;
            float sliderHeight = slider.GetComponent<RectTransform>().rect.height;
            if (sliderWidth > sliderTransform.rect.width) {
                sliderTransform.sizeDelta = new Vector2(sliderWidth, sliderHeight);
                mainTransform.sizeDelta = new Vector2(Mathf.Max(sliderWidth, textTransform.rect.width), (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
            }
        }
    }
}

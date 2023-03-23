using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class DynamicSizeSliderManager : MonoBehaviour
{
    [SerializeField]
    RectTransform mainTransform;
    [SerializeField]
    RectTransform textTransform;    
    [SerializeField]
    RectTransform sliderTransform;

    [SerializeField]
    RectTransform parentLayout;

    [SerializeField]
    int sliderMinWidth = 250;
    [SerializeField]
    VerticalLayoutGroup layoutGroup;

    [SerializeField]
    List<Slider> sliders;

    private float currentFontSize;

    private TMP_Text textComponent;
    private string currentText = "";

    private bool active = false;

    void Start() {
        textComponent = textTransform.GetComponent<TMP_Text>();
        currentFontSize = textComponent.fontSize;
    }

    void OnEnable() {
        update();
    }

    void Update() {
        // update slider size if font has changed size
        if (textComponent.fontSize != currentFontSize) {
            currentFontSize = textComponent.fontSize;
            update();
        }
        // update slider size if text has changed
        if (textComponent.text != currentText) {
            currentText = textComponent.text;
            update();
        }
    }

    private void update() {
        textTransform.GetComponent<ResizableTextManager>().updateTextSize();
        updateSize();
        normaliseSlider();
        forceLayoutRebuild();
    }

    private void updateSize() {
        if (textTransform.rect.width > sliderMinWidth) {
            // makes slider width equal to text width
            sliderTransform.sizeDelta = new Vector2(textTransform.rect.width, (textTransform.rect.width/sliderTransform.rect.width)*sliderTransform.rect.height);
            // makes parent fit content
            mainTransform.sizeDelta = new Vector2(textTransform.rect.width, (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
        } else {
            // makes slider width equal to minimum slider width
            sliderTransform.sizeDelta = new Vector2(sliderMinWidth, (sliderMinWidth/sliderTransform.rect.width)*sliderTransform.rect.height);
            // makes parent fit content
            mainTransform.sizeDelta = new Vector2(sliderMinWidth, (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
        }
    }

    // sets all slider sizes to same as largest slider size
    private void normaliseSlider() {
        if (sliders == null) return;

        foreach (Slider slider in sliders) {
            float sliderWidth = slider.GetComponent<RectTransform>().rect.width;
            float sliderHeight = slider.GetComponent<RectTransform>().rect.height;
            if (sliderWidth > sliderTransform.rect.width) {
                sliderTransform.sizeDelta = new Vector2(sliderWidth, sliderHeight);
                mainTransform.sizeDelta = new Vector2(Mathf.Max(sliderWidth, textTransform.rect.width), (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
            }
        }
    }

    // forces parent layout rebuild
    private void forceLayoutRebuild() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentLayout);
    }
}

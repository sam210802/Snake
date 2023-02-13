using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start() {
        currentFontSize = textTransform.GetComponent<TMP_Text>().fontSize;
        startUpdateSizeCoroutine();
    }

    void OnEnable() {
        startUpdateSizeCoroutine();
    }

    void Update() {
        // update slider size if font has changed size
        if (textTransform.GetComponent<TMP_Text>().fontSize != currentFontSize) {
            currentFontSize = textTransform.GetComponent<TMP_Text>().fontSize;
            startUpdateSizeCoroutine();
        }
    }

    public void startUpdateSizeCoroutine() {
        StartCoroutine(coroutines());
    }

    private IEnumerator coroutines() {
        yield return StartCoroutine(textTransform.GetComponent<ResizableTextManager>().coroutines());
        yield return StartCoroutine(updateSize());
        yield return StartCoroutine(normaliseSlider());
        yield return StartCoroutine(forceLayoutRebuild());
    }

    private IEnumerator updateSize() {
        if (textTransform.rect.width > sliderMinWidth) {
            sliderTransform.sizeDelta = new Vector2(textTransform.rect.width, (textTransform.rect.width/sliderTransform.rect.width)*sliderTransform.rect.height);
            mainTransform.sizeDelta = new Vector2(textTransform.rect.width, (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
        } else {
            sliderTransform.sizeDelta = new Vector2(sliderMinWidth, (sliderMinWidth/sliderTransform.rect.width)*sliderTransform.rect.height);
            mainTransform.sizeDelta = new Vector2(sliderMinWidth, (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
        }
        yield return null;
    }

    // sets slider size to same as largest slider
    private IEnumerator normaliseSlider() {
        if (sliders == null) yield break;

        foreach (Slider slider in sliders) {
            float sliderWidth = slider.GetComponent<RectTransform>().rect.width;
            float sliderHeight = slider.GetComponent<RectTransform>().rect.height;
            if (sliderWidth > sliderTransform.rect.width) {
                sliderTransform.sizeDelta = new Vector2(sliderWidth, sliderHeight);
                mainTransform.sizeDelta = new Vector2(Mathf.Max(sliderWidth, textTransform.rect.width), (textTransform.rect.height+layoutGroup.spacing+sliderTransform.rect.height));
            }
        }
        yield return null;
    }

    private IEnumerator forceLayoutRebuild() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentLayout);
        yield return null;
    }
}

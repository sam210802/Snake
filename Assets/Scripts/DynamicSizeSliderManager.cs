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
        startUpdateSizeCoroutine();
    }

    void Update() {
        // update slider size if font has changed size
        if (textComponent.fontSize != currentFontSize) {
            currentFontSize = textComponent.fontSize;
            startUpdateSizeCoroutine();
        }
        // update slider size if text has changed
        if (textComponent.text != currentText) {
            currentText = textComponent.text;
            startUpdateSizeCoroutine();
        }
    }

    public void startUpdateSizeCoroutine() {
        if (active) return;
        StartCoroutine(coroutines());
    }

    private IEnumerator coroutines() {
        active = true;
        yield return StartCoroutine(textTransform.GetComponent<ResizableTextManager>().coroutines());
        yield return StartCoroutine(updateSize());
        yield return StartCoroutine(normaliseSlider());
        yield return StartCoroutine(forceLayoutRebuild());
        active = false;
    }

    private IEnumerator updateSize() {
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
        yield return null;
    }

    // sets all slider sizes to same as largest slider size
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

    // forces parent layout rebuild
    private IEnumerator forceLayoutRebuild() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentLayout);
        yield return null;
    }
}

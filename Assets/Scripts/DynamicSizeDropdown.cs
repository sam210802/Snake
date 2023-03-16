using System.Linq;
using TMPro;
using UnityEngine;

public class DynamicSizeDropdown : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        var listOfOptions = dropdown.options.Select(OptionsMenu => OptionsMenu.text).ToList();
        dropdown.value = listOfOptions.IndexOf(OptionsMenu.loadTextPrefs());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

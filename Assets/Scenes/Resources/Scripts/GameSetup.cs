using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSetup : MonoBehaviour
{
    public string kind;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    // Start is called before the first frame update
    void Start()
    {
        _sliderText.text = kind + " count " + (int)GameObject.Find ("Slider_"+kind).GetComponent <Slider> ().value;
        _slider.onValueChanged.AddListener((v) => {
            _sliderText.text = kind + " count " + v.ToString();
        });
    }
}

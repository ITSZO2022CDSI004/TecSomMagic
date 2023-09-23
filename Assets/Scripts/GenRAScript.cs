using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenRAScript : MonoBehaviour
{

    [SerializeField]
    private GameObject ga;
    [SerializeField]
    private AudioClip ac;




    // Start is called before the first frame update
    void Start()
    {
        //textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        //textMeshProUGUI.text = "Hola";
        TextMeshProUGUI textMeshProUGUI = ga.GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = " ";
        Debug.Log("Soy GenRA");

    }

    public void setTitulo(string texto) {
        TextMeshProUGUI textMeshProUGUI = ga.GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = texto;
    }





}

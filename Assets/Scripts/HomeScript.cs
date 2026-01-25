using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour
{
    public TMP_InputField addressInput;
    public TMP_InputField portInput;
    void Start()
    {
        addressInput.text = PlayerPrefs.GetString("Address", "192.168.0.1");
        portInput.text = PlayerPrefs.GetString("Port", "8080");
    }

    public void OnButtonClick()
    {
        PlayerPrefs.SetString("Address", addressInput.text);
        PlayerPrefs.SetString("Port", portInput.text);
        SceneManager.LoadScene("SampleScene");
    }
}

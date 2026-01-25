using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReaderScript : MonoBehaviour
{
    public Canvas canvas;
    public Image image;
    public TMP_Text addressInput;
    public TMP_Text portInput;
    public RectTransform rectTransform;
    public RawImage background;
    public TMP_InputField colorInputField;
    public GameObject backgroundSettings;
    public RectTransform menuPanel;
    public TMP_Text menuPopupText;
    string address;
    string port;
    float checkInterval = 0f;
    public bool move = false;

    void Start()
    {
        address = PlayerPrefs.GetString("Address", "127.0.0.1");
        port = PlayerPrefs.GetString("Port", "8080");
        addressInput.text = address;
        portInput.text = port;
        colorInputField.text = PlayerPrefs.GetString("Color", "#141414");
        StartCoroutine(CheckForImageUpdate());
        SetSize();
    }
    void SetSize()
    {
        if (rectTransform != null)
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float scaleFactor = canvas.scaleFactor;
            rectTransform.sizeDelta = new Vector2(screenWidth / scaleFactor, screenHeight / scaleFactor);
        }
    }

    void Update()
    {
        SetSize();
        if (background.color.ToString() != colorInputField.text)
        {
            background.color = ColorUtility.TryParseHtmlString(colorInputField.text, out Color color) ? color : background.color;
            PlayerPrefs.SetString("Color", colorInputField.text);
        }
    }

    private IEnumerator LoadImageFromUrl()
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://" + address + ":" + port + "/texture"))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Destroy(image.sprite);
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
                StopCoroutine(LoadImageFromUrl());
            }
        }
    }

    private IEnumerator CheckForImageUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            StartCoroutine(LoadImageFromUrl());
        }
    }

    public void moveImage()
    {
        move = !move;
    }

    public void resetImage()
    {
        SetSize();
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void refreshImage()
    {
        image.sprite = null;
        StopCoroutine(LoadImageFromUrl());
        StartCoroutine(LoadImageFromUrl());
    }

    public void setBackground()
    {
        backgroundSettings.SetActive(!backgroundSettings.activeSelf);
    }

    public void popupMenu()
    {
        if (menuPanel.anchoredPosition != new Vector2(menuPanel.anchoredPosition.x, -125))
        {
            menuPanel.anchoredPosition = new Vector2(menuPanel.anchoredPosition.x, -125);
            menuPopupText.text = "▲";
        }
        else
        {
            menuPanel.anchoredPosition = new Vector2(menuPanel.anchoredPosition.x, 125);
            menuPopupText.text = "▼";
        }
    }

    public void exit()
    {
        SceneManager.LoadScene("HomeScene");
    }
}

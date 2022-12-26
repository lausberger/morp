using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
  public static UIManager instance;

  public GameObject startMenu;
  public TMP_InputField usernameField;

  public void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else if (instance != this)
    {
      Debug.Log("UIManager instance already exists. Destroying.");
      Destroy(this);
    }
  }

  public void ConnectToServer()
  {
    startMenu.SetActive(false);
    usernameField.interactable = false;
    Client.instance.ConnectToServer();
  }
}

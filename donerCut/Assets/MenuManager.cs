using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadScene("fruitninja");
    }
}

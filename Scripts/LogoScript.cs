using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoScript : MonoBehaviour
{
    public Image logoImage;
    //public string loadLevel;

    IEnumerator Start()
    {
        logoImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void FadeIn()
    {
        logoImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut()
    {
        logoImage.CrossFadeAlpha(0.0f, 2.5f, false);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class FadeScreen : MonoBehaviour
{
    public enum SkyBoxType
    {
        Space
    }

    public Material spaceSkyBox;

    public Camera centerEye;

    public SkyBoxType skyBox = SkyBoxType.Space;
    public bool fadeonStart = false;
    public float duration = 2;
    public Color fadeColor;
    private Renderer renderer;

    [SerializeField] UnityEvent fadeInSpace;
    [SerializeField] UnityEvent fadeOutSpace;

    private void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        renderer = GetComponent<Renderer>();
        if (fadeonStart)
        {
            FadeIn();
        }
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneRoutine(sceneName));
    }

    public void ChangeSceneFast(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator ChangeSceneRoutine(string sceneName)
    {
        SingleFadeOut();
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator ChangeSceneFastRoutine(string sceneName)
    {
        SingleFadeOut();
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(sceneName);
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void FadeLedgeOne()
    {
        FadeLedgeOne(0, 1);
    }

    public void FadeLedgeTwo()
    {
        FadeLedgeTwo(0, 1);
    }

    public void FadeLedgeOne(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeLedgeRoutineOne(alphaIn, alphaOut));
    }

    public void FadeLedgeTwo(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeLedgeRoutineTwo(alphaIn, alphaOut));
    }

    public UnityEvent onLedgeFadeOneComplete;
    public UnityEvent onLedgeFadeTwoComplete;

    IEnumerator FadeLedgeRoutineOne(float alphaIn, float alphaOut)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        float timer = 0;
        while (timer < duration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / duration);
            renderer.material.SetColor("_Color", newColor);
            timer += Time.deltaTime;
            yield return null;
        }

        onLedgeFadeOneComplete?.Invoke();

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        renderer.material.SetColor("_Color", newColor2);
        Fade(1, 0);
    }

    IEnumerator FadeLedgeRoutineTwo(float alphaIn, float alphaOut)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        float timer = 0;
        while (timer < duration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / duration);
            renderer.material.SetColor("_Color", newColor);
            timer += Time.deltaTime;
            yield return null;
        }

        onLedgeFadeTwoComplete?.Invoke();

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        renderer.material.SetColor("_Color", newColor2);

        Fade(1, 0);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private void SingleFadeOut()
    {
        StartCoroutine(SingleFadeOutRoutine(0, 1));
    }

    public IEnumerator SingleFadeOutRoutine(float alphaIn, float alphaOut)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;

        float timer = 0;
        while (timer < duration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / duration);

            renderer.material.SetColor("_Color", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        renderer.material.SetColor("_Color", newColor2);
    }

    public UnityEvent onFadeComplete;


    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;

        float timer = 0;
        while (timer < duration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer/duration);

            renderer.material.SetColor("_Color", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        onFadeComplete?.Invoke();

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        renderer.material.SetColor("_Color", newColor2);

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void ChangeSkyboxMaterial()
    {
        StartCoroutine(ChangeSkyBox());
    }

    public void ChangeRoom()
    {
        StartCoroutine(BackToRoom());
    }

    public void End()
    {
        StartCoroutine(BlackOut());
    }

    IEnumerator ChangeSkyBox()
    {
        FadeOut();
        yield return new WaitForSeconds(duration);
        fadeInSpace?.Invoke();
        //RenderSettings.skybox = spaceSkyBox;
        FadeIn();
    }

    IEnumerator BackToRoom()
    {
        FadeOut();
        yield return new WaitForSeconds(duration);
        fadeOutSpace?.Invoke();
        FadeIn();
    }

    IEnumerator BlackOut()
    {
        FadeOut();
        yield return new WaitForSeconds(duration);
    }
}


using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SettingsManager : MonoBehaviour
{
    public DynamicMoveProvider moveProvider;
    public ActionBasedContinuousTurnProvider turnProvider;

    public Slider movementSlider;
    public Slider rotationSlider;
    public Slider bgmSlider;

    public TMP_Text movementText;
    public TMP_Text rotationText;
    public TMP_Text bgmText;

    public AudioSource bgmSource;
    public GameObject rightRay;
    public GameObject leftRay;

    void Start()
    {
        // Convert SYSTEM VALUE → SLIDER VALUE
        movementSlider.value = moveProvider.moveSpeed * 10f;
        rotationSlider.value = turnProvider.turnSpeed;
        bgmSlider.value = bgmSource.volume * 100f;

        UpdateMovement(movementSlider.value);
        UpdateRotation(rotationSlider.value);
        UpdateBGM(bgmSlider.value);

        movementSlider.onValueChanged.AddListener(UpdateMovement);
        rotationSlider.onValueChanged.AddListener(UpdateRotation);
        bgmSlider.onValueChanged.AddListener(UpdateBGM);
    }

    void UpdateMovement(float sliderValue)
    {
        // Slider 0-100 → MoveSpeed 0-10
        moveProvider.moveSpeed = sliderValue / 10f;

        movementText.text = sliderValue.ToString("0");
    }

    void UpdateRotation(float sliderValue)
    {
        // Slider langsung sama dengan turnSpeed
        turnProvider.turnSpeed = sliderValue;

        rotationText.text = sliderValue.ToString("0");
    }

    void UpdateBGM(float sliderValue)
    {
        bgmSource.volume = sliderValue / 100f;
        bgmText.text = sliderValue.ToString("0");
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        int guiLayer = LayerMask.NameToLayer("GUI");

        var hands = FindObjectsOfType<MiddleFingerEvent>();

        foreach (var hand in hands)
        {
            SetLayerRecursively(hand.gameObject, guiLayer);
        }

        rightRay.SetActive(true);
        leftRay.SetActive(true);

        rightRay.GetComponent<XRInteractorLineVisual>().enabled = true;
        leftRay.GetComponent<XRInteractorLineVisual>().enabled = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;

        int guiLayer = LayerMask.NameToLayer("Default");

        var hands = FindObjectsOfType<MiddleFingerEvent>();

        foreach (var hand in hands)
        {
            SetLayerRecursively(hand.gameObject, guiLayer);
        }

        rightRay.SetActive(false);
        leftRay.SetActive(false);

        rightRay.GetComponent<XRInteractorLineVisual>().enabled = false;
        leftRay.GetComponent<XRInteractorLineVisual>().enabled = false;
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
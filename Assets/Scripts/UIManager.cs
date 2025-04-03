using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform sprintBar;
    public RectTransform cooldownBar;
    public Image sprintFill;
    public Image cooldownFill;

    [Header("Animation Settings")]
    public float uiAnimationSpeed = 0.4f; // Adjustable animation speed

    private Coroutine hideSprintCoroutine;
    private Coroutine showSprintCoroutine;
    private Coroutine hideCooldownCoroutine;
    private Coroutine cooldownFillCoroutine;

    private bool isSprintVisible = false;
    private bool isCooldownVisible = false;
    private bool isSprinting = false;

    void Start()
    {
        sprintBar.anchoredPosition = new Vector2(0, -100);
        cooldownBar.anchoredPosition = new Vector2(0, -100);
        cooldownFill.fillAmount = 0;
    }

    public void UpdateSprintBar(float fillAmount, bool sprinting)
    {
        isSprinting = sprinting;
        sprintFill.fillAmount = fillAmount;

        if (fillAmount > 0 && sprinting)
        {
            ShowSprintBar();
        }
        else if (fillAmount <= 0)
        {
            HideSprintBarSmooth();
        }

        if (fillAmount >= 1f && !sprinting)
        {
            HideSprintBarSmooth();
        }
    }

    public void ShowSprintBar()
    {
        if (isSprintVisible) return;
        isSprintVisible = true;
        StopHideSprintBarAnimation();

        if (showSprintCoroutine != null) StopCoroutine(showSprintCoroutine);
        showSprintCoroutine = StartCoroutine(MoveUI(sprintBar, new Vector2(0, 50), uiAnimationSpeed));
    }

    public void HideSprintBarSmooth()
    {
        if (!isSprintVisible) return;

        if (hideSprintCoroutine != null) StopCoroutine(hideSprintCoroutine);
        hideSprintCoroutine = StartCoroutine(HideSprintBarCoroutine());
    }

    private IEnumerator HideSprintBarCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        yield return MoveUI(sprintBar, new Vector2(0, -100), uiAnimationSpeed);
        isSprintVisible = false;
    }

    public void HideSprintBarInstant()
    {
        if (hideSprintCoroutine != null) StopCoroutine(hideSprintCoroutine);
        sprintBar.anchoredPosition = new Vector2(0, -100);
        isSprintVisible = false;
    }

    public void StopHideSprintBarAnimation()
    {
        if (hideSprintCoroutine != null)
        {
            StopCoroutine(hideSprintCoroutine);
            hideSprintCoroutine = null;
        }
    }

    public void ShowCooldownBar()
    {
        if (!isCooldownVisible)
        {
            isCooldownVisible = true;
            if (hideCooldownCoroutine != null) StopCoroutine(hideCooldownCoroutine);
            StartCoroutine(MoveUI(cooldownBar, new Vector2(0, 50), uiAnimationSpeed));
            StartCooldownFill();
        }
    }

    public void HideCooldownBar()
    {
        if (hideCooldownCoroutine != null) StopCoroutine(hideCooldownCoroutine);
        hideCooldownCoroutine = StartCoroutine(HideCooldownBarCoroutine());
    }

    private IEnumerator HideCooldownBarCoroutine()
    {
        yield return MoveUI(cooldownBar, new Vector2(0, -100), uiAnimationSpeed);
        isCooldownVisible = false;
    }

    private IEnumerator MoveUI(RectTransform uiElement, Vector2 targetPosition, float speed)
    {
        float duration = speed;
        float timeElapsed = 0;
        Vector2 startPos = uiElement.anchoredPosition;

        while (timeElapsed < duration)
        {
            uiElement.anchoredPosition = Vector2.Lerp(startPos, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        uiElement.anchoredPosition = targetPosition;
    }

    private void StartCooldownFill()
    {
        if (cooldownFillCoroutine != null) StopCoroutine(cooldownFillCoroutine);
        cooldownFillCoroutine = StartCoroutine(CooldownProgress());
    }

    private IEnumerator CooldownProgress()
    {
        float cooldownTime = 5f;
        float elapsed = 0f;
        cooldownFill.fillAmount = 1f;

        while (elapsed < cooldownTime)
        {
            cooldownFill.fillAmount = 1f - (elapsed / cooldownTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cooldownFill.fillAmount = 0f;
        HideCooldownBar();
    }
}
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AlarmBarView : MonoBehaviour
{
    [Header("Configuración de la Barra")]
    private Image healthFillImage;
    [SerializeField] private RectTransform alertRT;

    [Header("Colores de la Barra")]
    [SerializeField] private Color lowBarColor = Color.green;    // Verde cuando está llena
    [SerializeField] private Color midBarColor = Color.yellow;    // Amarillo a la mitad
    [SerializeField] private Color fullBarColor = Color.red;       // Rojo cuando está baja

    public void Initialize()
    {
        healthFillImage = GetComponent<Image>();
        healthFillImage.fillAmount = 0;
        healthFillImage.color = fullBarColor;
    }

    /// <summary>
    /// Actualiza la barra de vida al valor especificado
    /// </summary>
    /// <param name="newHealth">Nuevo valor de salud</param>
    public void UpdateAlarmBar(float newHealth, float maxHealth)
    {
        float fillAmount = newHealth / maxHealth;

        healthFillImage.fillAmount = fillAmount;
        UpdateBarColor(fillAmount);
    }

    public void IncreaseAlarmBar(float newHealth, float maxHealth, SuspicionType type)
    {
        //TODO Bar Message depending of type
        string message = "";
        switch (type)
        {
            case SuspicionType.FoodLack:
                message = "Los Humanos necesitan Comida!";
                break;
            case SuspicionType.WrongZone:
                message = "Parece que no debo estar aquí!";
                break;
            case SuspicionType.WrongInteraction:
                message = "No debí hacer eso!";
                break;
            case SuspicionType.VisibleTransformation:
                message = "Parece que fui descubierto";
                break;
            default:
                message = "Alert!";
                break;
        }

        float fillAmount = newHealth / maxHealth;
        alertRT.DOShakeAnchorPos(0.25f, 50, 20);
        healthFillImage.DOFillAmount(fillAmount, 0.5f);
        //healthFillImage.fillAmount = fillAmount;
        UpdateBarColor(fillAmount);
    }


    /// <summary>
    /// Obtiene el color correspondiente según el porcentaje de vida
    /// </summary>
    private Color GetBarColor(float fillPercentage)
    {
        // Transición entre colores basada en el porcentaje de vida
        if (fillPercentage > 0.5f)
        {
            // De 50% a 100%: Amarillo A Rojo
            float t = (fillPercentage - 0.5f) * 2f; // Normalizar a 0-1
            return Color.Lerp(midBarColor, fullBarColor, t);
        }
        else
        {
            // De 0% a 50%: Verde a Amarillo
            float t = fillPercentage * 2f; // Normalizar a 0-1
            return Color.Lerp(lowBarColor, midBarColor, t);
        }
    }

    public void InAlarmAnimation()
    {
        // TODO Animate Alarm
    }

    public void StopAlarmAnimation()
    {
        // TODO stop Animation Alarm
    }

    /// <summary>
    /// Actualiza el color de la barra según el porcentaje actual (sin animación)
    /// </summary>
    private void UpdateBarColor(float fillPercentage)
    {
        healthFillImage.color = GetBarColor(fillPercentage);
    }
}

using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float fadeDuration = 1f;

    private TextMeshProUGUI textMesh;
    private Color startColor;
    private float elapsed = 0f;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        if(textMesh != null)
        {
            startColor = textMesh.color;

        }
        Destroy(gameObject, fadeDuration);
    }

    void Update()
    {
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);

        if(textMesh != null)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }
    }
}

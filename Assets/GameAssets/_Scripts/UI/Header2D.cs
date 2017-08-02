using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class Header2D : MonoBehaviour {

    [SerializeField]
    private Sprite[] spriteArray;

    [SerializeField]
    private string stringText;

    [SerializeField]
    private float time;

    private void Start()
    {
        ShowText(stringText, time);
    }

    /// <summary>
    /// Muestra durante <paramref name="time"/> segundos el mensaje <paramref name="text"/>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time"></param>
	public void ShowText(string text, float time)
    {
        StartCoroutine(ShowText_Coroutine(text, time));
    }

    private IEnumerator ShowText_Coroutine(string text, float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);

        text = text.ToLower();

        for (int i = 0; i < text.Length; i++)
        {
            GameObject textContainer = new GameObject("letter-" + text[i]);

            Image image = textContainer.AddComponent<Image>();

            textContainer.transform.SetParent(this.transform);

            image.sprite = LetterToSprite(text[i]);

            yield return wait;
        }
    }

    private Sprite LetterToSprite(char letter)
    {
        int index = (int)letter - 97;

        if ((int)letter == 46)
        {
            index = 26;
        }
        else if ((int)letter == 32)
        {
            index = 27;
        }

        Sprite sprite = spriteArray[index];

        return sprite;
    }
}

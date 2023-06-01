using UnityEngine;
using TMPro;

public class InputFieldReader : MonoBehaviour
{
    public TMP_InputField inputField;
    public string parameter;

    private void Start()
    {
        // Attach a listener to the input field's "onEndEdit" event
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void OnInputFieldEndEdit(string inputText)
    {
        // Called when the user finishes editing the input field
        Debug.Log("Input Text: " + inputText);
        parameter = inputText;
        // Do something with the inputText, such as updating a parameter in your game
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionSelector : MonoBehaviour
{
    [SerializeField] private GameObject ConfirmationPopUp; // Referencia al Popup de Confirmación
    [SerializeField] private TextMeshProUGUI confirmationText; // Referencia al texto dentro del Popup de Confirmación

    private void Awake()
    {
        // Asegúrate de que el popup de confirmación esté desactivado al inicio
        ConfirmationPopUp.SetActive(false);
    }

    public void OptionSelected(string option)
    {
        // Actualiza el texto del popup de confirmación con la opción seleccionada
        confirmationText.text = $"¿Estás seguro que deseas elegir la opción: {option}?";
        // Muestra el popup de confirmación
        ConfirmationPopUp.SetActive(true);
    }

    // Este método puede ser llamado por los botones de confirmar y cancelar en tu popup
    public void ConfirmSelection(bool isConfirmed)
    {
        if (isConfirmed)
        {
            // Lógica para manejar la confirmación de la selección aquí
            Debug.Log("Opción confirmada: " + confirmationText.text);
        }
        // Esconde el popup de confirmación después de cualquier elección
        ConfirmationPopUp.SetActive(false);
    }
}

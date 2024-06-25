using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TabManager : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] tabs;
    public Color selectedButtonColor;
    public Color selectedTextColor;
    public Color defaultButtonColor;
    public Color defaultTextColor;
    [SerializeField] private RectTransform content;
    [SerializeField] private Scrollbar scroll;
    private void OnTabSelected(int index)
    {
        // Inactivar todos los botones primero
        ResetTabs();

        // Activar el botón seleccionado
        SetButtonState(buttons[index], true);
        tabs[index].SetActive(true);
        if (index == 2)
        {
            content.sizeDelta= new Vector2(867, 2550);
            scroll.value = 1;
        }
        if (index == 0)
        {
            content.sizeDelta= new Vector2(867, 4848);
            scroll.value = 1;
        }

        if (index == 1)
        {
            if (FinalUIController.Instance != null)
            {
                // Accediendo a una variable pública directamente
                
                // O usando un método público para obtener la variable
                content.sizeDelta= new Vector2(867,FinalUIController.Instance.getContentSize()+600);
            }
            else
            {
                Debug.LogError("FinalUIController instance not found!");
            }
            scroll.value = 1;
        }
    }

    private void ResetTabs()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            SetButtonState(buttons[i], false);
            tabs[i].SetActive(false);
        }
    }
    private void SetButtonState(Button button, bool isSelected)
    {
        
        ColorBlock colors = button.colors;
        TextMeshProUGUI  buttonText = button.GetComponentInChildren<TextMeshProUGUI >();
        
        if (isSelected)
        {
            Debug.Log("essleccionado");
            colors.normalColor = selectedButtonColor;
            colors.highlightedColor = selectedButtonColor;
            colors.disabledColor = selectedButtonColor;
            colors.selectedColor = selectedButtonColor;
            colors.pressedColor = defaultButtonColor;
            
            buttonText.color = selectedTextColor;
        }
        else
        {
            Debug.Log("no es leccionado");
            colors.normalColor = defaultButtonColor;
            colors.highlightedColor = defaultButtonColor;
            colors.disabledColor = defaultButtonColor;
            colors.selectedColor = defaultButtonColor;
            colors.pressedColor = selectedButtonColor;
            
            buttonText.color = defaultTextColor;
        }
        
        button.colors = colors;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Añadir listeners a todos los botones
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Necesario para capturar el índice correctamente en el lambda
            buttons[i].onClick.AddListener(() => OnTabSelected(index));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}

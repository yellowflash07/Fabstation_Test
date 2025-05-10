using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemsManager : MonoBehaviour
{
    [SerializeField] private List<ButtonController> uiButtons;
    [SerializeField] private List<MonoBehaviour> systems;

    void Start()
    {
        foreach (ButtonController button in uiButtons)
        {
            button.Select(false);
            button.SetButtonAction(() =>
            {
                foreach (ButtonController otherButton in uiButtons)
                {
                    if (otherButton != button)
                    {
                        otherButton.Select(false);
                    }
                }
                button.Select(true);
            });
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

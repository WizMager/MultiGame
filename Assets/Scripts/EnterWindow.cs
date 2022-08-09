using UnityEngine;
using UnityEngine.UI;

public class EnterWindow : MonoBehaviour
{
        [SerializeField] private Button signInButton;
        [SerializeField] private Button createAccountButton;
        [SerializeField] private Button signInBack;
        [SerializeField] private Button createAccountBack;
        [SerializeField] private Canvas optionsCanvas;
        [SerializeField] private Canvas signInCanvas;
        [SerializeField] private Canvas createAccountCanvas;

        private void Start()
        {
                signInButton.onClick.AddListener(OpenSignInWindow);
                createAccountButton.onClick.AddListener(OpenCreateAccountWindow);
                signInBack.onClick.AddListener(BackToOptionsWindows);
                createAccountBack.onClick.AddListener(BackToOptionsWindows);
        }

        private void BackToOptionsWindows()
        {
                signInCanvas.enabled = false;
                optionsCanvas.enabled = true;
                createAccountCanvas.enabled = false;
        }

        private void OpenCreateAccountWindow()
        {
                signInCanvas.enabled = false;
                optionsCanvas.enabled = false;
                createAccountCanvas.enabled = true;
        }

        private void OpenSignInWindow()
        {
                signInCanvas.enabled = true;
                optionsCanvas.enabled = false;
                createAccountCanvas.enabled = false;  
        }
}
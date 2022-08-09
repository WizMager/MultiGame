using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountWindow : AccountDataWindowBase
{
        [SerializeField] private InputField emailField;
        [SerializeField] private Button createAccountButton;

        private string _email;

        protected override void SubscribeToElementUI()
        {
                base.SubscribeToElementUI();
                emailField.onValueChanged.AddListener(ChangeEmail);
                createAccountButton.onClick.AddListener(CreateAccount);
        }

        private void CreateAccount()
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
            {
                Username = _userName,
                Email = _email,
                Password = _password
            },
                result =>
                {
                    Debug.Log($"Success! {_userName}");
                    EnterInGameScene();
                }, error => { Debug.Log($"{error.ErrorMessage}");});
        }

        private void ChangeEmail(string email)
        {
            _email = email;
        }
}
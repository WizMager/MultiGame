using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class SignInWindow : AccountDataWindowBase
{
      [SerializeField] private Button signInButton;

      protected override void SubscribeToElementUI()
      {
            base.SubscribeToElementUI();
            
            signInButton.onClick.AddListener(SignIn);
      }

      private void SignIn()
      {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
            {
                  Username = _userName,
                  Password = _password
            },
                  result =>
                  {
                        Debug.Log($"Success! {_userName}");
                        EnterInGameScene();
                  }, 
                  error => { Debug.Log($"{error.ErrorMessage}");});
      }
}
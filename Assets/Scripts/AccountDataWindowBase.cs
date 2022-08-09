using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class AccountDataWindowBase : MonoBehaviour
{
       [SerializeField] private InputField userNameField;
       [SerializeField] private InputField passwordField;

       protected string _userName;
       protected string _password;

       private void Start()
       {
              SubscribeToElementUI();
       }

       protected virtual void SubscribeToElementUI()
       {
             userNameField.onValueChanged.AddListener(UserNameChanged);
             passwordField.onValueChanged.AddListener(PasswordChanged);
       }

       private void PasswordChanged(string password)
       {
           _password = password;
       }

       private void UserNameChanged(string username)
       {
           _userName = username;
       }

       protected void EnterInGameScene()
       {
           SceneManager.LoadScene(1);
       }
}
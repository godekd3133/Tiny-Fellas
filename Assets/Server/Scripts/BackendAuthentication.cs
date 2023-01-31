using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

[System.Serializable]
public struct WinLogin
{
    public RectTransform rtrnParent;
    public InputField inputID;
    public InputField inputPW;
    public Toggle togAutoLogin;
}

[System.Serializable]
public struct WinConfirm
{
    public RectTransform rtrnParent;
    public Text txtContent;
    public Button btnConfirm;
    public Text txtConfirm;
}

[System.Serializable]
public struct WinSignLogout
{
    public RectTransform rtrnParent;
    public InputField inputReason;
    public Button btnSignout;
    public Button btnLogout;
}

public class BackendAuthentication : MonoBehaviour
{
    public WinLogin winLogin;
    public WinConfirm winConfirm;
    public WinSignLogout winSignLogout;

    private void Start()
    {
        if (PlayerPrefs.GetInt("AutoLogin?") == 1 ? true : false)
        {
            if (!AutoLogIn())
            {
                print("회원가입이나 로그인이 필요합니다");
                ShowWinConfirm(winConfirm, "auto login fail so, you need to signup or login", () => { });
                winLogin.rtrnParent.gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
    }

    public void ShowWinConfirm(WinConfirm winConfirm, string txtContent, UnityAction confirmAction, string txtConfirm = "Close")
    {
        winConfirm.rtrnParent.gameObject.SetActive(true);
        winConfirm.txtContent.text = txtContent;
        winConfirm.txtConfirm.text = txtConfirm;

        winConfirm.btnConfirm.onClick.RemoveAllListeners();
        winConfirm.btnConfirm.onClick.AddListener(() => { winConfirm.rtrnParent.gameObject.SetActive(false); });
        winConfirm.btnConfirm.onClick.AddListener(confirmAction);
    }

    public void LogOut()
    {
        var BRO = Backend.BMember.Logout();
        winSignLogout.rtrnParent.gameObject.SetActive(!BRO.IsSuccess());
        if (BRO.IsSuccess())
        {
            print("로그아웃 성공");
            ShowWinConfirm(winConfirm, "log out successful", () => { winLogin.rtrnParent.gameObject.SetActive(true); });
        }
        else
        {
            print("로그아웃 실패");
            ShowWinConfirm(winConfirm, "fail log out", () => { });
        }
    }

    public void SignOut()
    {
        var BRO = Backend.BMember.SignOut(winSignLogout.inputReason.text);
        winSignLogout.rtrnParent.gameObject.SetActive(!BRO.IsSuccess());
        if (BRO.IsSuccess())
        {
            print("회원탈퇴 성공");
            ShowWinConfirm(winConfirm, "sign out successful", () => { winLogin.rtrnParent.gameObject.SetActive(true); });
        }
        else
        {
            print("회원탈퇴 실패");
            ShowWinConfirm(winConfirm, "fail sign out", () => { });
        }
    }

    public void SignUp()
    {
        SignUp(winLogin.inputID.text, winLogin.inputPW.text);
    }

    public void SignUp(string id, string pw)
    {
        var BRO = Backend.BMember.CustomSignUp(id, pw, "Test1");

        if (BRO.IsSuccess())
        {
            print("회원 가입 완료");
            ShowWinConfirm(winConfirm, "signup successful", () => { });
        }
        else
        {
            print("회원 가입 실패");
            ShowWinConfirm(winConfirm, "fail signup", () => { });
        }
    }

    public void LogIn()
    {
        LogIn(winLogin.inputID.text, winLogin.inputPW.text);
    }

    public void LogIn(string id, string pw)
    {
        var BRO = Backend.BMember.CustomLogin(id, pw);

        PlayerPrefs.SetInt("AutoLogin?", winLogin.togAutoLogin.isOn ? 1 : 0);

        if (BRO.IsSuccess())
        {
            print("로그인 완료");
            //ShowWinConfirm(winConfirm, "login successful", () => { winSignLogout.rtrnParent.gameObject.SetActive(true); });
            ShowWinConfirm(winConfirm, "login successful", () =>
            {
                SceneLoader.LoadScene("TitleScene");

                //UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
            });
            winLogin.rtrnParent.gameObject.SetActive(false);
            BackendManager.Instance.GetChart();
            BackendPlayerData.CreateOrLoadData();
        }
        else
        {
            print("로그인 실패");
            ShowWinConfirm(winConfirm, "fail login", () => { });
        }
    }

    public bool AutoLogIn()
    {
        var BRO = Backend.BMember.LoginWithTheBackendToken();

        if (BRO.IsSuccess())
        {
            print("자동 로그인 완료");
            //ShowWinConfirm(winConfirm, "auto login successful", () => { winSignLogout.rtrnParent.gameObject.SetActive(true); });
            ShowWinConfirm(winConfirm, "auto login successful", () =>
            {
                SceneLoader.LoadScene("TitleScene");
            });
            winLogin.rtrnParent.gameObject.SetActive(false);
            BackendManager.Instance.GetChart();
            BackendPlayerData.CreateOrLoadData();
        }
        else
        {
            print("자동 로그인 실패");
            ShowWinConfirm(winConfirm, "fail auto login", () => { });
        }

        return BRO.IsSuccess();
    }
}

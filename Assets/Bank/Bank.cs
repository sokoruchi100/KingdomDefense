using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] private int startingBalance = 150;
    [SerializeField] private int currentBalance;
    [SerializeField] private TextMeshProUGUI displayBalance;
    public int CurrentBalance { get { return currentBalance; } }

    private void Awake() {
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    public void Deposit(int amount) {
        currentBalance += Mathf.Abs(amount);
        UpdateDisplay();
    }

    public void Withdraw(int amount) {
        currentBalance -= Mathf.Abs(amount);
        
        if (currentBalance < 0) {
            ReloadScene();
        }

        UpdateDisplay();
    }

    private void UpdateDisplay() {
        displayBalance.text = $"Gold: {currentBalance}";
    }

    private void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

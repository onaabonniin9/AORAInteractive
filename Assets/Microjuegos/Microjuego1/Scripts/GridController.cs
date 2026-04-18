using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GridController - Gestiona el grid 3x3, las posiciones de monedas
/// y la lógica de selección del jugador
/// </summary>
public class GridController : MonoBehaviour
{
    [Header("Grid Setup")]
    [SerializeField] private CoinCell[] cells; // Arrastra los 9 objetos CoinCell aquí en Inspector

    private List<int> coinPositions = new List<int>();
    private int selectedCount = 0;
    private bool inputEnabled = false;

    /// <summary>
    /// Inicia una ronda nueva: muestra monedas, luego las oculta
    /// </summary>
    public void StartRound(int coinsCount, float memorizeTime)
    {
        ResetAllCells();
        selectedCount = 0;
        inputEnabled = false;

        // Elegir posiciones aleatorias para las monedas
        coinPositions = GetRandomPositions(coinsCount);

        // Mostrar monedas
        foreach (int i in coinPositions)
            cells[i].ShowCoin();

        G1_GameManager.Instance.GetComponent<UIManager>()?.SetPhaseText("MEMORIZA");

        // Ocultar tras el tiempo de memorización
        StartCoroutine(HideCoinsAfterDelay(memorizeTime));
    }

    private IEnumerator HideCoinsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (CoinCell cell in cells)
            cell.HideCoin();

        G1_GameManager.Instance.GetComponent<UIManager>()?.SetPhaseText("SELECCIONA");
        inputEnabled = true;
        G1_GameManager.Instance.CurrentState_Set(G1_GameManager.GameState.Selecting);
    }

    public void OnCellTapped(int cellIndex)
    {
        if (!inputEnabled) return;
        if (cells[cellIndex].IsSelected) return;

        Debug.Log("selectedCount: " + selectedCount + " coinPositions.Count: " + coinPositions.Count);


        cells[cellIndex].SetSelected(true);
        selectedCount++;

        bool isCorrect = coinPositions.Contains(cellIndex);

        if (isCorrect)
        {
            cells[cellIndex].ShowCorrect();
            G1_GameManager.Instance.OnCoinCorrect();
        }
        else
        {
            cells[cellIndex].ShowWrong();
            G1_GameManager.Instance.OnCoinWrong();
        }

        // Si ya se han seleccionado todas las posiciones de monedas
        if (selectedCount >= coinPositions.Count)
        {
            inputEnabled = false;
            G1_GameManager.Instance.OnRoundComplete();
        }
    }

    public void DisableInput()
    {
        inputEnabled = false;
    }

    private void ResetAllCells()
    {
        foreach (CoinCell cell in cells)
            cell.ResetCell();
    }

    private List<int> GetRandomPositions(int count)
    {
        List<int> allIndices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        List<int> result = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int randomIdx = Random.Range(0, allIndices.Count);
            result.Add(allIndices[randomIdx]);
            allIndices.RemoveAt(randomIdx);
        }

        return result;
    }
}

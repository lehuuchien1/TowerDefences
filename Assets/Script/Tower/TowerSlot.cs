using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TowerSlot : MonoBehaviour
{
    public GameObject[] towerPrefabs; // Mảng chứa các Prefab của các tháp
    public GameObject towerSelectionUI; // UI hiển thị danh sách các tháp
    public GameObject insufficientFundsTextPrefab; // Prefab cho thông báo không đủ vàng

    private bool isUIActive = false; // Cờ để theo dõi trạng thái hiển thị của UI

    void Start()
    {
        // Ẩn towerSelectionUI khi trò chơi bắt đầu
        towerSelectionUI.SetActive(false);
    }

    void OnMouseDown()
    {
        // Hiển thị towerSelectionUI khi người chơi click vào slot của tháp
        towerSelectionUI.SetActive(true);
        isUIActive = true;

        // Đăng ký lắng nghe các click bên ngoài TowerSelectionUI
        StartCoroutine(ClickOutsideUI());
    }

    IEnumerator ClickOutsideUI()
    {
        // Chờ đợi cho đến frame tiếp theo
        yield return null;

        // Kiểm tra các click bên ngoài towerSelectionUI
        while (isUIActive)
        {
            if (Input.GetMouseButtonDown(0)) // Giả sử là click chuột trái
            {
                // Kiểm tra xem click có trúng vào towerSelectionUI hay không
                if (!IsPointerOverUIObject())
                {
                    // Click bên ngoài towerSelectionUI, vì vậy ẩn nó đi
                    towerSelectionUI.SetActive(false);
                    isUIActive = false;
                }
            }
            yield return null;
        }
    }

    bool IsPointerOverUIObject()
    {
        // Kiểm tra xem con trỏ chuột có đang trỏ vào một đối tượng UI hay không
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void BuildTower(int towerIndex)
    {
        // Kiểm tra xem towerIndex có hợp lệ không
        if (towerIndex >= 0 && towerIndex < towerPrefabs.Length)
        {
            int towerCost = towerPrefabs[towerIndex].GetComponent<Tower>().cost;
            if (GameManager.Instance.playerGold >= towerCost)
            {
                // Xây dựng tháp tại vị trí của slot tháp
                Instantiate(towerPrefabs[towerIndex], transform.position, Quaternion.identity);
                // Ẩn towerSelectionUI sau khi xây dựng
                towerSelectionUI.SetActive(false);
                isUIActive = false;
                // Vô hiệu hóa slot tháp sau khi xây dựng
                gameObject.SetActive(false);
                // Trừ vàng từ GameManager
                GameManager.Instance.playerGold -= towerCost;
                GameManager.Instance.UpdateUI();
            }
            else
            {
                ShowInsufficientFundsText();
            }
        }
    }

    private void ShowInsufficientFundsText()
    {
        if (insufficientFundsTextPrefab != null)
        {
            // Tạo thông báo không đủ vàng
            GameObject textObject = Instantiate(insufficientFundsTextPrefab, transform.position, Quaternion.identity);
            // Đặt textObject làm con của một Canvas nếu cần thiết
            textObject.transform.SetParent(GameManager.Instance.transform, false);
            StartCoroutine(DestroyTextAfterDelay(textObject, 0.5f));
        }
    }

    private IEnumerator DestroyTextAfterDelay(GameObject textObject, float delay)
    {
        // Chờ một khoảng thời gian và sau đó xóa textObject
        yield return new WaitForSeconds(delay);
        Destroy(textObject);
    }
}

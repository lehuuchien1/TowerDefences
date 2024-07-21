using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class TowerSlot : MonoBehaviour
{
    public GameObject[] towerPrefabs; // Mảng chứa các Prefab của các tháp
    public GameObject towerSelectionUI; // UI hiển thị danh sách các tháp

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
            // Xây dựng tháp tại vị trí của slot tháp
            Instantiate(towerPrefabs[towerIndex], transform.position, Quaternion.identity);
            // Ẩn towerSelectionUI sau khi xây dựng
            towerSelectionUI.SetActive(false);
            isUIActive = false;
            // Vô hiệu hóa slot tháp sau khi xây dựng
            gameObject.SetActive(false);
        }
    }
}

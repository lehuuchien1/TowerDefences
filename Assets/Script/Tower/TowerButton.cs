using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    public TowerSlot towerSlot; // Tham chiếu đến script TowerSlot
    public int towerIndex; // Chỉ số của loại tháp tương ứng

    void Start()
    {
        // Gán sự kiện OnClick cho Button
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Gọi phương thức BuildTower từ script TowerSlot với tham số là towerIndex
        towerSlot.BuildTower(towerIndex);
    }
}
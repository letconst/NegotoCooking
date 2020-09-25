using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlideShow : MonoBehaviour
{
    [SerializeField, Tooltip("スライドショーとして表示させる画像")]
    private List<Sprite> showImage;

    // スライドショーの画像を表示する領域
    private Image _placeholder;

    // Start is called before the first frame update
    private void Start()
    {
        _placeholder = GameObject.FindGameObjectWithTag("SlideShowImagePlaceholder").GetComponent<Image>();

        if (_placeholder == null) return;

        // 一番目の画像を表示
        _placeholder.sprite = showImage.First();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetButtonDown("Submit")) return;

        // 現在表示してる画像のインデックス
        var curImageIndex = showImage.IndexOf(_placeholder.sprite);

        // 最後の画像に到達したらゲームシーンに遷移
        if (curImageIndex == showImage.Count - 1)
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameScenes);

            return;
        }

        // 次の画像を表示
        _placeholder.sprite = showImage.ElementAt(curImageIndex + 1);
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        //aspectRatioを初期化
        _placeholder.GetComponent<AspectRatioFitter>().aspectRatio = 1.766666666f;
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
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameScenes, true);

            return;
        }

        //もし次の画像が最後だった場合
        if (curImageIndex == showImage.Count - 2)
        {
            //aspectRatioをMenu画像に合わせる
            _placeholder.GetComponent<AspectRatioFitter>().aspectRatio = 0.73f;
        }

        // 次の画像を表示
        _placeholder.sprite = showImage.ElementAt(curImageIndex + 1);
    }
}

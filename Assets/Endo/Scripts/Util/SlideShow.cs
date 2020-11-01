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

    // シーン読み込みか否か
    private bool _isSceneLoading;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.cutOperationBool = true;
        GameManager.Instance.bakeOperationBool = true;
        GameManager.Instance.boilOperationBool = true;
        GameManager.Instance.menuBool = true;
        _placeholder = GameObject.FindGameObjectWithTag("SlideShowImagePlaceholder").GetComponent<Image>();

        if (_placeholder == null) return;

        // 一番目の画像を表示
        _placeholder.sprite = showImage.First();
    }

    // Update is called once per frame
    private void Update()
    {
        // A押下時およびシーン読み込み中でなければ動作
        if (!Input.GetButtonDown("Submit") || _isSceneLoading) return;

        // 現在表示してる画像のインデックス
        var curImageIndex = showImage.IndexOf(_placeholder.sprite);

        // 最後の画像に到達したらゲームシーンに遷移
        if (curImageIndex == showImage.Count - 1)
        {
            _isSceneLoading = true;
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameScenes, true);

            return;
        }

        // 次の画像を表示
        _placeholder.sprite = showImage.ElementAt(curImageIndex + 1);
    }
}

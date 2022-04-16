using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomStories : MonoBehaviour
{
    [SerializeField]
    private Transform _sListParent;

    [SerializeField]
    private CustomListItem _listItemPrefab;

    [SerializeField]
    private float _defaultSpaceS;

    private List<CustomListItem> _sItems = new List<CustomListItem>();

    // Start is called before the first frame update
    void Start()
    {
        AddItemS();
        AddItemS();
        AddItemS();
    }

    public void AddItemS()
    {
        var item = Instantiate(_listItemPrefab, _sListParent);
        _sItems.Add(item);

        item.No.text = _sItems.Count.ToString();
    }


    public void RemoveLastS()
    {
        if (_sItems.Count == 2)
            return;
        var item = _sItems.Last();
        _sItems.Remove(item);
        Destroy(item.gameObject);
    }

    public void BtnOkClick()
    {
        int s = _sItems.Count;
        float[] sSpaces = new float[s];
        for (int i = 0; i < s; i++)
        {
            if (float.TryParse(_sItems[i].Value.text, out float space))
                sSpaces[i] = space;
            else
            {
                MainManager.Instance.MainWindow.ShowError("Story No. " + (i + 1) + " Should have numeric value!");
                return;
            }
        }

        MainManager.Instance.CreateGridWindow.SetCustomS(s, sSpaces.ToList());
        BtnCloseClick();
    }

    public void BtnCloseClick()
    {
        MainManager.Instance.CreateGridWindow.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomXY : MonoBehaviour
{
    [SerializeField]
    private Transform _xListParent;

    [SerializeField]
    private Transform _yListParent;

    [SerializeField]
    private CustomListItem _listItemPrefab;

    private List<CustomListItem> _xItems = new List<CustomListItem>();
    private List<CustomListItem> _yItems = new List<CustomListItem>();

    // Start is called before the first frame update
    void Start()
    {
        AddItemX();
        AddItemX();
        AddItemX();

        AddItemY();
        AddItemY();
        AddItemY();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemX()
    {
        var item = Instantiate(_listItemPrefab, _xListParent);
        _xItems.Add(item);

        item.No.text = _xItems.Count.ToString();
    }

    public void AddItemY()
    {
        var item = Instantiate(_listItemPrefab, _yListParent);
        _yItems.Add(item);

        item.No.text = _yItems.Count.ToString();
    }

    public void RemoveLastX()
    {
        if (_xItems.Count == 2)
            return;
        var item = _xItems.Last();
        _xItems.Remove(item);
        Destroy(item.gameObject);
    }

    public void RemoveLastY()
    {
        if (_yItems.Count == 2)
            return;
        var item = _yItems.Last();
        _yItems.Remove(item);
        Destroy(item.gameObject);
    }

    public void BtnOkClick()
    {
        int x = _xItems.Count;
        float[] xSpaces = new float[x];
        for (int i = 0; i < x; i++)
        {
            if (float.TryParse(_xItems[i].Value.text, out float space))
            {
                xSpaces[i] = space / 100.0f;
            }
            else
            {
                MainManager.Instance.MainWindow.ShowError("X No. " + (i + 1) + " should have numeric value!");
                return;
            }
        }

        int y = _yItems.Count;
        float[] ySpaces = new float[y];
        for (int i = 0; i < y; i++)
        {
            if (float.TryParse(_yItems[i].Value.text, out float space))
            {
                ySpaces[i] = space / 100.0f;
            }
            else
            {
                MainManager.Instance.MainWindow.ShowError("X No. " + (i + 1) + " should have numeric value!");
                return;
            }
        }

        MainManager.Instance.CreateGridWindow.SetCustomXY(x, y, xSpaces, ySpaces);
        BtnCloseClick();
    }

    public void BtnCloseClick()
    {
        MainManager.Instance.CreateGridWindow.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}

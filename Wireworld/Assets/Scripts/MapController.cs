using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;

public class MapController : MonoBehaviour
{
    public Tilemap map;
    public TileBase conductor, head, tail;
    public State currentlyDrawingState = State.Empty;

    public enum State
    {
        Empty,
        Conductor,
        Head,
        Tail
    }

    Dictionary<Vector3Int, State> field = new Dictionary<Vector3Int, State>();
    Dictionary<State, TileBase> tiles = new Dictionary<State, TileBase>();

    public bool anim = false; 

    void Start()
    {
        tiles.Add(State.Conductor, conductor);
        tiles.Add(State.Head, head);
        tiles.Add(State.Tail, tail);
        tiles.Add(State.Empty, null);

        for (int i = -5; i < 5; i++)
        {
            field.Add(new Vector3Int(i, 0, 0), State.Conductor);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) anim = !anim;

        if(anim)UpdateFieldState();
        SelectDrawingMode();
        Draw();

    }
    void Draw()
    {
        map.ClearAllTiles();

        //Draw cells
        foreach (var item in field)
            map.SetTile(new Vector3Int(item.Key.x, item.Key.y, 0), tiles.First(x => x.Key == item.Value).Value);

        //Hologram
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var holoTile = tiles.First(x => x.Key == currentlyDrawingState).Value;
        if (holoTile != null)
            map.SetTile(map.WorldToCell(pos), holoTile);


        if (Input.GetMouseButton(0))
        {
            field.Remove(map.WorldToCell(pos));
            field.Add(map.WorldToCell(pos), currentlyDrawingState);
        }
        if (Input.GetMouseButton(1))
        {
            field.Remove(map.WorldToCell(pos));
        }

    }
    void SelectDrawingMode()
    {
        if (Input.GetKey(KeyCode.Alpha1)) currentlyDrawingState = State.Conductor;
        if (Input.GetKey(KeyCode.Alpha2)) currentlyDrawingState = State.Head;
        if (Input.GetKey(KeyCode.Alpha3)) currentlyDrawingState = State.Tail;
        if (Input.GetKey(KeyCode.Alpha0)) currentlyDrawingState = State.Empty;
    }
    void UpdateFieldState()
    {
        Dictionary<Vector3Int, State> newField = new Dictionary<Vector3Int, State>();

        foreach (var item in field)
        {
            if (item.Value == State.Head) newField.Add(item.Key, State.Tail);
            else if (item.Value == State.Tail) newField.Add(item.Key, State.Conductor);
            else if (item.Value == State.Conductor)
            {
                int n = 0;

                if (field.Contains(new KeyValuePair<Vector3Int, State>(new Vector3Int(item.Key.x + 1, item.Key.y, 0), State.Head))) n++;
                if (field.Contains(new KeyValuePair<Vector3Int, State>(new Vector3Int(item.Key.x - 1, item.Key.y, 0), State.Head))) n++;

                if (field.Contains(new KeyValuePair<Vector3Int, State>(new Vector3Int(item.Key.x + 1, item.Key.y + 1, 0), State.Head))) n++;
                if (field.Contains(new KeyValuePair<Vector3Int, State>(new Vector3Int(item.Key.x, item.Key.y + 1, 0), State.Head))) n++;
                if (field.Contains(new KeyValuePair<Vector3Int, State>(new Vector3Int(item.Key.x - 1, item.Key.y + 1, 0), State.Head))) n++;

                if (field.Contains(new KeyValuePair<Vector3Int, State>(new Vector3Int(item.Key.x + 1, item.Key.y - 1, 0), State.Head))) n++;
                if (field.Contains(new KeyValuePair<Vector3Int, State>(new Vector3Int(item.Key.x, item.Key.y - 1, 0), State.Head))) n++;
                if (field.Contains(new KeyValuePair<Vector3Int, State>(new Vector3Int(item.Key.x - 1, item.Key.y - 1, 0), State.Head))) n++;

                if (n == 1 || n == 2)
                    newField.Add(item.Key, State.Head);
                else
                    newField.Add(item.Key, State.Conductor);
            }
        }
        field = newField;
        Thread.Sleep(30);

    }
}


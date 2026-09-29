using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SlidingPuzzle : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button finishBtn;
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private Texture2D puzzleImage;
    [SerializeField] private RectTransform board;
    [SerializeField, Range(2, 8)] int gridSize = 4;
    [SerializeField] private float spacing = 2f;

    [Header("Behaviour")]
    [SerializeField] private float slideDuration = 0.12f;
    [SerializeField] private float revealDuration = 0.5f;
    [SerializeField] private int shuffleMoves = 200;
    [SerializeField] private bool startOnAwake = true;

    [Header("Events")]
    public UnityEvent<int> onMoveMade;   // passes the total move count
    public UnityAction OnPuzzleFinish;
    public UnityAction OnPuzzleClose;

    private int[] cells;             // cells[position] = tile id (row-major, row 0 = top)
    private RectTransform[] tiles;   // tiles[id]
    private int emptyId;             // id of the missing tile (bottom-right piece)
    private float tileSize;
    private int moves;
    private bool isSliding;
    private bool isSolved;

    public int Moves => moves;
    public bool IsSolved => isSolved;

    private void Awake()
    {
        closeBtn.onClick.AddListener(() =>
        {
            CloseAndDestroyPuzzle();
            OnPuzzleClose?.Invoke();
        });

        finishBtn.onClick.AddListener(() =>
        {
            CloseAndDestroyPuzzle();
            OnPuzzleFinish?.Invoke();
        });

        finishBtn.gameObject.SetActive(false);
    }

    void Start()
    {
        if (startOnAwake) 
            NewGame();
    }

    [ContextMenu("New Game")]
    public void NewGame()
    {
        StopAllCoroutines();
        isSliding = false;
        isSolved = false;
        moves = 0;
        onMoveMade?.Invoke(moves);

        BuildTiles();
        Shuffle();
        PlaceAllTiles();
    }

    public void InitSlidingPuzzle(UnityAction onClose, UnityAction onFinish)
    {
        OnPuzzleFinish = onFinish;
        OnPuzzleClose = onClose;
    }

    // ---------- Build ----------
    private void BuildTiles()
    {
        int count = gridSize * gridSize;
        emptyId = count - 1;
        cells = new int[count];
        tiles = new RectTransform[count];

        float boardSize = Mathf.Min(board.rect.width, board.rect.height);
        tileSize = (boardSize - spacing * (gridSize - 1)) / gridSize;

        for (int id = 0; id < count; id++)
        {
            cells[id] = id;

            var go = Instantiate(btnPrefab);
            var rt = (RectTransform)go.transform;
            rt.SetParent(board, false);

            // Anchor/pivot top-left so positions are easy to compute
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 1f);
            rt.sizeDelta = new Vector2(tileSize, tileSize);

            // Slice the texture with UVs (UV origin is bottom-left)
            int col = id % gridSize;
            int row = id / gridSize;
            var raw = go.GetComponent<RawImage>();
            raw.texture = puzzleImage;
            raw.uvRect = new Rect(
                col / (float)gridSize,
                (gridSize - 1 - row) / (float)gridSize,
                1f / gridSize,
                1f / gridSize);

            var btn = go.GetComponent<Button>();
            btn.transition = Selectable.Transition.None;
            int captured = id;
            btn.onClick.AddListener(() => OnTileClicked(captured));

            // The missing piece stays hidden until the puzzle is solved
            if (id == emptyId)
            {
                raw.color = new Color(1f, 1f, 1f, 0f);
                raw.raycastTarget = false;
            }

            tiles[id] = rt;
        }
    }

    private Vector2 CellToPosition(int cellIndex)
    {
        int x = cellIndex % gridSize;
        int y = cellIndex / gridSize;
        return new Vector2(x * (tileSize + spacing), -y * (tileSize + spacing));
    }

    private void PlaceAllTiles()
    {
        for (int pos = 0; pos < cells.Length; pos++)
            tiles[cells[pos]].anchoredPosition = CellToPosition(pos);
    }

    // ---------- Shuffle ----------

    // Shuffling with random *valid moves* guarantees the puzzle is solvable.
    private void Shuffle()
    {
        int emptyPos = Array.IndexOf(cells, emptyId);
        int previousPos = -1;

        for (int i = 0; i < shuffleMoves; i++)
        {
            int[] neighbors = GetNeighbors(emptyPos);

            int next;
            do { next = neighbors[UnityEngine.Random.Range(0, neighbors.Length)]; }
            while (next == previousPos && neighbors.Length > 1); // avoid undoing last move

            Swap(emptyPos, next);
            previousPos = emptyPos;
            emptyPos = next;
        }

        // Extremely unlikely, but never start already solved
        if (CheckSolved()) Shuffle();
    }

    private int[] GetNeighbors(int pos)
    {
        int x = pos % gridSize;
        int y = pos / gridSize;
        var list = new System.Collections.Generic.List<int>(4);
        if (x > 0) list.Add(pos - 1);
        if (x < gridSize - 1) list.Add(pos + 1);
        if (y > 0) list.Add(pos - gridSize);
        if (y < gridSize - 1) list.Add(pos + gridSize);
        return list.ToArray();
    }

    private void Swap(int a, int b)
    {
        (cells[a], cells[b]) = (cells[b], cells[a]);
    }

    // ---------- Input / Moves ----------
    private void OnTileClicked(int tileId)
    {
        if (isSliding || isSolved) return;

        int tilePos = Array.IndexOf(cells, tileId);
        int emptyPos = Array.IndexOf(cells, emptyId);

        if (Array.IndexOf(GetNeighbors(emptyPos), tilePos) < 0) return; // not adjacent

        SlideTile(tileId, tilePos, emptyPos);
    }

    private void SlideTile(int tileId, int fromPos, int toPos)
    {
        isSliding = true;

        RectTransform rt = tiles[tileId];
        Vector2 start = rt.anchoredPosition;
        Vector2 end = CellToPosition(toPos);

        rt.DOAnchorPos(end, slideDuration).OnComplete(()=> 
        {
            rt.anchoredPosition = end;

            Swap(fromPos, toPos);
            // The empty piece moves logically (it's invisible, so just snap it)
            tiles[emptyId].anchoredPosition = CellToPosition(fromPos);

            moves++;
            onMoveMade?.Invoke(moves);
            isSliding = false;

            if (CheckSolved())
                SolveSequence();
        });
    }

    private bool CheckSolved()
    {
        for (int i = 0; i < cells.Length; i++)
            if (cells[i] != i) return false;
        return true;
    }

    private void CloseAndDestroyPuzzle()
    {
        Destroy(gameObject);
    }

    // ---------- Solved ----------
    private void SolveSequence()
    {
        isSolved = true;

        // Fade in the missing piece to complete the picture
        var raw = tiles[emptyId].GetComponent<RawImage>();
        raw.DOFade(1, revealDuration).OnComplete(() =>
        {
            raw.color = Color.white;
            finishBtn.gameObject.SetActive(true);
            //onSolved?.Invoke();
        });
    }
}
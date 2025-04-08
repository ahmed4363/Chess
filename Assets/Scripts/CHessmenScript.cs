using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHessmenScript : MonoBehaviour
{
    //References
    public GameObject controller;
    public GameObject movePlate;
    public GameObject whitePawnPromo;
    public GameObject blackPawnPromo;
    //Positions
    public int xBoard = -1;
    public int yBoard = -1;
    //keep Player turns
    //who is who
    private string player;
    //Keep Track of Sprites
    //Black
    public Sprite bK;
    public Sprite bQ;
    public Sprite bB;
    public Sprite bN;
    public Sprite bR;
    public Sprite bP;
    //White
    public Sprite wK;
    public Sprite wQ;
    public Sprite wB;
    public Sprite wN;
    public Sprite wR;
    public Sprite wP;

    //Unique Properties
    public bool EnPass;
    public bool CanCastle = false;
    public bool Castled = false;
    private bool hasrun;

    //Pawn
    public int lastPlayedEnPass;

    public void Update()
    {
        if (!hasrun)
        { 
            if (yBoard == 0 && name == "bP")
            {
                Debug.Log("Promoted as Black");
                GameObject promobar = Instantiate(blackPawnPromo, new Vector3(this.transform.position.x, this.transform.position.y, -5), Quaternion.identity);
                var promo = promobar.GetComponentsInChildren<PawnPromotion>();
                for (var i = 0; i < promo.Length; i++)
                {
                    promo[i].SetReference(this.transform.gameObject);
                }
                hasrun = true;
            }
            if (yBoard == 7 && name == "wP")
            {
                Debug.Log("Promoted as White");
                GameObject promobar = Instantiate(whitePawnPromo, new Vector3(this.transform.position.x, this.transform.position.y, -5), Quaternion.identity);
                var promo = promobar.GetComponentsInChildren<PawnPromotion>();
                for (var i = 0; i < promo.Length; i++)
                {
                    promo[i].SetReference(this.transform.gameObject);
                }
                hasrun = true;
            }
        }
    }
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Moves the Piece
        SetCords();
        switch (this.name)
        {
            //Black
            case "bK": this.GetComponent<SpriteRenderer>().sprite = bK; player = "black"; CanCastle = true; break;
            case "bQ": this.GetComponent<SpriteRenderer>().sprite = bQ; player = "black"; break;
            case "bB": this.GetComponent<SpriteRenderer>().sprite = bB; player = "black"; break;
            case "bN": this.GetComponent<SpriteRenderer>().sprite = bN; player = "black"; break;
            case "bR": this.GetComponent<SpriteRenderer>().sprite = bR; player = "black"; CanCastle = true;  break;
            case "bP": this.GetComponent<SpriteRenderer>().sprite = bP; player = "black"; break;
            //White
            case "wK": this.GetComponent<SpriteRenderer>().sprite = wK; player = "white"; CanCastle = true; break;
            case "wQ": this.GetComponent<SpriteRenderer>().sprite = wQ; player = "white"; break;
            case "wB": this.GetComponent<SpriteRenderer>().sprite = wB; player = "white"; break;
            case "wN": this.GetComponent<SpriteRenderer>().sprite = wN; player = "white"; break;
            case "wR": this.GetComponent<SpriteRenderer>().sprite = wR; player = "white"; CanCastle = true; break;
            case "wP": this.GetComponent<SpriteRenderer>().sprite = wP; player = "white"; break;
        }
    }
    public void SetCords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1);
    }
    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int x)
    {
        yBoard = x;
    }
    public void OnMouseUp()
    {
        if (!controller.gameObject.GetComponent<GameLogic>().gameOver && controller.gameObject.GetComponent<GameLogic>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
    }
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("movePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }
    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "bQ":
            case "wQ":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                break;
            case "bN":
            case "wN":
                LmovePlate();
                break;
            case "bB":
            case "wB":
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                break;
            case "bK":
            case "wK":
                SurroundMovePlate();
                break;
            case "bR":
            case "wR":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "bP":
                BlackPawnMovePlate(xBoard, yBoard - 1);
                break;
            case "wP":
                WhitePawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }
    public void LineMovePlate(int X, int Y)
    {
        GameLogic GL = controller.GetComponent<GameLogic>();

        int x = xBoard + X;
        int y = yBoard + Y;
        while (GL.positionOnBoard(x, y) && GL.getPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += X;
            y += Y;
        }
        if (GL.positionOnBoard(x, y) && GL.getPosition(x, y).GetComponent<CHessmenScript>().player != player)
        {
            movePlateAttackSpawn(x, y);
        }
    }

    public void LmovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard - 1);
        if (CanCastle)
        {
            GameLogic GL = controller.GetComponent<GameLogic>();
            if (GL.getPosition(xBoard + 3, yBoard) != null)
            {
                GameObject cp1 = GL.getPosition(xBoard + 3, yBoard);
                if (CanCastle && cp1.GetComponent<CHessmenScript>().CanCastle && GL.getPosition(xBoard + 1, yBoard) == null && GL.getPosition(xBoard + 2, yBoard) == null)
                {
                    CastlingMovePlate(xBoard + 2, yBoard, true);
                }
            }
            if(GL.getPosition(xBoard - 4, yBoard)!= null)
            { 
               GameObject cp2 = GL.getPosition(xBoard - 4, yBoard);
            if (CanCastle && cp2.GetComponent<CHessmenScript>().CanCastle && GL.getPosition(xBoard - 1, yBoard) == null && GL.getPosition(xBoard - 2, yBoard) == null && GL.getPosition(xBoard - 3, yBoard) == null)
            {
                CastlingMovePlate(xBoard - 2, yBoard, false);
            }
        }
        }
    }
    public void PointMovePlate(int X, int Y)
    {
        GameLogic GL = controller.GetComponent<GameLogic>();

        if (GL.positionOnBoard(X, Y))
        {
            GameObject cp = GL.getPosition(X, Y);
            if (cp == null)
            {
                MovePlateSpawn(X, Y);
            }
            else if (cp.GetComponent<CHessmenScript>().player != player)
            {
                movePlateAttackSpawn(X, Y);
            }
        }

    }
    public void BlackPawnMovePlate(int X, int Y)
    {
        GameLogic GL = controller.GetComponent<GameLogic>();
        if (GL.positionOnBoard(X, Y))
        {
            if (GL.getPosition(X, Y) == null)
            {
                MovePlateSpawn(X, Y);
                EnPass = false;
            }
            if (Y == 5 && GL.getPosition(X, Y - 1) == null && GL.getPosition(X, Y) == null)
            {
                MovePlateSpawn(X, Y - 1);
                EnPass = true;
            }
            if (GL.positionOnBoard(X + 1, Y) && GL.getPosition(X + 1, Y) != null && GL.getPosition(X + 1, Y).GetComponent<CHessmenScript>().player != player)
            {
                movePlateAttackSpawn(X + 1, Y);
            }
            if (GL.positionOnBoard(X - 1, Y) && GL.getPosition(X - 1, Y) != null && GL.getPosition(X - 1, Y).GetComponent<CHessmenScript>().player != player)
            {
                movePlateAttackSpawn(X - 1, Y);
            }
            if (GL.positionOnBoard(X + 1, Y) && GL.getPosition(X + 1, Y + 1) != null && GL.getPosition(X + 1, Y + 1).GetComponent<CHessmenScript>().player != player && GL.getPosition(X + 1, Y + 1).GetComponent<CHessmenScript>().EnPass)
            {
                if (!(lastPlayedEnPass > GL.turns + 1))
                {
                    EnPassAttackSpawnBlack(X + 1, Y);
                }
            }
            if (GL.positionOnBoard(X - 1, Y) && GL.getPosition(X - 1, Y + 1) != null && GL.getPosition(X - 1, Y + 1).GetComponent<CHessmenScript>().player != player && GL.getPosition(X - 1, Y + 1).GetComponent<CHessmenScript>().EnPass)
            {
                if (!(lastPlayedEnPass > GL.turns + 1))
                    EnPassAttackSpawnBlack(X - 1, Y);
            }
        }
    }
    public void WhitePawnMovePlate(int X, int Y)
    {
        GameLogic GL = controller.GetComponent<GameLogic>();
        if (GL.positionOnBoard(X, Y))
        {
            if (GL.getPosition(X, Y) == null)
            {
                MovePlateSpawn(X, Y);
                EnPass = false;
            }
            if (Y == 2 && GL.getPosition(X, Y + 1) == null && GL.getPosition(X, Y) == null)
            {
                MovePlateSpawn(X, Y + 1);
                EnPass = true;
            }
            if (GL.positionOnBoard(X + 1, Y) && GL.getPosition(X + 1, Y) != null && GL.getPosition(X + 1, Y).GetComponent<CHessmenScript>().player != player)
            {
                movePlateAttackSpawn(X + 1, Y);
                EnPass = false;
            }
            if (GL.positionOnBoard(X - 1, Y) && GL.getPosition(X - 1, Y) != null && GL.getPosition(X - 1, Y).GetComponent<CHessmenScript>().player != player)
            {
                movePlateAttackSpawn(X - 1, Y);
                EnPass = false;
            }
            if (GL.positionOnBoard(X + 1, Y) && GL.getPosition(X + 1, Y - 1) != null && GL.getPosition(X + 1, Y - 1).GetComponent<CHessmenScript>().player != player && GL.getPosition(X + 1, Y - 1).GetComponent<CHessmenScript>().EnPass)
            {
                EnPass = false;
                if (!(lastPlayedEnPass > GL.turns + 1))
                    EnPassAttackSpawnWhite(X + 1, Y);
            }
            if (GL.positionOnBoard(X - 1, Y) && GL.getPosition(X - 1, Y - 1) != null && GL.getPosition(X - 1, Y - 1).GetComponent<CHessmenScript>().player != player && GL.getPosition(X - 1, Y - 1).GetComponent<CHessmenScript>().EnPass)
            {
                EnPass = false;
                if (!(lastPlayedEnPass > GL.turns + 1))
                    EnPassAttackSpawnWhite(X - 1, Y);
            }
        }
    }
    public void MovePlateSpawn(int MatrixX, int MatrixY)
    {
        float x = MatrixX;
        float y = MatrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();

        mpScript.SetReferance(gameObject);
        mpScript.SetCoords(MatrixX, MatrixY);
        if (EnPass)
        {
            GameLogic GL = controller.GetComponent<GameLogic>();
            lastPlayedEnPass = GL.turns;
            Debug.Log("Last played EnPassaent " + lastPlayedEnPass);
        }

    }
    public void movePlateAttackSpawn(int MatrixX, int MatrixY)
    {
        float x = MatrixX;
        float y = MatrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.AttackPiece = true;
        mpScript.SetReferance(gameObject);
        mpScript.SetCoords(MatrixX, MatrixY);

    }
    public void EnPassAttackSpawnWhite(int MatrixX, int MatrixY)
    {
        float x = MatrixX;
        float y = MatrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.AttackPiecePass = true;
        mpScript.SetReferance(gameObject);
        mpScript.SetCoords(MatrixX, MatrixY);
    }
    public void EnPassAttackSpawnBlack(int MatrixX, int MatrixY)
    {
        float x = MatrixX;
        float y = MatrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.AttackPiecePassBlack = true;
        mpScript.SetReferance(gameObject);
        mpScript.SetCoords(MatrixX, MatrixY);
    }
    public void CastlingMovePlate(int X, int Y, bool Short)
    {
        GameLogic GL = controller.GetComponent<GameLogic>();

        if (GL.positionOnBoard(X, Y))
        {
            GameObject cp = GL.getPosition(X, Y);
            if (cp == null && GL.positionOnBoard(X - 2, Y) && GL.getPosition(X - 2, Y))
            {
                float x = X;
                float y = Y;

                x *= 0.66f;
                y *= 0.66f;

                x += -2.3f;
                y += -2.3f;

                GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3), Quaternion.identity);
                MovePlate mpScript = mp.GetComponent<MovePlate>();
                mpScript.SetReferance(gameObject);
                mpScript.SetCoords(X, Y);
                mpScript.Castled = true;
                mpScript.shortCastle = Short;
            }
            if (cp == null && GL.positionOnBoard(X + 2, Y) && GL.getPosition(X + 2, Y))
            {
                float x = X;
                float y = Y;

                x *= 0.66f;
                y *= 0.66f;

                x += -2.3f;
                y += -2.3f;

                GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3), Quaternion.identity);
                MovePlate mpScript = mp.GetComponent<MovePlate>();
                mpScript.SetReferance(gameObject);
                mpScript.SetCoords(X, Y);
                mpScript.Castled = true;
                mpScript.shortCastle = Short;
            }
        }
    }
}
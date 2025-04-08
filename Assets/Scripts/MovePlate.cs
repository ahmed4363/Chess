using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;
    GameObject referance = null;


    //Board Positions
    int matrixX;
    int matrixY;

    //False; movement, true; Attacking
    public bool AttackPiece = false;
    public bool AttackPiecePass = false;
    public bool AttackPiecePassBlack = false;

    //Referances
    public CHessmenScript Rf;
    public GameLogic GL;
    public bool Castled = false;
    public bool shortCastle;
    public void Start()
    {
        CHessmenScript cHessmenScript = referance.GetComponent<CHessmenScript>();
        Rf = cHessmenScript;
        controller = GameObject.FindGameObjectWithTag("GameController");
        GL = controller.GetComponent<GameLogic>();
        if(AttackPiece || AttackPiecePass || AttackPiecePassBlack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 1f);
        }
    }
    public void OnMouseDown()
    {
        if(AttackPiece)
        {
            GameObject cp = GL.getPosition(matrixX, matrixY);
            if(cp.name == "wK")
            {
                GL.WinnerText("Black");
            }
            if (cp.name == "bK")
            {
                GL.WinnerText("White");
            }
            Destroy(cp);
        }
        if(Castled && shortCastle && GL.GetCurrentPlayer() == "white")
        {
            GL.getPosition(7, 0).GetComponent<CHessmenScript>().SetXBoard(5);
            GL.getPosition(7, 0).GetComponent<CHessmenScript>().SetYBoard(Rf.yBoard);
            GL.getPosition(7, 0).GetComponent<CHessmenScript>().SetCords();
            GL.SetPosition(GL.getPosition(7, 0));
        }
        if (Castled && !shortCastle && GL.GetCurrentPlayer() == "white")
        {
            GL.getPosition(0, 0).GetComponent<CHessmenScript>().SetXBoard(3);
            GL.getPosition(0, 0).GetComponent<CHessmenScript>().SetYBoard(Rf.yBoard);
            GL.getPosition(0, 0).GetComponent<CHessmenScript>().SetCords();
            GL.SetPosition(GL.getPosition(0, 0));
        }
        if (Castled && shortCastle && GL.GetCurrentPlayer() == "black")
        {
            GL.getPosition(7, 7).GetComponent<CHessmenScript>().SetXBoard(5);
            GL.getPosition(7, 7).GetComponent<CHessmenScript>().SetYBoard(Rf.yBoard);
            GL.getPosition(7, 7).GetComponent<CHessmenScript>().SetCords();
            GL.SetPosition(GL.getPosition(7, 7));
        }
        if (Castled && !shortCastle && GL.GetCurrentPlayer() == "black")
        {
            GL.getPosition(0, 7).GetComponent<CHessmenScript>().SetXBoard(3);
            GL.getPosition(0, 7).GetComponent<CHessmenScript>().SetYBoard(Rf.yBoard);
            GL.getPosition(0, 7).GetComponent<CHessmenScript>().SetCords();
            GL.SetPosition(GL.getPosition(0, 7));
        }
        if (AttackPiecePass)
        {
            GameObject cp = GL.getPosition(matrixX, matrixY - 1);
            Destroy(cp);
        }
        if (AttackPiecePassBlack)
        {
            GameObject cp = GL.getPosition(matrixX, matrixY + 1);
            Destroy(cp);
        }
        GL.PlayMovePiece();
        GL.SetPositionEmpty(Rf.GetXBoard(), Rf.GetYBoard());
        Rf.SetXBoard(matrixX);
        Rf.SetYBoard(matrixY);
        Rf.SetCords();

        GL.SetPosition(referance);
        GL.NextTurn();
        Rf.DestroyMovePlates();
        Rf.CanCastle = false;
    }
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }
    public void SetReferance(GameObject obj)
    {
        referance = obj;
    }
    public GameObject GetReferance()
    {
        return referance;
    }
}

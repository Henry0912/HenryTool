using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM : MonoBehaviour
{
    public enum Round
    {
        non,
        player1,
        player2
    }

    public enum Process
    {
        start,
        waitLogin,
        decidePlayer,
        p1Action,
        p2Action,
        checkWin,
        end,
    }

    public List<HPlayer> allPlayer = new List<HPlayer>();
    public Round round = Round.non;
    public Process process = Process.start;
    int roundCount = -1;
    public int Number = 0;

    public void Login(HPlayer player) {
        allPlayer.Add(player);
        player.RpcSetPlayer(allPlayer.Count);

        if (allPlayer.Count == 2)//
            process = Process.decidePlayer;//
    }

    public void AddNunber(int addNun) {
        Number += addNun;
        if (Number >= 20)
            process = Process.checkWin;
        else
            process = Process.decidePlayer;
    }

    void Start() {
        process = Process.waitLogin;
    }

    void Update() {
        switch (process) {
            case Process.waitLogin:
                foreach (HPlayer pl in allPlayer)
                    pl.SysMsg = "等待玩家連線...";
                break;

            case Process.decidePlayer:
                roundCount++;
                if (roundCount % 2 == 0)//p1回合
                {
                    round = Round.player1;
                    process = Process.p1Action;

                }
                else//p2回合
                {
                    round = Round.player2;
                    process = Process.p2Action;
                }
                break;

            case Process.p1Action:
                allPlayer[0].SysMsg = "輪到你了!";
                allPlayer[0].SetProcess(HPlayer.Process.action);
                allPlayer[1].SysMsg = "等待對方...";
                allPlayer[1].SetProcess(HPlayer.Process.wait);
                break;

            case Process.p2Action:
                allPlayer[1].SysMsg = "輪到你了!";
                allPlayer[1].SetProcess(HPlayer.Process.action);
                allPlayer[0].SysMsg = "等待對方...";
                allPlayer[0].SetProcess(HPlayer.Process.wait);
                break;

            case Process.checkWin:
                switch (round) {
                    case Round.player1:
                        allPlayer[0].SysMsg = "Winner";
                        allPlayer[1].SysMsg = "Loser";
                        break;
                    case Round.player2:
                        allPlayer[1].SysMsg = "Winner";
                        allPlayer[0].SysMsg = "Loser";
                        break;
                }

                allPlayer[0].SetProcess(HPlayer.Process.end);
                allPlayer[1].SetProcess(HPlayer.Process.end);
                process = Process.end;
                break;
        }
    }
}

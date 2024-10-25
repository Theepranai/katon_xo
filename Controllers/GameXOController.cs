using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace katon_xo.Controllers
{
    public static class TempValue
    {
        public static string[] Slot = ["", "", "", "", "", "", "", "", ""];
        public static string LastWinner = "";
        public static string Winner = "";
        public static int Xwin = 0;
        public static int Owin = 0;
        public static int Draw = 0;

    }

    [Authorize]
    public class GameXOController : Controller
    {
        public IActionResult Index(int id = -1, string text = "")
        {
            if (id >= 0)
            {
                TempValue.Slot[id] = text;

                BotRandom();

                if (CheckWiner())
                {
                    for (var i = 0; i < TempValue.Slot.Length; i++)
                    {
                        if (TempValue.Slot[i] == "") TempValue.Slot[i] = "-";
                    }

                    if (TempValue.Winner == "X") { TempValue.Xwin += 1; } else { TempValue.Owin += 1; }

                    TempValue.LastWinner += TempValue.Winner;

                    string special = "";

                    if (TempValue.LastWinner.Contains("XXX"))
                    {
                        TempValue.LastWinner = "";
                        TempValue.Xwin += 1;
                        special = " Special X + 1";
                    }

                    if (TempValue.LastWinner.Contains("OOO"))
                    {
                        TempValue.LastWinner = "";
                        TempValue.Owin += 1;
                        special = " Special O + 1";
                    }

                    TempData["winner"] = TempValue.Winner + " win!!!" + special;
                }

                if (TempValue.Slot.Count(x => x == "") == 0)
                {
                    TempValue.Draw += 1;
                }
            }

            return View(TempValue.Slot);
        }

        public IActionResult Reset()
        {
            TempValue.Slot = ["", "", "", "", "", "", "", "", ""];
            TempValue.Xwin = 0;
            TempValue.Owin = 0;
            TempValue.Draw = 0;
            return Redirect(nameof(Index));
        }

        public IActionResult NewGame()
        {
            TempValue.Slot = ["", "", "", "", "", "", "", "", ""];
            return Redirect(nameof(Index));
        }

        public bool CheckWiner()
        {
            //row
            for (int i = 0; i < TempValue.Slot.Length; i += 3)
            {
                if (TempValue.Slot[i] == "") continue;
                TempValue.Winner = TempValue.Slot[i];
                if (TempValue.Slot[i] == TempValue.Slot[i + 1] && TempValue.Slot[i + 1] == TempValue.Slot[i + 2]) return true;
            }

            //column
            for (int i = 0; i < 3; i += 1)
            {
                if (TempValue.Slot[i] == "") continue;
                TempValue.Winner = TempValue.Slot[i];
                if (TempValue.Slot[i] == TempValue.Slot[i + 3] && TempValue.Slot[i + 3] == TempValue.Slot[i + 6]) return true;
            }

            //left
            if (TempValue.Slot[4] == "") return false;

            TempValue.Winner = TempValue.Slot[4];

            if (TempValue.Slot[0] == TempValue.Slot[4] && TempValue.Slot[4] == TempValue.Slot[8]) return true;
            if (TempValue.Slot[2] == TempValue.Slot[4] && TempValue.Slot[4] == TempValue.Slot[6]) return true;


            return false;
        }

        public void BotRandom()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, 9);

            if(TempValue.Slot.Count(x => x == "") > 0)
            {
                if (TempValue.Slot[num] != "")
                {
                    BotRandom();
                    return;
                }
                TempValue.Slot[num] = "O";
            }
        }
    }
}

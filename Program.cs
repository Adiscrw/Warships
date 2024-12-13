




using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;


class WarShips
{
    const string inputInfo = "Velikost lode 1-5 policek\nSmery umisteni:\n   1:dolu\n   2:doprava\n   3:nahoru\n   4:doleva";
    const string BoatSymol = " O";
    const string WeaponShootContinueSymbol = " O";
    const string WeaponShootStopSymbol = " S";
    const string WeaponMissSymbol = "  ";
    const string WeaponMissedSymbol = " +";
    const string WeaponHitSymbol = " x";
    const string EmptyCellSymbol = " ~";

    const int WeaponTypeBasic = 0;
    const int WeaponTypeTorpedoHorizontal = 1;
    const int WeaponTypeTorpedoVertival = 2;
    const int WeaponTypeCarpetHorizontal = 3;
    const int WeaponTypeCarpetVertical = 4;
    const int WeaponTypeCrossFire = 5;

    const int PlaygroundSize = 12;




    static string[,] GetTargetPattern(int weapon)
    {
        switch (weapon)
        {
            case WeaponTypeBasic:
                // basic weapon
                return new string[,] { { WeaponShootContinueSymbol } };
            case WeaponTypeTorpedoHorizontal:
                // torpedo - vertical
                return new string[,] { { WeaponShootStopSymbol, WeaponShootStopSymbol, WeaponShootStopSymbol, WeaponShootStopSymbol, WeaponShootStopSymbol, WeaponShootStopSymbol, WeaponShootStopSymbol, WeaponShootStopSymbol, WeaponShootStopSymbol, WeaponShootStopSymbol } };
            case WeaponTypeTorpedoVertival:
                // torpero - horizontal
                return new string[,] { { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol },
                                   { WeaponShootStopSymbol } };
            case WeaponTypeCarpetHorizontal:
                return new string[,] { { WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol } };
            case WeaponTypeCarpetVertical:
                return new string[,] { { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol },
                                    { WeaponShootContinueSymbol } };
            case WeaponTypeCrossFire:
                return new string[,] { { WeaponMissSymbol, WeaponShootContinueSymbol, WeaponMissSymbol },
                                   { WeaponShootContinueSymbol, WeaponShootContinueSymbol, WeaponShootContinueSymbol },
                                   { WeaponMissSymbol, WeaponShootContinueSymbol, WeaponMissSymbol } };
            default:
                return null;
        }
    }


    static int GetWeaponPrice(int weapon)
    {
        switch (weapon)
        {
            case WeaponTypeBasic:
                return 10;
            case WeaponTypeTorpedoHorizontal:
                return 200;
            case WeaponTypeTorpedoVertival:
                return 200;
            case WeaponTypeCarpetHorizontal:
                return 1;
            case WeaponTypeCarpetVertical:
                return 1;
            case WeaponTypeCrossFire:
                return 1;

            default:
                return 0;
        }
    }


    static void ArrayInicialization(string[,] mapPlayer)
    {
        for (int i = 0; i < mapPlayer.GetLength(1); i++)
        {
            mapPlayer[0, i] = string.Format("{0:D2}", i);
            mapPlayer[i, 0] = string.Format("{0:D2}", i);
        }
        for (int i = 1; i < mapPlayer.GetLength(0); i++)
        {
            for (int j = 1; j < mapPlayer.GetLength(1) - 1; j++)
            {
                mapPlayer[i, j] = EmptyCellSymbol;
            }
        }
        for (int i = 0; i < mapPlayer.GetLength(1); i++)
        {
            mapPlayer[PlaygroundSize - 1, i] = " ";
            mapPlayer[i, PlaygroundSize - 1] = " ";
        }
    }

    static void BoatPlacement(string[,] array, int boatLength, int x, int y, int direction)
    {

        if (direction == 1)
        {
            for (int i = 0; i < boatLength; i++)
            {
                array[x + i, y] = BoatSymol;
            }
        }
        else if (direction == 2)
        {
            for (int i = 0; i < boatLength; i++)
            {
                array[x, y + i] = BoatSymol;
            }
        }
        else if (direction == 3)
        {
            for (int i = 0; i < boatLength; i++)
            {
                array[x - i, y] = BoatSymol;
            }
        }
        else if (direction == 4)
        {
            for (int i = 0; i < boatLength; i++)
            {
                array[x, y - i] = BoatSymol;
            }
        }
        else
        {
            Console.WriteLine("Neplatny vyraz, zadejte znovu");
        }
    }
    static bool AroundCheck(string[,] array, int boatLength, int x, int y, int direction)
    {
        bool status = false;
        if (direction == 1)
        {
            Console.WriteLine("Smer zadan spravne");
            for (int i = -1; i < boatLength + 1; i++)
            {
                if (0 <= x + i && x + i <= 11 && array[x + i, y + 1] != BoatSymol && array[x + i, y - 1] != BoatSymol && array[x + i, y] != BoatSymol)
                {
                    status = true;
                    Console.WriteLine("Kolem lodi nelezi jina lod");
                    Console.WriteLine("souradnice je na mape");
                }
                else
                {
                    Console.WriteLine("Chyba");
                    status = false;
                    return status;
                }

            }
        }
        else if (direction == 2)
        {
            for (int i = -1; i < boatLength + 1; i++)
            {
                if (0 <= y + i && y + i <= 11 && array[x + 1, y + i] != BoatSymol && array[x - 1, y + i] != BoatSymol && array[x, y + i] != BoatSymol)
                {
                    status = true;
                }
                else
                {
                    status = false;
                    return status;
                }
            }
        }
        else if (direction == 3)
        {
            for (int i = -1; i < boatLength + 1; i++)
            {
                if (0 <= x - i && x - i <= 11 && array[x - i, y + 1] != BoatSymol && array[x - i, y - 1] != BoatSymol && array[x - i, y] != BoatSymol)
                {
                    status = true;
                }
                else
                {
                    status = false;
                    return status;
                }
            }
        }
        else if (direction == 4)
        {
            for (int i = -1; i < boatLength + 1; i++)
            {
                if (0 <= y - i && y - i <= 12 && array[x + 1, y - i] != BoatSymol && array[x - 1, y - i] != BoatSymol && array[x, y - i] != BoatSymol)
                {
                    status = true;
                }
                else
                {
                    status = false;
                    return status;
                }
            }
        }
        else
        {
            status = false;
        }
        return status;
    }

    static void PlayerWiewMap(string[,] playerView, List<Coordinates> hits)
    {
        foreach (var hit in hits)
        {
            playerView[hit.Y, hit.X] = WeaponHitSymbol;

        }
        Print2DArray(playerView);
    }



    static List<Coordinates> CheckHit(string[,] pattern, string[,] playground, int shot_x, int shot_y)
    {
        List<Coordinates> result = new List<Coordinates>();
        for (int y = 0; y < pattern.Length; y++)
        {
            for (int x = 0; x < pattern[y, 0].Length; x++)
            {
                if (y + shot_y >= playground.GetLength(0) || x + shot_x >= playground.GetLength(1))
                {
                    // zkontroloujeme, jestli uz nejsme mimo hriste
                    continue;
                }

                if (playground[y + shot_y,x + shot_x] == BoatSymol)
                {
                    // dale zpracovavame je tehdy, pokud na danem miste hriste je kus lodi
                    if (pattern[y,x] == WeaponShootContinueSymbol || pattern[y, x] == WeaponShootStopSymbol)
                    {
                        // pokud je v matici symbol zasahu, tak zaznamename zasah
                        result.Add(new Coordinates(y + shot_y, x + shot_x));
                    }

                    if (pattern[y, x] == WeaponShootStopSymbol)
                    {
                        // pokud se jedna o zasah, po kterem se ma koncit, tak koncime
                        break;
                    }
                }
            }
        }

        /*
        if(weapon == 0) 
        {
            if (playground[x,y] == pattern[0,0])
            {
                playground[x, y] = "x";
                result.Add(new Coordinates(x, y));
            }
            else 
            {

            }
        }
        else if(weapon == 1) //xtorpedo
        {
            for (int i = 0; i <= playground.GetLength(0)-1; i++)
            {
                if (playground[x,y+i] == pattern[1,y+i])
                {
                    playground[x + i, y] = "x";
                    i = playground.GetLength(0);
                    result.Add(new Coordinates(x+i, y));
                }
                else
                {

                }
            }

        }
        else if (weapon == 2)//ytorpedo
        {
            for (int i = 0; i <= playground.GetLength(1)-1; i++)
            {
                if (playground[x+i, y] == pattern[1, y + i])
                {
                    playground[x+i, y] = "x";
                    i = playground.GetLength(1);
                    result.Add(new Coordinates(x+i, y));
                }
                else
                {

                }
            }
        }
        else if(weapon == 3) //xkobercovy nalet
        {
            for (int i = 0; i <= playground.GetLength(0) - 1; i++)
            {
                if (playground[x, y + i] == pattern[1, y + i])
                {
                    playground[x + i, y] = "x";
                    result.Add(new Coordinates(x + i, y));
                }
                else
                {

                }
            }
        }
        else if (weapon == 4)//ykobercovy nalet
        {
            for (int i = 0; i <= playground.GetLength(1) - 1; i++)
            {
                if (playground[x + i, y] == pattern[1, y + i])
                {
                    playground[x + i, y] = "x";
                    result.Add(new Coordinates(x + i, y));
                }
                else
                {

                }
            }
        }
        else if (weapon == 5)//kriz
        {
            for (int i = -1; i < playground.GetLength(0)&&i<playground.GetLength(1)&& i<=1; i++)
            {
                for (int j = -1; j <=1; j++)
                {
                    if (pattern[i + 1, j + 1] == " ")
                    {

                    }
                    else if (playground[x+i,y+j] == pattern[i+1,j+1])
                    {
                        result.Add(new Coordinates(x+i, y+j));
                        playground[x + i, y + j] = "x";
                    }
                    else
                    {

                    }

                }
            }
        }
        */

        return result;
    }






    /*
    static void ShootComputer(List<Coordinates>HitsComputer, string[,] playground, int money)
    {
        const int basic = 10;
        const int torpedo = 200;
        const int carpet = 510;
        const int crossfire = 600;
        int weapon = -1;
        int x = -1;
        int y = -1;
        Random rng = new Random();


        while (weapon < 0)
        {
            weapon = rng.Next(0,6);

            string[,] weapon_pattern = GetTargetPattern(weapon);
            int weapon_price = GetWeaponPrice(weapon);
            if (weapon_price >= 0)
            {
                if (weapon_price < money)
                {
                    do
                    {
                        x = rng.Next(1, PlaygroundSize);
                        y = rng.Next(1, PlaygroundSize);
                    }
                    while (HitsComputer.Find(x,y));


                    if (x > 0 && x < PlaygroundSize)
                    {
                        Console.WriteLine("Zadejte radek: ");


                        if (y > 0 && y < PlaygroundSize)
                        {
                            // zjistime, kam se zbran trefila
                            HitsComputer = CheckHit(weapon_pattern, playground, x, y);

                            // zakreslime zasahy do hraciho pole
                            PlayerWiewMap(playerWiew, hits);
                        }
                        else
                        {
                            Console.WriteLine("Zvolili jste souradnici mimo hraci pole");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Zvolili jste souradnici mimo hraci pole");
                    }
                }
                else
                {
                    Console.WriteLine("Cena zbrane {0} je vyssi nez mate k dispozici {1}", weapon_price, money);
                }

            }
            else
            {
                Console.WriteLine("Zvolili jste neexistujici zbran.");
            }

            if (weapon == 1 || weapon == 2 && money - torpedo >= 0)
            {
                money -= torpedo;
            }
            else if (weapon == 3 || weapon == 4 && money - carpet >= 0)
            {
                money -= carpet;
            }
            else if (weapon == 5 && money - crossfire >= 0)
            {
                money -= crossfire;
            }
            else if (weapon == 0 && money - basic >= 0)
            {
                money -= basic;
            }
            else if (money - basic <= 0)
            {
                Console.WriteLine("Pocitac vyhral, dosly vam  penize");
                break;
            }
            else
            {
                Console.Write(" Nemate dost penez, zvolte levnejsi zbran\n");
                weapon = -1;
            }
        }


        while (weapon < 0)
        {
            weapon = rng.Next(0, 6);
            if (weapon == 1 || weapon == 2 && money - torpedo >= 0)
            {
                money -= torpedo;
            }
            else if (weapon == 3 || weapon == 4 && money - carpet >= 0)
            {
                money -= carpet;
            }
            else if (weapon == 5 && money - crossfire >= 0)
            {
                money -= crossfire;
            }
            else if (weapon == 0 && money - basic >= 0)
            {
                money -= basic;
            }
            else if (money - basic <= 0)
            {
                Console.WriteLine("Vyhrali jste, pocitaci dosly penize");
                break;
            }
            else
            {
                weapon = -1;
            }
        }
        while (0 < x && x < 12)
        {
            if (weapon == 2 || weapon == 4)
            {
                x = 1;
            }
            else
            {
                x = rng.Next(1, 12);
            }
        }
        while (0 < y && y < 12)
        {
            if (weapon == 1 || weapon == 3)
            {
                y = 1;
            }
            else
            {
                y = rng.Next(1, 12);
            }
        }
        PlayerWiewMap(pattern, CheckHit(GetTargetPattern(weapon), playground, x, y));
    }
    */

    static void Shoot(string[,] playerWiew, string[,] playground, int money)
    {
        const int basic = 10;
        const int torpedo = 200;
        const int carpet = 500;
        const int crossfire = 600;
        int weapon = -1;
        int x = -1;
        int y = -1;

        while (weapon < 0)
        {
            Console.WriteLine("Vyberte zbran (0=Basic, 1=TorpedoHorizontal...: ");
            weapon = Convert.ToInt32(Console.ReadLine());

            string[,] weapon_pattern = GetTargetPattern(weapon);
            int weapon_price = GetWeaponPrice(weapon);
            if (weapon_price >= 0)
            {
                if (weapon_price < money)
                {
                    Console.WriteLine("Zadejte sloupec: ");
                    x = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Zadejte radek: ");
                    y = Convert.ToInt32(Console.ReadLine());

                    if (y > 0 && y < PlaygroundSize)
                    {
                        if (x > 0 && x < PlaygroundSize)
                        {
                            // zjistime, kam se zbran trefila
                            List<Coordinates> hits = CheckHit(weapon_pattern, playground, x, y);

                            // zakreslime zasahy do hraciho pole
                            PlayerWiewMap(playerWiew, hits);
                        }
                        else
                        {
                            Console.WriteLine("Zvolili jste souradnici mimo hraci pole");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Zvolili jste souradnici mimo hraci pole");
                    }
                }
                else
                {
                    Console.WriteLine("Cena zbrane {0} je vyssi nez mate k dispozici {1}", weapon_price, money);
                }

            }
            else
            {
                Console.WriteLine("Zvolili jste neexistujici zbran.");
            }

            if (weapon == 1 || weapon == 2 && money - torpedo >= 0)
            {
                money -= torpedo;
            }
            else if (weapon == 3 || weapon == 4 && money - carpet >= 0)
            {
                money -= carpet;
            }
            else if (weapon == 5 && money - crossfire >= 0)
            {
                money -= crossfire;
            }
            else if (weapon == 0 && money - basic >= 0)
            {
                money -= basic;
            }
            else if (money - basic <= 0)
            {
                Console.WriteLine("Pocitac vyhral, dosly vam  penize");
                break;
            }
            else
            {
                Console.Write(" Nemate dost penez, zvolte levnejsi zbran\n");
                weapon = -1;
            }
        }
        while (0 < x && x < 12)
        {
            if (weapon == 1  || weapon == 3)
            {
                x = 1;
            }
            else
            {
                Console.WriteLine("Zadejte souradnice sloupce: ");
                x = Convert.ToInt32(Console.ReadLine());
            }
        }
        while (0 < y && y < 12)
        {
            if (weapon == 2 || weapon == 4)
            {
                y = 1;
            }
            else
            {
                Console.WriteLine("zadejte souradnice radku: ");
                y = Convert.ToInt32(Console.ReadLine());
            }
        }
        PlayerWiewMap(playerWiew, CheckHit(GetTargetPattern(weapon), playground, x, y));
    }

    static string Input(string array)
    {
        Console.WriteLine(array);
        string value = Console.ReadLine();
        return value;
    }

    static void Print2DArray(string[,] arrayToPrint)
    {
        string[,] array = new string[5, 5];
        for (int i = 0; i < arrayToPrint.GetLength(0); i++)
        {
            for (int j = 0; j < arrayToPrint.GetLength(1); j++)
            {
                Console.Write(arrayToPrint[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Funguje main");

        string[,] mapPlayer = new string[12, 12];
        string[,] mapComputer = new string[12, 12];
        string[,] playerWiew = new string[12, 12];
        List<Coordinates> HitsComputer = new List<Coordinates>();
        int moneyPlayer = 500;
        int moneyComputer = 500;


        ArrayInicialization(mapComputer);
        for (int i = 5; i > 0; i--)
        {
            Random rng = new Random();
            int x = rng.Next(1, 11);
            int y = rng.Next(1, 11);
            int direction = rng.Next(1, 5);
            if (AroundCheck(mapComputer, i, x, y, direction) == true)
            {
                BoatPlacement(mapComputer, i, x, y, direction);
                Print2DArray(mapComputer);
            }
            else
            {
                i++;
            }
        }

        Print2DArray(mapComputer);
        ArrayInicialization(mapPlayer);
        ArrayInicialization(playerWiew);
        Print2DArray(mapPlayer);


        BoatPlacement(mapPlayer, 5, 1, 1, 1);
        BoatPlacement(mapPlayer, 4, 1, 3, 1);
        BoatPlacement(mapPlayer, 3, 1, 5, 1);
        BoatPlacement(mapPlayer, 2, 1, 7, 1);
        BoatPlacement(mapPlayer, 1, 1, 10, 1);

        Print2DArray(mapPlayer);

        /*
        for (int i = 5; i > 0; i--)
        {
            Console.WriteLine(inputInfo);
            int x = Convert.ToInt32(Input("Zadejte pozici na radku: "));
            int y = Convert.ToInt32(Input("Zadejte pozici na sloupci: "));
            int length = Convert.ToInt32(Input("Zadejte delku lode: "));
            int direction = Convert.ToInt32(Input("Zadejte smer, 1,2,3,4"));
            if (AroundCheck(mapPlayer, length, x, y, direction) == true)
            {
                BoatPlacement(mapPlayer, length, x, y, direction);
                Print2DArray(mapPlayer);
            }
            else
            {
                Console.WriteLine("Chyba v zadani, lod zasahuje mimo pole nebo lezi blizko jine lodi zkuste to znova");
                i++;
            }
        }
        */

        while (true)
        {
            Shoot(playerWiew, mapComputer, moneyPlayer);
            //ShootComputer(mapPlayer, moneyComputer);
        }
    }

    public struct Coordinates
    {
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => $"({X}, {Y})";
    }
}

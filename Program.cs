




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
    const string WeaponMissSymbol = " *";
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

    const int HitReward = 300;

    const string instructiones = "Vitejte ve hre lode, nejdrive umistete sve lode na mapu.\n" +
        "Potom zvolte zbran, kterou pouzijete, pokud na ni budete mit dost penez. ";





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
                return 550;
            case WeaponTypeCarpetVertical:
                return 550;
            case WeaponTypeCrossFire:
                return 700;

            default:
                return 0;
        }
    }


    static void ArrayInicialization(string[,] playground) //inicializujeme pole
    {
        for (int i = 0; i < playground.GetLength(1); i++)
        {
            playground[0, i] = string.Format("{0:D2}", i);
            playground[i, 0] = string.Format("{0:D2}", i);
        }
        for (int i = 1; i < playground.GetLength(0); i++)
        {
            for (int j = 1; j < playground.GetLength(1) - 1; j++)
            {
                playground[i, j] = EmptyCellSymbol;
            }
        }
        for (int i = 0; i < playground.GetLength(1); i++)
        {
            playground[PlaygroundSize - 1, i] = " ";
            playground[i, PlaygroundSize - 1] = " ";
        }
    }//

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
    }//umistujeme lod na mapu
    static bool AroundCheck(string[,] array, int boatLength, int x, int y, int direction)//pri zadavani kontrolujeme jestli v okoli nelezi jina lod
    {
        bool status = false;
        if (direction == 1)
        {
           // Console.WriteLine("Smer zadan spravne");
            for (int i = -1; i < boatLength + 1; i++)
            {
                if (0 <= x + i && x + i <= 11 && array[x + i, y + 1] != BoatSymol && array[x + i, y - 1] != BoatSymol && array[x + i, y] != BoatSymol)
                {
                    status = true;
                   // Console.WriteLine("Kolem lodi nelezi jina lod");
                   // Console.WriteLine("souradnice je na mape");
                }
                else
                {
                   // Console.WriteLine("Chyba");
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

    static int RecordHitsToPlayground(string[,] playground, List<Hit> hits,int money)
    {
        foreach (var hit in hits)
        {
            if (hit.HitType == HitTypes.Hit)
            {
                playground[hit.X, hit.Y] = WeaponHitSymbol;
                money += HitReward;
            }
            else
            {
                playground[hit.X, hit.Y] = WeaponMissSymbol;
            }
        }
        return money;
    }



    static List<Hit> CheckHit(string[,] pattern, string[,] playground, int shot_x, int shot_y)
    {
        List<Hit> result = new List<Hit>();
        bool stopIteration = false;

        for (int y = 0; y < pattern.GetLength(1); y++)
        {
            for (int x = 0; x < pattern.GetLength(0); x++)
            {
                if (y + shot_y >= playground.GetLength(0)-1 || x + shot_x >= playground.GetLength(1) - 1)
                {
                    // zkontroloujeme, jestli uz nejsme mimo hriste
                    continue;
                }

                switch (playground[x + shot_x, y + shot_y])
                {
                    case BoatSymol:
                        // dale zpracovavame je tehdy, pokud na danem miste hriste je kus lodi
                        if (pattern[x, y] == WeaponShootContinueSymbol || pattern[x, y] == WeaponShootStopSymbol)
                        {
                            // pokud je v matici symbol zasahu, tak zaznamename zasah
                            result.Add(new Hit(HitTypes.Hit, x + shot_x, y + shot_y));
                        }

                        if (pattern[x, y] == WeaponShootStopSymbol)
                        {
                            // pokud se jedna o zasah, po kterem se ma koncit, tak koncime
                            stopIteration = true;
                        }

                        break;

                    case EmptyCellSymbol:
                        // dale zpracovavame je tehdy, pokud na danem miste hriste je kus lodi
                        if (pattern[x, y] == WeaponShootContinueSymbol || pattern[x, y] == WeaponShootStopSymbol)
                        {
                            // pokud je v matici symbol zasahu, tak zaznamename zasah
                            result.Add(new Hit(HitTypes.Miss, x + shot_x, y + shot_y));
                        }

                        break;

                    default:
                        Console.WriteLine("Neocekavany symbol >{0}< na hracim poli [{1}, {2}]", playground[x + shot_x, y + shot_y], x + shot_x, y + shot_y);
                        break;
                }       

                if (stopIteration)
                {
                    break;
                }
            }

            if (stopIteration)
            {
                break;
            }
        }
        return result;
    }



    static int GetRandomWeapon(int moneyLimit)
    {
        Random rng = new Random();
        int selectedWeapon = -1;

        if (moneyLimit >= GetWeaponPrice(WeaponTypeBasic))
        {
            while (selectedWeapon < 0)
            {
                selectedWeapon = rng.Next(0, 6);

                int weaponPrice = GetWeaponPrice(selectedWeapon);

                if (weaponPrice > moneyLimit)
                {
                    selectedWeapon = -1;
                }
            }
        }

        return selectedWeapon;
    }

    static Hit GetRandomTarget(string [,] playground)
    {
        Random rng = new Random();
        Hit target = new Hit(HitTypes.Hit, -1, -1);

        while (target.X < 0)
        {
            target.X = rng.Next(1, PlaygroundSize);
            target.Y = rng.Next(1, PlaygroundSize);

            if (playground[target.X, target.Y] == WeaponHitSymbol || playground[target.X, target.Y] == WeaponMissSymbol)
            {
                target.X = -1;
                target.Y = -1;
            }
        }

        return target;
    }

    static bool CheckAllBoatsHit(string[,] playground)
    {
        bool status = false;
        for (int i = 1; i < playground.GetLength(0); i++)
        {
            for (int j = 1; j < playground.GetLength(1); j++)
            {
                if (playground[i,j] == BoatSymol)
                {
                    status = true;
                }
                else
                {

                }
            }
        }
        return status;
    }

    static int ShootComputer(string[,] playground, int money)
    {
        Random rng = new Random();
        int weapon = GetRandomWeapon(money);

        if (weapon < 0)
        {
            Console.WriteLine("Computer exhausted all money - LOST.");
            return 0;
        }

        int weapon_price = GetWeaponPrice(weapon);
        string[,] weapon_pattern = GetTargetPattern(weapon);

        Hit target = GetRandomTarget(playground);

        // zjistime, kam se zbran trefila
        List<Hit> hits = CheckHit(weapon_pattern, playground,target.X, target.Y);

        // zakreslime zasahy do hraciho pole
        money -= weapon_price;
        money = RecordHitsToPlayground(playground, hits, money);
        Console.WriteLine("Penize pocitace: "+money);
        Print2DArray("Tvoje hrací plocha:", playground);
        return money;
    }


    static int Shoot(string[,] playerWiew, string[,] playground, int money)
    {
        int weapon = -1;
        int x = -1;
        int y = -1;

        while (weapon < 0)
        {
            while (weapon < 0 || weapon >5)
            {
                Console.WriteLine("Vyberte zbran \n0=Basic, cena 10\n 1=TorpedoHorizontal, cena 200\n 2=TorpedoVertical, cena 200\n 3=CarpetHorizontal, cena 550\n 4=CarpetVertical, cena 550\n 5=CrossFire, cena 700 ");
                weapon = Convert.ToInt32(Console.ReadLine());
            }
            

            string[,] weapon_pattern = GetTargetPattern(weapon);
            int weapon_price = GetWeaponPrice(weapon);
            if (weapon_price >= 0)
            {
                if (weapon_price < money)
                {
                    Console.WriteLine("Zadejte radek: ");
                    x = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Zadejte sloupec: ");
                    y = Convert.ToInt32(Console.ReadLine());

                    if (y > 0 && y < PlaygroundSize)
                    {
                        if (x > 0 && x < PlaygroundSize)
                        {
                            // zjistime, kam se zbran trefila
                            List<Hit> hits = CheckHit(weapon_pattern, playground, x, y);

                            // zakreslime zasahy do hraciho pole
                            money -= weapon_price;
                            money = RecordHitsToPlayground(playerWiew, hits,money);
                            Console.WriteLine("Vase penize: " + RecordHitsToPlayground(playground, hits, money));

                            Print2DArray("Hrací plocha protivníka:", playerWiew);


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
                    weapon = -1;
                }

            }
            else
            {
                Console.WriteLine("Zvolili jste neexistujici zbran.");
            }
        }
        return money;
    }

    static string Input(string array)
    {
        Console.WriteLine(array);
        string value = Console.ReadLine();
        return value;
    }

    static void Print2DArray(string message, string[,] arrayToPrint)
    {
        Console.WriteLine(message);
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

        string[,] mapPlayer = new string[12, 12];
        string[,] mapComputer = new string[12, 12];
        string[,] playerWiew = new string[12, 12];
        List<Hit> HitsComputer = new List<Hit>();
        Console.WriteLine(instructiones);
        


        ArrayInicialization(mapComputer);
        for (int i = 5; i > 0; i--)
        {
            //generujeme pole pocitace
            Random rng = new Random();
            int x = rng.Next(1, 11);
            int y = rng.Next(1, 11);
            int direction = rng.Next(1, 5);
            if (AroundCheck(mapComputer, i, x, y, direction) == true)
            {
                BoatPlacement(mapComputer, i, x, y, direction);
                //Print2DArray(mapComputer);
            }
            else
            {
                i++;
            }
        }

        //Print2DArray(mapComputer);



        ArrayInicialization(mapPlayer);
        BoatPlacement(mapPlayer, 5, 1, 1, 1);
        BoatPlacement(mapPlayer, 4, 1, 3, 1);
        BoatPlacement(mapPlayer, 3, 1, 5, 1);
        BoatPlacement(mapPlayer, 2, 1, 7, 1);
        BoatPlacement(mapPlayer, 1, 1, 10, 1);

        Print2DArray("Tvoje hrací plocha:", mapPlayer);
        

        /*
        ArrayInicialization(mapPlayer);
        ArrayInicialization(playerWiew);
        Print2DArray("Tvoje hrací plocha:", mapPlayer);
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
                Print2DArray("Tvoje hrací plocha:", mapPlayer);
            }
            else
            {
                Console.WriteLine("Chyba v zadani, lod zasahuje mimo pole nebo lezi blizko jine lodi zkuste to znova");
                i++;
            }
        }
        */
        int moneyPlayer = 500;
        int moneyComputer = 500;
        bool status = true;
        while (status == true)
        {           
            if (CheckAllBoatsHit(mapComputer) == false)
            {
                Console.WriteLine("Pocitac prohral, potopili jste vsechny lode");
                status = false;
                break;
            }

            if (moneyPlayer <= 0)
            {
                Console.WriteLine("Pocitac vyhral, dosly vam penize");
                status = false;
                break;
            }
            moneyPlayer = Shoot(playerWiew, mapComputer, moneyPlayer);
            if (CheckAllBoatsHit(mapPlayer) == false)
            {
                Console.WriteLine("Pocitac vyhral, potopil vsechny lode");
                status = false;
                break;
            }

            if (moneyComputer <= 0)
            {
                Console.WriteLine("Pocitac prohral, dosly mu penize");
                status = false;
                break;
            }
            moneyComputer = ShootComputer(mapPlayer, moneyComputer);
        }
        Console.ReadKey();
    }

    public struct Hit
    {
        public Hit(HitTypes hitType, int x, int y)
        {
            HitType = hitType;
            X = x;
            Y = y;
        }

        public HitTypes HitType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => $"({HitType}: {X}, {Y})";
    }

    public enum HitTypes
    {
        Miss = 0,
        Hit = 1
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameCharacters
{
    public class GameCharacter
    {
        private ConsoleKey select;
        public static int ch_amount;
        public static int active_character;
        private string character_name;
        private bool game_over;
        private bool used_heal;
        private bool named;
        private int number;
        private int coordinate_x;
        private int coordinate_y;
        private int hp;
        private int lives_amount;
        private bool camp;
        static Random rand = new Random();
        public static GameCharacter[] character = new GameCharacter[3];
        public GameCharacter(int index, string name, int x, int y, int hitpoints, int lives)
        {
            number = index;
            character_name = name;
            coordinate_x = x;
            coordinate_y = y;
            hp = hitpoints;
            lives_amount = lives;
        }
        private void ally_info() 
        { 
            for (int i = 0; i < ch_amount; i++)
            {
                character[i].camp_check();
                if (character[i].camp_check() == character[active_character - 1].camp_check())
                {
                    character[i].print();
                }
            }
            Thread.Sleep(3000);
        }
        public void camp_decide()
        {
            int option = rand.Next(0, 3);
            switch (option)
            {
                case 0:
                    camp = false;
                    break;
                case 1:
                    camp = true;
                    break;
                case 2:
                    camp = false;
                    break;
                case 3:
                    camp = true;
                    break;
            }
        }
        public void print()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Номер персонажа: {number}");
            Console.WriteLine($"Имя персонажа: {character_name}\n");
            Console.WriteLine($"Координата х: {coordinate_x}\n");
            Console.WriteLine($"Координата у: {coordinate_y}\n");
            Console.WriteLine($"Лагерь: {camp}\n");
            Console.WriteLine($"Количество здоровья: {hp}\n");
            Console.WriteLine($"Количество жизней: {lives_amount}\n");
        }
        private void movex()
        {
        movex:
            Console.WriteLine(": Введите значение, на которое вы хотите переместить своего персонажа по оси Х: ");
            int coordinate_move_x = int.Parse(Console.ReadLine());
            if (coordinate_move_x + coordinate_x > 5 || coordinate_move_x + coordinate_x < 0)
            {
                Console.WriteLine("Вы не можете зайти за границу по оси Х! (поле 5х5)");
                goto movex;
            }
            if (coordinate_move_x > 5 || coordinate_move_x < -5)
            {
                Console.WriteLine("Вы не можете переместиться так далеко! Максимальное значение - 5\n");
                goto movex;
            }
            else coordinate_x += coordinate_move_x;
        }
        private void movey()
        {
        movey:
            Console.WriteLine(": Введите значение, на которое вы хотите переместить своего персонажа по оси Y: ");
            int coordinate_move_y = int.Parse(Console.ReadLine());
            if (coordinate_move_y + coordinate_y > 5 || coordinate_move_y + coordinate_y < 0)
            {
                Console.WriteLine("Вы не можете зайти за границу по оси Y! (0 & 10)");
                goto movey;
            }
            if (coordinate_move_y > 5 || coordinate_move_y < -5)
            {
                Console.WriteLine("Вы не можете переместиться так далеко! Максимальное значение - 5\n");
                goto movey;
            }
            else coordinate_y += coordinate_move_y;
        }
        private void heal()
        {
            if (used_heal == false)
            {
                hp += 10;
                used_heal = true;
            }
        }
        private void damage(int uron) 
        {
            hp -= uron;
            if (hp < 0 && lives_amount != 0 || lives_amount > 0)
            {
                lives_amount -= 1;
                hp = 10;
            }
        }
        private bool life_check() 
        {
            if (hp < 0 && lives_amount == 0 || lives_amount < 0)
            {
                game_over = true;
            }
            return game_over;
        }
        public static int choose() 
        {
            Console.WriteLine(": Введите номер персонажа, за которого желаете сыграть:");
            active_character = int.Parse(Console.ReadLine());
            return active_character - 1;
        }
        private void actions() 
        {
            Console.WriteLine("1 - перенести персонажа по оси X");
            Console.WriteLine("2 - перенести персонажа по оси Y");
            Console.WriteLine("3 - исцелить персонажа");
            Console.WriteLine("4 - выбрать другого персонажа");
            Console.WriteLine("5 - вывести информацию о живых союзниках");
            select = Console.ReadKey().Key;
            switch (select) 
            {
                case ConsoleKey.NumPad1:
                    movex();
                    break;
                case ConsoleKey.NumPad2:
                    movey();
                    break;
                case ConsoleKey.NumPad3:
                    heal();
                    break;
                case ConsoleKey.NumPad4:
                    character[choose()].game();
                    break;
                case ConsoleKey.NumPad5:
                    ally_info();
                    break;
            }
        }
        private bool camp_check() 
        { 
            switch (camp) 
            {
                case false:
                    return false;
                case true:
                    return true;
            }
        }
        public void game() 
        { 
            while (game_over != true) 
            {
            start:
                Console.Clear();
                print();
                actions();
            pos_check:
                for (int i = 0; i < ch_amount; i++)
                {
                    if (character[active_character - 1].coordinate_x == character[i].coordinate_x && character[active_character - 1].coordinate_y == character[i].coordinate_y && character[active_character - 1] != character[i])
                    {
                        character[active_character - 1].camp_check();
                        character[i].camp_check();
                        if (character[active_character - 1].camp_check() == character[i].camp_check()) 
                        {
                            Console.WriteLine($": На вашей клетке другой союзный персонаж, это {character[i].character_name}");
                            Thread.Sleep(3000);
                        }
                        if (character[active_character - 1].camp_check() != character[i].camp_check())
                        {
                            Console.WriteLine($": На вашей клетке враг {character[i].character_name}! Вступить в бой? (Y/N/F)");
                            select = Console.ReadKey().Key;
                            switch (select)
                            {
                                case ConsoleKey.Y:
                                again:
                                    if (character[active_character - 1].life_check() == true)
                                    {
                                        Console.WriteLine(": К сожалению, ваш персонаж погиб");
                                        character[choose()].game();
                                        game_over = true;
                                    }
                                    if(character[i].life_check() == true)
                                    {
                                        Console.WriteLine(": Вы победили противника! Теперь он будет отображаться, как союзник.");
                                        Thread.Sleep(5000);
                                        character[i].camp = character[active_character - 1].camp;
                                        goto start;
                                    }
                                    int cout_d = rand.Next(2, 5);
                                    character[active_character - 1].damage(cout_d);
                                    Console.Clear();
                                    character[active_character - 1].print();
                                    character[i].print();
                                    Console.WriteLine($": Вы получили {cout_d} урона от противника!");
                                    cout_d = rand.Next(2, 5);
                                    character[i].damage(cout_d);
                                    Console.WriteLine($": Вы нанесли {cout_d} урона!\n");
                                    Console.WriteLine("Продолжить бой? (Y/N)");
                                    select = Console.ReadKey().Key;
                                    switch (select) 
                                    {
                                        case ConsoleKey.Y:
                                            goto again;
                                        case ConsoleKey.N:
                                            break;
                                    }
                                    break;
                                case ConsoleKey.N:
                                    if (coordinate_x < 5)
                                    {
                                        coordinate_x += 1;
                                    }
                                    else
                                    {
                                        coordinate_x -= 1;
                                    }
                                    if (coordinate_y < 5)
                                    {
                                        coordinate_y += 1;
                                    }
                                    else
                                    {
                                        coordinate_y -= 1;
                                    }
                                    Console.WriteLine(": Вы отказались от битвы, получено 5 урона");
                                    character[active_character - 1].hp -= 5;
                                    break;
                                case ConsoleKey.F:
                                    int chance_to_flee = rand.Next(0, 1);
                                    switch (chance_to_flee) 
                                    {
                                        case 0:
                                            character[active_character - 1].hp -= 5;
                                            Console.WriteLine(": Вы не сбежали, получено 5 урона");
                                            Thread.Sleep(3000);
                                            break;
                                        case 1:
                                            if (coordinate_x < 5)
                                            {
                                                coordinate_x += 1;
                                            }
                                            else
                                            {
                                                coordinate_x -= 1;
                                            }
                                            if (coordinate_y < 5)
                                            {
                                                coordinate_y += 1;
                                            }
                                            else
                                            {
                                                coordinate_y -= 1;
                                            }
                                            Console.WriteLine(": Вам удалось сбежать!");
                                            Thread.Sleep(3000);
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}

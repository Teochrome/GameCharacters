using GameCharacters;
Random rand = new Random();
bool named = false;
Console.WriteLine("Выберите количество персонажей:");
GameCharacter.ch_amount = int.Parse(Console.ReadLine());
GameCharacter.character = new GameCharacter[GameCharacter.ch_amount];
string[] names = new string[GameCharacter.ch_amount];
for (int j = 0; j < GameCharacter.ch_amount; j++)
{
    Console.WriteLine($"Введите имя персонажу №{j + 1}");
    names[j] = Console.ReadLine();
}
for (int i = 0; i < GameCharacter.ch_amount; i++)
{
    GameCharacter.character[i] = new GameCharacter(i + 1, names[i], rand.Next(0, 5), rand.Next(0, 5), 10, rand.Next(0, 3));
    GameCharacter.character[i].camp_decide();
}
for (int i = 0; i < GameCharacter.ch_amount; i++)
{
    GameCharacter.character[i].print();
}
GameCharacter.character[GameCharacter.choose()].game();
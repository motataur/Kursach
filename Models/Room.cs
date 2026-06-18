namespace Kursach.Models;

public class Room
{
    public int Number { get; set; }
    public int Area { get; set; }
    public string Name { get; set; }
    public int PricePerN {get; set;}
    public bool IsFree { get; set; }
}
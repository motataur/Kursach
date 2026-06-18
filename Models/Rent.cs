using System;

namespace Kursach.Models;

public class Rent
{
    public int Id { get; set; }
    public int RoomNumber  { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
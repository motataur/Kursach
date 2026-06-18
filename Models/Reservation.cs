using System;

namespace Kursach.Models;

public class Reservation
{
    public int Id { get; set; }
    public int RoomNumber { get; set; }
    public int ClientId { get; set; }
    public DateTime ReservationDate { get; set; }
    public string? ClientLastName { get; set; } // для отображения в таблице
}
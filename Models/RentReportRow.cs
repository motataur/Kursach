using System;

namespace Kursach.Models;

public class RentReportRow
{
    public int RoomNumber { get; set; }
    public string ClientFullName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Price { get; set; }
}
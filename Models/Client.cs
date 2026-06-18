using System;

namespace Kursach.Models;

public class Client
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PassportSeries { get; set; }
    public string? PassportNumber { get; set; }
}
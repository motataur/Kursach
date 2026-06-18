using System;
using System.Collections.Generic;
using Kursach.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Kursach.DB;

public class RentRepository
{
    private readonly string _connectionString;

    public RentRepository(IOptions<DBConnection> connect)
    {
        _connectionString = connect.Value.ConnectionString;
    }

    public List<Rent> GetRentList()
    {
        List<Rent> data = new List<Rent>();
        string sql = "select * from rent;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            using var dr = mc.ExecuteReader();
            while (dr.Read())
            {
                data.Add(new Rent
                {
                    Id = dr.GetInt32("id"),
                    RoomNumber = dr.GetInt32("roomNumber"),
                    ClientId = dr.GetInt32("clientId"),
                    StartDate = dr.GetDateTime("startDate"),
                    EndDate = dr.GetDateTime("endDate")
                });
            }
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        return data;
    }

    public void AddRent(Rent rent)
    {
        string sql = @"INSERT INTO rent (roomNumber, clientId, startDate, endDate) 
                       VALUES (@RoomNumber, @ClientId, @StartDate, @EndDate);";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@RoomNumber", rent.RoomNumber);
            mc.Parameters.AddWithValue("@ClientId", rent.ClientId);
            mc.Parameters.AddWithValue("@StartDate", rent.StartDate);
            mc.Parameters.AddWithValue("@EndDate", rent.EndDate);
            mc.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }

    public DateTime? GetEndDateForRoom(int roomNumber)
    {
        DateTime? endDate = null;
        string sql = @"SELECT endDate FROM rent 
                       WHERE roomNumber = @roomNumber 
                       AND endDate >= NOW() 
                       ORDER BY endDate DESC LIMIT 1;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@roomNumber", roomNumber);
            var result = mc.ExecuteScalar();
            if (result != null && result != DBNull.Value)
                endDate = Convert.ToDateTime(result);
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        return endDate;
    }

    public List<RentReportRow> GetReportData(DateTime from, DateTime to)
    {
        List<RentReportRow> data = new List<RentReportRow>();
        string sql = @"SELECT r.roomNumber, r.startDate, r.endDate, 
                              c.lastname, c.name AS clientName,
                              rm.pricePerN
                       FROM rent r
                       LEFT JOIN client c ON r.clientId = c.id
                       LEFT JOIN room rm ON r.roomNumber = rm.number
                       WHERE r.startDate >= @from AND r.startDate <= @to
                       ORDER BY r.startDate;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@from", from.Date);
            mc.Parameters.AddWithValue("@to", to.Date.AddDays(1).AddSeconds(-1));
            using var dr = mc.ExecuteReader();
            while (dr.Read())
            {
                var start = dr.GetDateTime("startDate");
                var end = dr.GetDateTime("endDate");
                var pricePerN = dr.GetInt32("pricePerN");
                var nights = Math.Max(1, (end.Date - start.Date).Days);

                data.Add(new RentReportRow
                {
                    RoomNumber = dr.GetInt32("roomNumber"),
                    ClientFullName = $"{dr.GetString("lastname")} {dr.GetString("clientName")}",
                    StartDate = start,
                    EndDate = end,
                    Price = pricePerN * nights
                });
            }
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        return data;
    }
}
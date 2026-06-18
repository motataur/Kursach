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
                       ORDER BY endDate DESC LIMIT 0;";
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
}
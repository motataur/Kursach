using System;
using System.Collections.Generic;
using Kursach.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Kursach.DB;

public class RoomRepository
{
    private readonly string _connectionString;

    public RoomRepository(IOptions<DBConnection> connect)
    {
        _connectionString = connect.Value.ConnectionString;
    }

    public List<Room> GetRoomList()
    {
        List<Room> data = new List<Room>();
        string sql = "SELECT r.*, rc.name FROM room r LEFT JOIN roomclass rc ON r.classId = rc.classId;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            using var dr = mc.ExecuteReader();
            while (dr.Read())
            {
                data.Add(new Room
                {
                    Number = dr.GetInt32("number"),
                    Area = dr.GetInt32("area"),
                    IsFree = dr.GetBoolean("isFree"),
                    Name = dr.GetString("name"),
                    PricePerN = dr.GetInt32("pricePerN"),
                });
            }
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        return data;
    }

    public Room? GetRoomByNumber(int number)
    {
        Room? room = null;
        string sql = @"SELECT r.*, rc.name 
                       FROM room r 
                       LEFT JOIN roomclass rc ON r.classId = rc.classId 
                       WHERE r.number = @number;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@number", number);
            using var dr = mc.ExecuteReader();
            if (dr.Read())
            {
                room = new Room
                {
                    Number = dr.GetInt32("number"),
                    Area = dr.GetInt32("area"),
                    IsFree = dr.GetBoolean("isFree"),
                    Name = dr.GetString("name"),
                    PricePerN = dr.GetInt32("pricePerN")
                };
            }
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        return room;
    }

    public void SetRoomFree(int number, bool isFree)
    {
        string sql = "UPDATE room SET isFree = @isFree WHERE number = @number;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@isFree", isFree);
            mc.Parameters.AddWithValue("@number", number);
            mc.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }

    public void FreeExpiredRooms()
    {
        string freeSql = @"UPDATE room r
                       SET r.isFree = 1
                       WHERE EXISTS (
                           SELECT 1 FROM rent rt WHERE rt.roomNumber = r.number
                       )
                       AND NOT EXISTS (
                           SELECT 1 FROM rent rt 
                           WHERE rt.roomNumber = r.number 
                           AND rt.endDate >= NOW()
                       );";

        
        string occupySql = @"UPDATE room r
                       SET r.isFree = 0
                       WHERE EXISTS (
                           SELECT 1 FROM rent rt 
                           WHERE rt.roomNumber = r.number 
                           AND rt.startDate <= NOW()
                           AND rt.endDate >= NOW()
                       );";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using (var mc1 = new MySqlCommand(freeSql, connection))
                mc1.ExecuteNonQuery();
            using (var mc2 = new MySqlCommand(occupySql, connection))
                mc2.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
}
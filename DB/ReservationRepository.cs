using System;
using System.Collections.Generic;
using Kursach.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Kursach.DB;

public class ReservationRepository
{
    private readonly string _connectionString;

    public ReservationRepository(IOptions<DBConnection> connect)
    {
        _connectionString = connect.Value.ConnectionString;
    }

    public List<Reservation> GetReservationList()
    {
        List<Reservation> data = new List<Reservation>();
        string sql = @"SELECT r.*, c.lastname 
                       FROM reservation r 
                       LEFT JOIN client c ON r.clientId = c.id;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            using var dr = mc.ExecuteReader();
            while (dr.Read())
            {
                data.Add(new Reservation
                {
                    Id = dr.GetInt32("id"),
                    RoomNumber = dr.GetInt32("rommNumber"),
                    ClientId = dr.GetInt32("clientId"),
                    ReservationDate = dr.GetDateTime("reservationDate"),
                    ClientLastName = dr.GetString("lastname")
                });
            }
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        return data;
    }

    public void DeleteReservationByRoom(int roomNumber)
    {
        string sql = "DELETE FROM reservation WHERE rommNumber = @roomNumber;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@roomNumber", roomNumber);
            mc.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }

    public void AddReservation(Reservation reservation)
    {
        string sql = @"INSERT INTO reservation (rommNumber, clientId, reservationDate) 
                       VALUES (@RoomNumber, @ClientId, @ReservationDate);";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@RoomNumber", reservation.RoomNumber);
            mc.Parameters.AddWithValue("@ClientId", reservation.ClientId);
            mc.Parameters.AddWithValue("@ReservationDate", reservation.ReservationDate);
            mc.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
}
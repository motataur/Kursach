using System;
using System.Collections.Generic;
using Kursach.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Kursach.DB;

public class ClientRepository
{
    private readonly string _connectionString;

    public ClientRepository(IOptions<DBConnection> connect)
    {
        _connectionString = connect.Value.ConnectionString;
    }

    public List<Client> GetClientList()
    {
        List<Client> data = new List<Client>();
        string sql = "select * from client c;";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            using var dr = mc.ExecuteReader();
            while (dr.Read())
            {
                data.Add(new Client
                {
                    Id = dr.GetInt32("id"),
                    Name = dr.GetString("name"),
                    LastName = dr.GetString("lastname"),
                    Email = dr.GetString("email"),
                    Phone = dr.GetString("phone"),
                    PassportSeries = dr.GetString("passportseries"),
                    PassportNumber = dr.GetString("passportnumber"),
                });
            }
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        return data;
    }

    public void AddClient(Client client)
    {
        string sql = @"INSERT INTO client (name, lastname, email, phone, passportseries, passportnumber) 
                       VALUES (@Name, @LastName, @Email, @Phone, @PassportSeries, @PassportNumber);";
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@Name", client.Name);
            mc.Parameters.AddWithValue("@LastName", client.LastName);
            mc.Parameters.AddWithValue("@Email", client.Email ?? string.Empty);
            mc.Parameters.AddWithValue("@Phone", client.Phone ?? string.Empty);
            mc.Parameters.AddWithValue("@PassportSeries", client.PassportSeries ?? string.Empty);
            mc.Parameters.AddWithValue("@PassportNumber", client.PassportNumber ?? string.Empty);
            mc.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
}
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data;

public static class BadDb{
    public static string ConnectionString { get; set; } = string.Empty;

    public static int InsertOrder(int id, string customer, string product, int qty, decimal price)
    {
        const string sql = @"
            INSERT INTO Orders(Id, Customer, Product, Qty, Price)
            VALUES (@Id, @Customer, @Product, @Qty, @Price)";

        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Customer", customer);
        cmd.Parameters.AddWithValue("@Product", product);
        cmd.Parameters.AddWithValue("@Qty", qty);
        cmd.Parameters.AddWithValue("@Price", price);

        conn.Open();
        return cmd.ExecuteNonQuery();
    }
}

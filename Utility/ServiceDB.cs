using ManageOrders.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;

namespace ManageOrders.Utility
{
    public class ServiceDB
    {
        private readonly string _connectionString;

        public ServiceDB(string pathToDB)
        {
            if (!File.Exists(pathToDB))
            {
                CreateDB.Create(pathToDB);
            }
            _connectionString = $"Data Source={pathToDB}";
        }

        public List<OrderModel> GetOrders()
        {
            List<OrderModel> orders = new List<OrderModel>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string request = @"SELECT
                                    id_order
                                    , name_client
                                    , name_executor
                                    , pickup_address
                                    , delivery_address
                                    , pickup_time
                                    , status
                                    , cancel_reason
                                    FROM
                                    Orders
                                    ";
                using var command = new SqliteCommand(request, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderModel
                    {
                        IdOrder = reader.GetInt32(0),
                        NameClient = reader.GetString(1),
                        NameExecutor = reader.GetString(2),
                        PickupAddress = reader.GetString(3),
                        DeliveryAddress = reader.GetString(4),
                        PickupTime = reader.GetDateTime(5),
                        Status = reader.GetString(6),
                        CancelReason = reader.IsDBNull(7) ? null : reader.GetString(7),
                    });
                }

                connection.Close();
            }
            return orders;
        }

        public void CreateOrder(OrderModel order)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string request = @"INSERT INTO Orders
                                    (name_client
                                    ,name_executor
                                    ,pickup_address
                                    ,delivery_address
                                    ,pickup_time
                                    ,status
                                    ,cancel_reason)
                                    VALUES
                                    (
                                    @name_client,
                                    ,@name_executor,
                                    ,@pickup_address,
                                    ,@delivery_address,
                                    ,@pickup_time,
                                    ,@status,
                                    ,@cancel_reason)
                                    ";
                using (var command = new SqliteCommand(request, connection))
                {
                    command.Parameters.Clear();
                    
                    command.Parameters.AddWithValue("@name_client", order.NameClient);
                    command.Parameters.AddWithValue("@name_executor", order.NameExecutor);
                    command.Parameters.AddWithValue("@pickup_address", order.PickupAddress);
                    command.Parameters.AddWithValue("@delivery_address", order.DeliveryAddress);
                    command.Parameters.AddWithValue("@pickup_time", order.PickupTime);
                    command.Parameters.AddWithValue("@status", order.Status);
                    command.Parameters.AddWithValue("@cancel_reason", order.CancelReason);

                    command.ExecuteNonQuery();
                };

                connection.Close();
            }
        }

        public OrderModel EditOrder(OrderModel order)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string request = @"UPDATE Orders
                                    SET 
                                    name_client = @name_client,
                                    name_executor = @name_executor,
                                    pickup_address = @pickup_address,
                                    delivery_address = @delivery_address,
                                    pickup_time = @pickup_time,
                                    status = @status,
                                    cancel_reason = @cancel_reason,
                                    WHERE id_order = @id_order
                                    ";
                using (var command = new SqliteCommand(request, connection))
                {
                    command.Parameters.Clear();
                    
                    command.Parameters.AddWithValue("@name_client", order.NameClient);
                    command.Parameters.AddWithValue("@name_executor", order.NameExecutor);
                    command.Parameters.AddWithValue("@pickup_address", order.PickupAddress);
                    command.Parameters.AddWithValue("@delivery_address", order.DeliveryAddress);
                    command.Parameters.AddWithValue("@pickup_time", order.PickupTime);
                    command.Parameters.AddWithValue("@status", order.Status);
                    command.Parameters.AddWithValue("@cancel_reason", order.CancelReason);
                    command.Parameters.AddWithValue("@id_order", order.IdOrder);

                    command.ExecuteNonQuery();
                };

                connection.Close();
            }
            return order;
        }

        public void DeleteOrder(int idOrder)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string request = @"DELETE FROM Orders
                                    WHERE 
                                    id_order = @id_order";
                using (var command = new SqliteCommand(request, connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id_order", idOrder);
                    command.ExecuteNonQuery();
                };

                connection.Close();
            }
        }

    }
}

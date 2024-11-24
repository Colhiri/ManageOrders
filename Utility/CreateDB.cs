using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace ManageOrders.Utility
{
    public static class CreateDB
    {
        public static void Create(string pathToDB)
        {
            // Создаем подключение к базе данных
            using (SqliteConnection connection = new SqliteConnection($"Data Source={pathToDB}"))
            {
                connection.Open();

                string createTableQuery = @"
                CREATE TABLE Orders (
                    id_order TEXT NOT NULL PRIMARY KEY,
                    name_client TEXT NOT NULL,
                    name_executor TEXT NOT NULL,
                    pickup_address TEXT NOT NULL,
                    delivery_address TEXT NOT NULL,
                    pickup_time DATETIME NOT NULL,
                    status TEXT NOT NULL,
                    cancel_reason TEXT
                )";

                // Выполняем запрос
                using (SqliteCommand command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();

                CreateData(pathToDB);
            }
        }

        public static void CreateData(string pathToDB)
        {
            if (!File.Exists(pathToDB))
            {
                Create(pathToDB);
            }

            string request = $@"INSERT INTO Orders (id_order, name_client, name_executor, pickup_address, delivery_address, pickup_time, status, cancel_reason) 
                                VALUES 
                                ('{Guid.NewGuid()}', 'Ivan Petrov', 'Sergey Ivanov', '123 Main St', '456 Oak St', '2024-11-24 09:00:00', 'Новая', NULL),
                                ('{Guid.NewGuid()}', 'Anna Ivanova', 'Dmitry Smirnov', '78 Elm St', '34 Pine St', '2024-11-25 12:00:00', 'Передано на выполнение', NULL),
                                ('{Guid.NewGuid()}', 'Alexey Sidorov', 'Olga Kuznetsova', '23 Birch St', '90 Cedar St', '2024-11-26 15:00:00', 'Выполнено', NULL),
                                ('{Guid.NewGuid()}', 'Maria Volkova', 'Ivan Sokolov', '345 Maple St', '567 Willow St', '2024-11-24 14:30:00', 'Отменена', 'Клиент отказался'),
                                ('{Guid.NewGuid()}', 'Sergey Popov', 'Natalia Andreeva', '11 Spruce St', '77 Redwood St', '2024-11-27 10:00:00', 'Новая', NULL),
                                ('{Guid.NewGuid()}', 'Dmitry Orlov', 'Alexey Morozov', '101 Aspen St', '202 Chestnut St', '2024-11-28 11:00:00', 'Передано на выполнение', NULL),
                                ('{Guid.NewGuid()}', 'Natalia Petrova', 'Tatiana Sidorova', '88 Fir St', '150 Juniper St', '2024-11-29 13:00:00', 'Выполнено', NULL),
                                ('{Guid.NewGuid()}', 'Vladimir Smirnov', 'Ekaterina Volkova', '400 Linden St', '680 Magnolia St', '2024-11-30 14:00:00', 'Отменена', 'Проблемы с адресом'),
                                ('{Guid.NewGuid()}', 'Olga Ivanova', 'Alexander Kuzmin', '32 Poplar St', '65 Sycamore St', '2024-12-01 15:30:00', 'Новая', NULL),
                                ('{Guid.NewGuid()}', 'Alexey Nikitin', 'Maria Petrova', '55 Hawthorn St', '99 Palm St', '2024-12-02 16:00:00', 'Выполнено', NULL);
                                ";

            // Создаем подключение к базе данных
            using (SqliteConnection connection = new SqliteConnection($"Data Source={pathToDB}"))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(request, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}

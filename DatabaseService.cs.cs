using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListApp
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            var options = new SQLiteConnectionString(dbPath, true);
            _database = new SQLiteAsyncConnection(options);

            _database.CreateTableAsync<TaskItem>().Wait();
        }

        public Task<List<TaskItem>> GetTasksAsync() => _database.Table<TaskItem>().ToListAsync();

        public Task<int> SaveTaskAsync(TaskItem task) => _database.InsertAsync(task);

        public Task<int> DeleteTaskAsync(TaskItem task) => _database.DeleteAsync(task);
    }
}

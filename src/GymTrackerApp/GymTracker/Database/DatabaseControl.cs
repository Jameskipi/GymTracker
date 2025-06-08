using GymTracker.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Database
{
    internal class UsersDatabase
    {
        private const string db_name = "AppDatabase.db";
        private readonly SQLiteAsyncConnection _connection;

        public UsersDatabase()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, db_name));
            _connection.CreateTableAsync<Users>().Wait();
        }
        
        public async Task<List<Users>> GetUsersAsync()
        {
            return await _connection.Table<Users>().ToListAsync();
        }

        public async Task<Users> GetUserByLoginAsync(string username)
        {
            return await _connection.Table<Users>().Where(x => x.Username == username).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(Users user)
        {
            await _connection.InsertAsync(user);
        }
    }
}

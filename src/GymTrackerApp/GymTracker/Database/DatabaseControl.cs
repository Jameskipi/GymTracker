using GymTracker.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Database
{
    internal class AppDatabase
    {
        private const string db_name = "AppDatabase.db";
        private readonly SQLiteAsyncConnection _connection;

        public AppDatabase()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, db_name));
            _connection.CreateTableAsync<Users>().Wait();
            _connection.CreateTableAsync<Workouts>().Wait();
            _connection.CreateTableAsync<Exercises>().Wait();
            _connection.CreateTableAsync<ExerciseTemplates>().Wait();
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

        public async Task<List<Workouts>> GetWorkoutsAsync(int id)
        {
            return await _connection.Table<Workouts>().Where(x => x.User_ID == id).ToListAsync();
        }

        public async Task CreateWorkoutAsync(Workouts workout)
        {
            await _connection.InsertAsync(workout);
        }

        public async Task DeleteAllUsers()
        {
           await _connection.ExecuteAsync("DELETE FROM Users");
        }

        public async Task DeleteAllWorkouts()
        {
            await _connection.ExecuteAsync("DELETE FROM Workouts");
        }
        public async Task DeleteAllExercises()
        {
            await _connection.ExecuteAsync("DELETE FROM Exercises");
        }
        public async Task DeleteAllExerciseTemplates()
        {
            await _connection.ExecuteAsync("DELETE FROM ExerciseTemplates");
        }
    }
}

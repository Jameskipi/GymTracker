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

        public async Task RemoveWorkoutAsync(int id)
        {
            var workout = await _connection.Table<Workouts>().Where(x => x.ID == id).FirstOrDefaultAsync();
            await _connection.DeleteAsync(workout);
        }

        public async Task<List<ExerciseTemplates>> GetTemplatesAsync()
        {
            return await _connection.Table<ExerciseTemplates>().ToListAsync();
        }

        public async Task CreateTemplateAsync(ExerciseTemplates templates)
        {
            await _connection.InsertAsync(templates);
        }

        public async Task<List<Exercises>> GetExercisesAsync(int id)
        {
            return await _connection.Table<Exercises>().Where(x => x.Workout_ID == id).ToListAsync();
        }

        public async Task CreateExerciseAsync(Exercises exercise)
        {
            await _connection.InsertAsync(exercise);
        }

        public async Task RaiseExerciseAsync(int id)
        {
            var oldExercise = await _connection.Table<Exercises>().Where(x => x.ID == id).FirstOrDefaultAsync();

            if (oldExercise.ExerciseIsDone == 1)
            {
                return;
            }

            var exercise = new Exercises
            {
                ID = oldExercise.ID,
                Workout_ID = oldExercise.Workout_ID,
                ExerciseName = oldExercise.ExerciseName,
                ExerciseMaxNum = oldExercise.ExerciseMaxNum,
                ExerciseCurrNum = oldExercise.ExerciseCurrNum + 1,
                ExerciseIsDone = (oldExercise.ExerciseCurrNum + 1 >= oldExercise.ExerciseMaxNum) ? 1 : 0 
            };

            await _connection.UpdateAsync(exercise);
        }

        public async Task RemoveExerciseAsync(int id)
        {
            var exercise = await _connection.Table<Exercises>().Where(x => x.ID == id).FirstOrDefaultAsync();
            await _connection.DeleteAsync(exercise);
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

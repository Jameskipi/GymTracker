using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Database
{
    public class Exercises
    {
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }
        public int Workout_ID { get; set; }
        public string ExerciseName { get; set; }
        public int ExerciseMaxNum { get; set; }
        public int ExerciseCurrNum { get; set; }
        public int ExerciseIsDone { get; set; }
    }
}

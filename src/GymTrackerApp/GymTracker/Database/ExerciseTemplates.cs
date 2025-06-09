using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Database
{
    public class ExerciseTemplates
    {
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }
        public int User_ID { get; set; }
        public string ExerciseName { get; set; }
        public int ExerciseMaxNum { get; set; }
    }
}

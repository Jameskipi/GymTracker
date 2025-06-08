using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Database
{
    public class Workouts
    {
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }
        public int User_ID { get; set; }
        public DateTime WorkoutDate { get; set; }
        public string WorkoutName { get; set; } 
    }
}


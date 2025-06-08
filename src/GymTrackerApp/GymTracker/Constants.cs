using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker
{
    class Constants
    {
        public static Database.AppDatabase appDatabase = new Database.AppDatabase();
        public static int currentUserID = -1;
        public static int currentWorkoutID = -1;
    }
}

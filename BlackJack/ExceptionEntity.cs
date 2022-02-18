using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    // here we will map a class to a database and we will be able to read from the database. An entity is a word used commonly used to refer to a database object like we are creating
    public class ExceptionEntity
    {
        // this entity maps exactly to our database table
        public int Id { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

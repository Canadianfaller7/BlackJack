using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Casino;
using Casino.BlackJack;

namespace BlackJack
{
    class Program // this is our main class called Program and this is the entrance point of a console app
    {
        static void Main(string[] args) // this is our main method(function inside the class and when called will do the stuff we put inside it)
        {
            

            Console.WriteLine("Welcome to the Grand Hotel and Casino! Let's start by telling me your name.\n"); // welcoming someone to black jack and asking their name
            string playerName = Console.ReadLine(); // save user input to a var

            // this code right here is a command we can call that will print out a log of exceptions if we put the name as admin
            if (playerName.ToLower() == "admin")
            {
                List<ExceptionEntity> Exceptions = ReadExceptions(); // we make a list of the entity we are pulling from the db
                foreach (var exception in Exceptions) // then we iterate through the list and then print the exceptions that are inside our db table to the screen.
                {
                    Console.Write(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp + " | ");
                    Console.WriteLine();
                }
                Console.Read();
                return;
            }

            // all this from bool to the if inside the while is to do an exception if the user adds string or decimal and to tell them to just add numbers
            bool validAnswer = false; // set this to false to run error messages
            int bank = 0;
            while (!validAnswer) // while validAnswer is true
            {
                Console.WriteLine("\nAnd how much money did you bring today?\n"); // print this
                validAnswer = int.TryParse(Console.ReadLine(), out bank); // try running this code which will get users answer and try running it
                if (!validAnswer) // if user put a string or decimal in the code then print statement below else continue on with code
                {
                    Console.WriteLine("\nPlease enter digits only, No decimals");
                }
            }
            Console.WriteLine($"\nHello {playerName}. Would you like to join a game of 21 right now?\n"); // ask user if they want to play black jack
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yeah" || answer == "ya" || answer == "sure" || answer == "y") // acceptable answers from user
            {
                Player player = new Player(playerName, bank); // if user wants to play create a new Player with their name and how much money they have
                player.Id = Guid.NewGuid();
                using (StreamWriter file = new StreamWriter(@"D:\Logs\log.txt", true)) // this will save the cards dealt to us in a new file
                {
                    file.WriteLine(player.Id);
                }
                Game game = new BlackJackGame(); // start a new black jack game
                game += player; // add the player to the game
                player.isActivelyPlaying = true; // this is true if the player said they want to play
                while (player.isActivelyPlaying && player.Balance > 0) // this is saying while the player is wanting to play and they don't have a balance of 0 then
                {
                    try
                    {
                        game.Play(); // keep the game playing
                    }
                    catch (FraudException ex)
                    {
                        Console.WriteLine(ex.Message);
                        UpdateDBWithException(ex); // this is calling our method below and then we pass in ex as an exception object to this method
                        Console.ReadLine();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nAn error occured. Please contact your System Administrator");
                        UpdateDBWithException(ex); // same as above
                        Console.ReadLine();
                        return;
                    }
                }
                game -= player; // else remove the player from the game
                Console.WriteLine("\nThank you for playing!"); // print this if the user isn't active anymore
            }
            Console.WriteLine("\nFeel free to look around the casino. Bye for now."); // this will print if we don't get an acceptable answer or user doesn't want to play anymore
            Console.ReadLine();
        }

        // here we will use ado.net which is an abstraction layer for communicating to a data base. We will use it to write actual sql queres inside our c# code and execute them wihtin the database we are targeting and return results to our actual application
        private static void UpdateDBWithException(Exception ex)  // this is a private static method that will take a type exception called ex and the code inside is what we will use to log any exceptions made and update our db that we made
        {
            // ( this is ado.net) this is a connection string that contains info about the database instance we are trying to connect to, like usr name, password, location, where it is and how to access it. We always need a connection string to connect to a database in this manner
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=BlackJackGame;Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                       TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            
            // to make a string read just as a straight string we add the @ symbole infront of the string, so like above we have a \ which is an escape key in C# so we use the @ to tell the system its an actual \ and not an escape key
            // this is a query string, our actual sql query we are writing
            string queryString = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) VALUES
                                 (@ExceptionType, @ExceptionMessage, @TimeStamp)"; // we are saying insert into table Exceptions the column names(exceptionType, exceptionMessage, timeStamp) and 
                                  // then we give some values, but we don't actual add the values yet, here we are doing parameterized queres and putting in place holders and then ado.net will help create
                                  // some parameters that will then map in exactly the exact value once the query is actually ran. This helps protect from sql injections
            
            // using statements are ways of controlling unmanaged code or unmanaged resources. Here we will open up a sql connection which is connecting to an external database and then we will wrap it
            // in {} so once we exit the last } it will automatically shut down this resource so memory is freed up and we don't have memory shortages or slow downs in our programs. This is what "using" is mainly used for.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // inside the {} is where we will actually deal with our external database
                SqlCommand command = new SqlCommand(queryString, connection); // this is a sql command and we will pass our query string as well as our connection here and this command takes in the command as well as the connection
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar); // here we are adding the data type to the command above inside which will be the data type inside our sql table
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);

                // here are the actual parameters that we are adding to the command. These will be the values of our data types from above.

                // we are getting the value of the parameter here which is object ex and grab its type which is the type of data type we are workign with and then we will return it as string
                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString(); // here we specify which parameter we are talking about because paramaters are a list. Now we will specify which collection item and we do that with the [](array)
                command.Parameters["@ExceptionMessage"].Value = ex.Message; // this is already a string and so it will return the message just fine
                command.Parameters["@TimeStamp"].Value = DateTime.Now; // now this will do a time stamp, give the current date and time when the query is ran and the exception is put in our db

                // here is how we send this info above to our database
                connection.Open();
                command.ExecuteNonQuery(); // execute the command. This is like an insert statement from sql
                connection.Close(); // then we close it.
            }
        }

        // this is our method to make our admin command work
        private static List<ExceptionEntity> ReadExceptions() // will return a list of exceptions entities and will call the database, query the db and get back all the exceptions and then display them
        {
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=BlackJackGame;Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                       TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"; // getting connection string like above

            string queryString = @"Select Id, ExceptionType, ExceptionMessage, TimeStamp From Exceptions"; // this is the query string and we are selecting all columns from our exceptions table

            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>(); // create an empty list to return the info later on

            using (SqlConnection connection = new SqlConnection(connectionString)) // do the same as above with the using connection so everything gets closed out when we are done
            {
                SqlCommand command = new SqlCommand(queryString, connection); // create new sql command object and pass in our query string and connection

                connection.Open(); // here we open up our connection
                SqlDataReader reader = command.ExecuteReader(); // this is a sql data reader option and while this is open we will then do the stuff inside the while loop

                while (reader.Read()) // this will loop through each record we are getting back and for each record we want to create a new object into an exception entity object
                {
                    ExceptionEntity exception = new ExceptionEntity(); // this is our exception object
                    exception.Id = Convert.ToInt32(reader["Id"]); // now we are mapping what we get back into our exception object, so this will be the Id number and we have to convert it to C#
                    exception.ExceptionType = reader["ExceptionType"].ToString(); // this will give back the exception type, fraud or just other exception
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString(); // give back the message that was printed out
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"]); // give back the timestamp the exception was made and logged to our db

                    Exceptions.Add(exception); // we then add all these object to our empy Exceptions list we made above
                }
                connection.Close(); // then close our connection.
            }
            return Exceptions; // and return our exceptions because this returns our exception entities.
            // when mapping using ado.net we need to make sure that we don't have typos when we are mapping our column names is VS doesn't know if these are put in correct or not so just be careful when mapping these.
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    /* here we made a class called fraud exception and it will inherit from the exception class*/
    public class FraudException : Exception
    {
        // we then make 2 constructors with one overloading the other and passing in a message just like we can with a regular exception and then we will call the construct in our blackJackGame and Program class
        public FraudException()
            :base() { }
        public FraudException(string message)
            :base(message) { }

        
    }
}

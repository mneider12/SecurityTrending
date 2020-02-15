using System.Collections.Generic;

namespace Model
{
    public class Account
    {
        public Account()
        {
            Positions = new Dictionary<string, Position>();
        }

        public Dictionary<string, Position> Positions
        {
            get;
        }
    }
}

using static Core.TransactionEnums;

namespace Model
{
    /// <summary>
    /// represents an interest in a single security
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Symbol representing the position
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// Quantity in the position
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// class of the position
        /// </summary>
        public TransactionClass Class { get; set; }
        /// <summary>
        /// compare two positions for equality
        /// </summary>
        /// <param name="obj">position to compare</param>
        /// <returns>whether two positions are equal</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Position);
        }
        /// <summary>
        /// get a hash of the properties in the position
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return Symbol.GetHashCode() + Quantity.GetHashCode() + Class.GetHashCode();
        }
        #region private helper methods

        /// <summary>
        /// check for equality of two positions
        /// </summary>
        /// <param name="otherPosition">other position</param>
        /// <returns>whether two positions are equal</returns>
        private bool Equals(Position otherPosition)
        {
            return otherPosition != null && Symbol == otherPosition.Symbol && Quantity == otherPosition.Quantity && Class == otherPosition.Class;
        }

        #endregion
    }
}

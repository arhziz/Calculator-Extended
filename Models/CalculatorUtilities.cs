namespace Calculator_Extended
{
    /// <summary>
    /// this is a class that has all the enums
    /// </summary>
    public class CalculatorUtilities
    {
        /// <summary>
        /// current calculator operation
        /// </summary>
        public enum CalculatorFunction
        {
            Plus,
            Minus,
            Multiply,
            Divide
        }

        /// <summary>
        /// current calculator state
        /// </summary>
        public enum CalculatorState
        {
            Default,
            FunctionPressed,
            CanSolve
        }
        /// <summary>
        /// the last action performed by the calculator
        /// </summary>
        public enum LastAction
        {
            NumberBtn,
            ArithmeticBtn,
            FunctionBtn,
            MemoryBtn,
            EqualsBtn
        }

    }
}

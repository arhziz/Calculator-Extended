namespace Calculator_Extended
{
    public class CalculatorUtilities
    {
        public enum CalculatorFunction
        {
            Plus,
            Minus,
            Multiply,
            Divide
        }

        public enum CalculatorState
        {
            Default,
            FunctionPressed,
            CanSolve
        }

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

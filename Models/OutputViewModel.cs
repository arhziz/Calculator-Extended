namespace Calculator_Extended
{
    public class OutputViewModel : BaseModel 
    {
        public OutputModel Data;

        public OutputViewModel()
        {

            Data = CalculatorStructure.Instance.GetOutput();
        }
    }

    
}

namespace Calculator_Extended
{
    /// <summary>
    /// Output view model for data context binding
    /// N.B This view model is not un use.
    /// </summary>
    public class OutputViewModel : BaseModel 
    {
        //holding the output model class
        public OutputModel Data;

        #region Constructor
        public OutputViewModel()
        {

            //getting the data for the output model
            Data = CalculatorStructure.Instance.GetOutput();
        }
        #endregion

    }

    
}

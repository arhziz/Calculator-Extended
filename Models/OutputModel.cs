using System.Globalization;
namespace Calculator_Extended
{
    /// <summary>
    /// Just a simple output model class for ui binding
    /// </summary>
    public class OutputModel : BaseModel
    {
        public string topOutputBlock { get; set; }
        public string MainOutputBlock { get; set; }

        // Gets a NumberFormatInfo associated with the en-US culture.
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        public OutputModel(string topBlock, decimal mainBlock)
        {
           

            //update the topOutput block and the main output block from the constructor
            this.topOutputBlock = topBlock;
            nfi.NumberDecimalDigits = getTheDecimalPlaces(mainBlock);
            //if mainBlock has decimal
            this.MainOutputBlock = mainBlock.ToString(mainBlock % 1 == 0 ? "N0" : "N", nfi);
        }

        private int getTheDecimalPlaces(decimal theMainBlock)
        {
            //convert the number into string
            string number = theMainBlock.ToString();
            return number.Substring(number.IndexOf(".") + 1).Length;
        }

    }
}

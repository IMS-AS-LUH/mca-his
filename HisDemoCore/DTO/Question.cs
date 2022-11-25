namespace HisDemo.DTO
{
    /// <summary>
    /// A Question answered during data entry.
    /// </summary>
    public class Question
    {
        public string QID { get; set; }
        public string QuestionText { get; set; }
        public string ShortText { get; set; } = null;
        public QuestionType QuestionType { get; set; } = QuestionType.Plaintext;
        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = int.MaxValue;
        public int DecimalPlaces { get; set; } = 0;

        public bool Required { get; set; } = false;
        public bool MustAnswerYes { get; set; } = false;

        public string Default { get; set; } = null;

        public bool AlwaysReadOnly { get; set; } = false;

        public bool NumberAcceptZeroAsNull { get; set; } = false;


        public override string ToString()
        {
            return $"QID={this.QID ?? "*NULL*"};Type={QuestionType}";
        }

    }
}

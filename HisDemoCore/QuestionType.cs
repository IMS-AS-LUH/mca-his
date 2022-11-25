namespace HisDemo
{
    public enum QuestionType
    {
        /// <summary>
        /// Basic answer (plaintext). Minimum and Maximum defnie min/max characters.
        /// </summary>
        Plaintext,
        /// <summary>
        /// Simple boolean answer.
        /// </summary>
        YesNo,
        /// <summary>
        /// Enter a Date (without time). Required... fields apply.
        /// </summary>
        Date,
        /// <summary>
        /// A numeric (integer) entry, must be between Minimum and Maximum.
        /// If DecimalPlaces > 0, this is a decimal number.
        /// </summary>
        Number,
        /// <summary>
        /// Select choices. Minimum and Maximum define how many you must/may select.
        /// </summary>
        Choice,
        /// <summary>
        /// Stores only B32ID part of Specimen MCA-Barcode-ID. Min/Max does not apply for now.
        /// </summary>
        B32IDOfSpecimen,
        /// <summary>
        /// Plaintext validated against E-Mail regex. Min/Max does not apply.
        /// </summary>
        EmailAddress,
        /// <summary>
        /// Plaintext validated against Telehone regex. Min/Max does not apply.
        /// </summary>
        PhoneNumber,
        /// <summary>
        /// Boolean with explicit unknown option.
        /// </summary>
        YesNoUnknown,
        /// <summary>
        /// Boolean with explicit living alone option.
        /// </summary>
        YesNoAlone,
        /// <summary>
        /// No input, only text, also no QID here!
        /// </summary>
        JustText,
        /// <summary>
        /// Male/Female/Diverse
        /// </summary>
        Gender,
        /// <summary>
        /// Enter a Time (without date). Required... fields apply.
        /// </summary>
        Time,
        /// <summary>
        /// Postleitzahl (ZIP Code)
        /// </summary>
        PLZ,
        /// <summary>
        /// Values for not, sometimes, often
        /// </summary>
        NotSometimesOften,
    }
}

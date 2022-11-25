using System.Collections.Generic;

namespace HisDemo.DTO
{
    /// <summary>
    /// Contains Questions answered by patiend during data entry.
    /// </summary>
    public class Questionnaire
    {
        public string Title { get; set; } = "Unbenannter Fragebogen";
        public List<Question> Questions { get; set; } = new List<Question>();

        public bool ShowToPatient { get; set; } = true;

        /// <summary>
        /// Show in printed info and also include in QR Code
        /// </summary>
        public bool ShowInPrint { get; set; } = false;
    }
}

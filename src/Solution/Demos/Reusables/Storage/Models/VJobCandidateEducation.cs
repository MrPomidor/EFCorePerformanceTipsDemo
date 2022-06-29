using System;
using System.Collections.Generic;

namespace Reusables.Storage
{
    /// <summary>
    /// Displays the content from each education related element in the xml column Resume in the HumanResources.JobCandidate table. The content has been localized into French, Simplified Chinese and Thai. Some data may not display correctly unless supplemental language support is installed.
    /// </summary>
    public partial class VJobCandidateEducation
    {
        public int JobCandidateId { get; set; }
        public string EduLevel { get; set; }
        public DateTime? EduStartDate { get; set; }
        public DateTime? EduEndDate { get; set; }
        public string EduDegree { get; set; }
        public string EduMajor { get; set; }
        public string EduMinor { get; set; }
        public string EduGpa { get; set; }
        public string EduGpascale { get; set; }
        public string EduSchool { get; set; }
        public string EduLocCountryRegion { get; set; }
        public string EduLocState { get; set; }
        public string EduLocCity { get; set; }
    }
}

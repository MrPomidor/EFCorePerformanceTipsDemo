using System;
using System.Collections.Generic;

namespace Reusables.Storage
{
    /// <summary>
    /// Displays the content from each employement history related element in the xml column Resume in the HumanResources.JobCandidate table. The content has been localized into French, Simplified Chinese and Thai. Some data may not display correctly unless supplemental language support is installed.
    /// </summary>
    public partial class VJobCandidateEmployment
    {
        public int JobCandidateId { get; set; }
        public DateTime? EmpStartDate { get; set; }
        public DateTime? EmpEndDate { get; set; }
        public string EmpOrgName { get; set; }
        public string EmpJobTitle { get; set; }
        public string EmpResponsibility { get; set; }
        public string EmpFunctionCategory { get; set; }
        public string EmpIndustryCategory { get; set; }
        public string EmpLocCountryRegion { get; set; }
        public string EmpLocState { get; set; }
        public string EmpLocCity { get; set; }
    }
}

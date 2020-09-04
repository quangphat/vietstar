using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Commons
{
    public class Enums
    {
        public enum ProfileStatus
        {
            Draft = 0,
            Input = 1,
            Deny = 2,
            Expertise = 3,
            Additional = 4,
            Release = 5,
            Compared = 6,
            Cancel = 7,
            PCB = 8,
            New = 9,
            Processing = 10,
            Accept = 12,
            Finish = 14,
        }
        public enum ProfileType
        {
            Common = 1,
            Courier = 2,
            MCredit = 3,
            Company = 4,
            RevokeDebt = 5,
            Product = 10
        }
        public enum NoteType
        {
            Common = 1,
            Courier = 2,
            Company = 3,
            MCreditTemp = 4,
            RevokeDebt = 5,
            CheckDup = 6
        }
        public enum CheckDupPartnerStatus
        {
            NotCheck = 0,
            MatchCondition = 1,
            NotMatch = 2
        }
        public enum CheckDupCICStatus
        {
            NotDebt = 0,
            Warning = 1,
            Debt = 2
        }

        public enum MCTableType
        {
            MCreditCity = 1,
            MCreditLoanPeriod = 2,
            MCreditlocations = 3,
            MCreditProduct = 4,
            MCreditProfileStatus = 5
        }

        public enum MCreditProfileStatus
        {
            Draft = 0,
            SentToMc = -1,
            Submit = -3,
            Addinional = -4,
            Updated = -5
        }
    }
}

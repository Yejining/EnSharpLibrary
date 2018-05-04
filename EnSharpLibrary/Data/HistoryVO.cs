using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnSharpLibrary.Data
{
    class HistoryVO
    {
        private DateTime dateBorrowed;
        private DateTime dateDeadlineForReturn;
        private int numberOfRenew;

        public HistoryVO(DateTime dateBorrowed, DateTime dateDeadlineForReturn, int numberOfRenew)
        {
            this.dateBorrowed = dateBorrowed;
            this.dateDeadlineForReturn = dateDeadlineForReturn;
            this.numberOfRenew = numberOfRenew;
        }

        public DateTime DateBorrowed
        {
            get { return dateBorrowed; }
        }

        public DateTime DateDeadlineForReturn
        {
            get { return dateDeadlineForReturn; }
            set { dateDeadlineForReturn = value; }
        }
            
        public int NumberOfRenew
        {
            get { return numberOfRenew; }
            set { numberOfRenew = value; }
        }
    }
}

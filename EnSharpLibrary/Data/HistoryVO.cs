using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnSharpLibrary.Data
{
    class HistoryVO
    {
        private int memberID;
        private float bookID;
        private DateTime dateBorrowed;
        private DateTime dateReturn;
        private DateTime dateDeadlineForReturn;
        private int numberOfRenew;

        public HistoryVO(int memberID, float bookID, DateTime dateBorrowed)
        {
            this.memberID = memberID;
            this.bookID = bookID;
            this.dateBorrowed = dateBorrowed;
            dateDeadlineForReturn = dateBorrowed.AddDays(6);
            numberOfRenew = 0;
        }

        public void RenewBook()
        {
            dateReturn = DateTime.Now;
        }

        public bool IsBookOverdued()
        {
            if (DateTime.Now > dateDeadlineForReturn) return true;
            else return false;
        }

        public int MemberID
        {
            get { return memberID; }
        }

        public float BookID
        {
            get { return bookID; }
        }

        public DateTime DateBorrowed
        {
            get { return dateBorrowed; }
        }

        public DateTime DateReturn
        {
            get { return dateReturn; }
            set { dateReturn = value; }
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

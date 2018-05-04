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

        /// <summary>
        /// HistoryVO의 생성자입니다. 어느 도서의 대출일, 예상 반납일, 연장 횟수를 저장합니다.
        /// </summary>
        /// <param name="dateBorrowed">대출일</param>
        /// <param name="dateDeadlineForReturn">예상 반납일</param>
        /// <param name="numberOfRenew">연장 횟수</param>
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

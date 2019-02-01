using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//8107110610

namespace Payslip_Calculator
{
    internal class Program
    {
        public class Constants
        {
            //Social security and health insurance constants
            public const int insuranceBase = 3000;
            public const double pension = insuranceBase * 0.0658;
            public const double generalIllnessMaternity = insuranceBase * 0.014;
            public const double universalPension = insuranceBase * 0.022;
            public const double healthInsurance = insuranceBase * 0.032;
            public const double unemployment = insuranceBase * .004;
            public const double insuranceTax = pension + generalIllnessMaternity + universalPension + healthInsurance + unemployment;

            //vouchers
            public const int taxedVoucher = 90;
            public const int voucher = 60;

            //Taxes
            public const double tax = .1; //Taxes is 10% in BG

            // Base salary
            public const double baseSalary = 5349.72;

            //Bank Holidays
            public static DateTime[] holidays = new DateTime[]
            {
                new DateTime(2019,01,01),
                new DateTime(2019,03,04),
                new DateTime(2019,04,26),
                new DateTime(2019,04,29),
                new DateTime(2019,05,01),
                new DateTime(2019,05,06),
                new DateTime(2019,05,24),
                new DateTime(2019,09,06),
                new DateTime(2019,09,23),
                new DateTime(2019,12,24),
                new DateTime(2019,12,25),
                new DateTime(2019,12,26),

                //new DateTime(2019,01,01),
                //new DateTime(2019,03,04),
                //new DateTime(2019,04,26),
                //new DateTime(2019,04,29),
                //new DateTime(2019,05,01),
                //new DateTime(2019,05,06),
                //new DateTime(2019,05,24),
                //new DateTime(2019,09,06),
                //new DateTime(2019,09,23),
                //new DateTime(2019,12,24),
                //new DateTime(2019,12,25),
                //new DateTime(2019,12,26),
            };
        }

        static void Main(string[] args)
        {
            #region Asya Payslip Calculator
            
            //input variables
            int PTOdays, compensationDays, workedHolidays, years, officialWorkingDays, workedNonWorkingDays, actualWorkingDays;

            //calculated variables
            double OMZ, annuity, totalCompensation, annuityAmount, holidaysAmount, PTOamount, compensationAmount, taxedAmount, taxableIncome, netIncome, nonWorkingDaysAmount;

            #region Input & Number Check
            //Input and check whether numbers are entered and non-negative numbers
            years = CheckInteger("Enter years of service: ");
            officialWorkingDays = OfficialWorkingDays(Constants.holidays);
            workedHolidays = CheckInteger("Enter the number of worked holidays: ");
            workedNonWorkingDays = CheckInteger("Enter the number of worked non-working days: ");
            PTOdays = CheckInteger("Enter the number of used PTO days: ");
            compensationDays = CheckInteger("Enter the number of used compensation days: ");
            actualWorkingDays = officialWorkingDays - PTOdays - compensationDays;
            #endregion

            #region Calculations
            //Calculations left bracket
            annuity = years * .006;
            OMZ = Math.Round(Constants.baseSalary / (1 + annuity) / officialWorkingDays * actualWorkingDays, 2);
            annuityAmount = Math.Round(OMZ * annuity, 2);
            holidaysAmount = Math.Round(Constants.baseSalary / officialWorkingDays * 2 * workedHolidays, 2);
            nonWorkingDaysAmount = Math.Round(Constants.baseSalary / officialWorkingDays * 1.75 * workedNonWorkingDays, 2);
            PTOamount = Math.Round(Constants.baseSalary / officialWorkingDays * PTOdays, 2);
            compensationAmount = Math.Round(Constants.baseSalary / officialWorkingDays * compensationDays, 2);
            taxableIncome = Math.Round(Constants.baseSalary + holidaysAmount + nonWorkingDaysAmount + Constants.taxedVoucher - Constants.insuranceTax, 2);
            totalCompensation = Math.Round(Constants.baseSalary + holidaysAmount + nonWorkingDaysAmount + Constants.taxedVoucher + Constants.voucher, 2);

            //Calculations right bracket
            taxedAmount = Math.Round(taxableIncome * Constants.tax, 2);
            netIncome = Math.Round(totalCompensation - Constants.taxedVoucher - Constants.insuranceTax - Constants.voucher - taxedAmount, 2);
            #endregion

            #region Output
            //Output
            Console.WriteLine();
            Console.WriteLine("Left bracket\n************");
            Console.WriteLine("Base Salary:              " + OMZ);
            Console.WriteLine("Annuity:                  " + annuityAmount);
            Console.WriteLine("Taxed Voucher:            " + Constants.taxedVoucher);

            if (holidaysAmount != 0)
            {
                Console.WriteLine("Worked holidays:      " + workedHolidays + " - " + holidaysAmount);
            }

            if (nonWorkingDaysAmount != 0)
            {
                Console.WriteLine("Non-working days:     " + workedNonWorkingDays + " - " + nonWorkingDaysAmount);
            }

            if (PTOamount != 0)
            {
                Console.WriteLine("PTO:                  " + PTOdays + " - " + PTOamount);
            }

            if (compensationAmount != 0)
            {
                Console.WriteLine("Compensation Days:    " + compensationDays + " - " + compensationAmount);
            }

            Console.WriteLine("Voucher:                  " + Constants.voucher + "\n");

            Console.WriteLine("Taxable Income:           " + taxableIncome);
            Console.WriteLine("Total Disposable Incone:  " + totalCompensation + "\n");

            Console.WriteLine("Right bracket\n*************");
            Console.WriteLine("DDFL:                     " + taxedAmount);
            Console.WriteLine("On your bank account:     " + netIncome);
            Console.WriteLine("Insurance Tax:            " + Constants.insuranceTax);
            Console.WriteLine("Taxed Voucher:            " + Constants.taxedVoucher);
            Console.WriteLine("Voucher:                  " + Constants.voucher);
            #endregion

            #endregion

            Console.ReadKey();
        }

        static int CheckInteger(string request)
        {
            //Checks whether an input value is an actual integer
            int checkedInteger;
            string stringInteger;
            bool isNumeric;

            do
            {
                //Makes the custom request and stores input
                Console.Write(request);
                stringInteger = Console.ReadLine();

                //Parses input and returns true if it's numerical &  if it's a negative value. In both cases notify invalid number
                isNumeric = int.TryParse(stringInteger, out int n);
                if (!isNumeric || stringInteger.Contains("-"))
                {
                    Console.WriteLine("Not a valid number.");
                }

                //loop above while isNumeric is false or the integer string contains "-"
            } while (!isNumeric || stringInteger.Contains("-"));

            //If in the end the value is numerical, convert the string to an int and return
            checkedInteger = Convert.ToInt32(stringInteger);
            return checkedInteger;
        }

        public static int OfficialWorkingDays(params DateTime[] bankHolidays)
        {
            int month, businessDays, fullWeekCount; ;
            DateTime firstDate, lastDate = new DateTime();

            //Outputs text requesting the month and calls method checkinteger
            month = CheckInteger("Enter the month (1-12):");
            //Checks the month number and populates the dates of in the first and last date variables
            switch (month)
            {
                case 1:
                    firstDate = Convert.ToDateTime("01/01/2019");
                    lastDate = Convert.ToDateTime("31/01/2019");
                    break;
                case 2:
                    firstDate = Convert.ToDateTime("01/02/2019");
                    lastDate = Convert.ToDateTime("28/02/2019");
                    break;
                case 3:
                    firstDate = Convert.ToDateTime("01/03/2019");
                    lastDate = Convert.ToDateTime("31/03/2019");
                    break;
                case 4:
                    firstDate = Convert.ToDateTime("01/04/2019");
                    lastDate = Convert.ToDateTime("30/04/2019");
                    break;
                case 5:
                    firstDate = Convert.ToDateTime("01/05/2019");
                    lastDate = Convert.ToDateTime("31/05/2019");
                    break;
                case 6:
                    firstDate = Convert.ToDateTime("01/06/2019");
                    lastDate = Convert.ToDateTime("30/06/2019");
                    break;
                case 7:
                    firstDate = Convert.ToDateTime("01/07/2019");
                    lastDate = Convert.ToDateTime("31/07/2019");
                    break;
                case 8:
                    firstDate = Convert.ToDateTime("01/08/2019");
                    lastDate = Convert.ToDateTime("31/08/2019");
                    break;
                case 9:
                    firstDate = Convert.ToDateTime("01/09/2019");
                    lastDate = Convert.ToDateTime("30/09/2019");
                    break;
                case 10:
                    firstDate = Convert.ToDateTime("01/10/2019");
                    lastDate = Convert.ToDateTime("31/10/2019");
                    break;
                case 11:
                    firstDate = Convert.ToDateTime("01/11/2019");
                    lastDate = Convert.ToDateTime("30/11/2019");
                    break;
                case 12:
                    firstDate = Convert.ToDateTime("01/12/2019");
                    lastDate = Convert.ToDateTime("31/12/2019");
                    break;
                default:
                    firstDate = DateTime.Now;
                    break;
            }

            TimeSpan span = lastDate - firstDate;
            businessDays = span.Days + 1;
            fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDate.DayOfWeek == DayOfWeek.Sunday
                    ? 7 : (int)firstDate.DayOfWeek;
                int lastDayOfWeek = lastDate.DayOfWeek == DayOfWeek.Sunday
                    ? 7 : (int)lastDate.DayOfWeek;

                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;

                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDate <= bh && bh <= lastDate)
                    --businessDays;
            }

            return businessDays;
        }

       

    }
}
/*Payslip calculator - formulas
 * 
 * OMZ = baseSalary / ( 1 + annuity) / officialWorkingDays * actualWorkingDays
 * annuityPercentage = years * 0.6;
 * PTOamount = (baseSalary / officialWorkingDays) * PTOdays
 * compensationAmount = baseSalary / officialWorkingDays * compensationDays
 * holidaysAmount = baseSalary / officialWorkingDays * 2 * workedHolidays;
 * nonWorkingDayAmount = baseSalary / officialWorkingDays * 1.75 * workedNonWorkingDays
 * taxableIncome = baseSalary + holidaysAmount + nonWorkingDaysAmount + taxVoucher - insuranceTax
 * totalCompensation = baseSalary + holidaysAmount + nonWorkingDaysAmount + taxVoucher + voucher
 * taxedAmount = taxableIncome * tax
 * netIncome = totalCompensation - taxVoucher - voucher - taxedAmount - insuranceTax
 */
